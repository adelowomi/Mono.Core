# Lookup — Identity & business verification

`IMonoLookUp` wraps Mono's identity-verification surface: BVN (with consent),
NIN, TIN, drivers license, international passport, address, account number,
mashup, credit history, CAC business search + profile + status report.

Uses `LookupSecretKey ?? SecretKey` ([configuration](./configuration.md#per-product-keys)).

## BVN — consent-based identity lookup

Three-step OTP flow (BVN ownership requires customer consent under NDPR /
CBN rules):

```csharp
// 1. Initiate — sends OTP to phone numbers on file with BVN
var init = await _lookup.InitiateBvnLookUp(new InitiateBvnLookUpModel
{
    Bvn   = "12345678901",
    Scope = ScopeConstants.Identity, // or BankAccounts for linked-accounts info
});
var sessionId = init.Data.SessionId;
// init.Data.Methods lists the phone numbers / emails OTP was sent to

// 2. User picks a method, enters the OTP → you call verify
await _lookup.VerifyBvnLookUp(new VerifyBvnLookUpOtpModel
{
    Method      = "alternate_phone", // from init.Data.Methods
    PhoneNumber = "08012345678",
}, sessionId);

// 3. Mono validates, then you fetch the BVN details
var details = await _lookup.GetBvnDetails(new BvnDetailsModel { Otp = userEnteredOtp }, sessionId);
```

`sessionId` is the same value throughout — sent as the `x-session-id`
header automatically.

## NIN

Two flavors: JSON (synchronous) and PDF (async job).

```csharp
// JSON — synchronous
var nin = await _lookup.GetNin(new NinRequestModel { Nin = "12345678901" });

// PDF — async; returns a job id to poll
var job = await _lookup.GetNinPdf(new NinPdfRequestModel { Nin = "12345678901" });

// Poll until completed
var poll = await _lookup.PollNinJob(job.Data.JobId);
if (poll.Data.Status == NinJobStatusConstants.Completed)
{
    var pdfUrl = poll.Data.Url; // 7-day expiry
}
```

Mashup combines BVN + NIN + DOB verification in one call — useful for KYC
flows that just need a yes/no:

```csharp
var mashup = await _lookup.GetMashUp(new MashUpRequestModel
{
    Bvn         = "12345678901",
    Nin         = "23456789012",
    DateOfBirth = "1990-01-15",
});
```

## CAC — Nigerian business registry

```csharp
// Search by name
var search = await _lookup.GetCacLookUp("ACME LIMITED");
var business = search.Data.First();
var businessId = business.Id.ToString();
var rcNumber = business.RcNumber;

// Aggregate profile by RC number (business + directors + shareholders + secretaries + PSC)
var profile = await _lookup.GetCacProfile(rcNumber);

// PSC (Persons with Significant Control) by numeric business id
var psc = await _lookup.GetCacPsc(businessId);
foreach (var person in psc.Data)
{
    Console.WriteLine($"{person.Name}: {person.OwnershipPercentage}%");
}

// Shareholders / directors / secretaries individually
var shareholders = await _lookup.GetCacCompany(businessId);
var directors    = await _lookup.GetDirectors(businessId);
var secretaries  = await _lookup.GetSecretary(businessId);

// Compliance status report (PDF)
var report = await _lookup.GetCacStatusReport(businessId);
await File.WriteAllBytesAsync("status.pdf", report.Data);
```

**Path-parameter wrinkle:** `GetCacProfile` takes the **RC number**
(`"RC123456"`) while every other CAC endpoint takes the **numeric business
id** Mono returned from `GetCacLookUp`. Flagged in the XML doc.

### Deprecated CAC endpoints

`GetPreviousAddress` and `GetChangeOfName` are marked `[Obsolete]` —
Mono retired the underlying endpoints. Use `GetCacProfile` instead, which
aggregates the same data.

## Other lookups

```csharp
// Bank account → name + masked BVN
var acct = await _lookup.GetAccountNumber(new AccountRequestModel
{
    NipCode       = "000014",
    AccountNumber = "0123456789",
});

// Drivers license
var dl = await _lookup.GetDriverLicense(new DriversLicenseRequestModel
{
    LicenseNumber = "ABC12345AA12",
    FirstName     = "Ada",
    LastName      = "Lovelace",
    DateOfBirth   = "1990-01-15",
});

// International passport
var pp = await _lookup.GetPassport(new InternationalPassportRequestModel
{
    PassportNumber = "A12345678",
    LastName       = "Lovelace",
});

// TIN
var tin = await _lookup.GetTin(new TinRequestModel
{
    Number  = "12345678-0001",
    Channel = "TIN",
});

// Address (electricity-meter based)
var addr = await _lookup.GetAddress(new AddressLookUpRequestModel
{
    MeterNumber = "12345678901",
    Address     = "12 Analytical St, Lagos",
});

// Credit history — provider is "crc" | "xds" | "all"
var credit = await _lookup.GetCreditHistory("crc", new CreditHistoryRequestModel
{
    Bvn = "12345678901",
});

// NIP-supported banks list (for InitiateAccountLinking institution selection)
var banks = await _lookup.GetBanks();
```

## Pricing

Each lookup is billed per call. The Mono dashboard has the per-product
breakdown. Failed lookups (invalid identity, no match) are still billed —
[changelog February 2026](./CHANGELOG.md).

## Common errors

| Mono message | Likely cause |
|---|---|
| `"Please use a Lookup app, ..."` | Your secret key is for the wrong product — see [configuration](./configuration.md#per-product-keys) |
| `"Invalid BVN"` | 11 digits, all numeric, exists in NIBSS records |
| `"Invalid NIN"` | 11 digits; doesn't match a NIMC record |
| `"Lookup not found"` | The identity exists but Mono's data source returned no match (try a different lookup, e.g. TIN-via-CAC channel) |
