using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.HttpWire.Tests;

/// <summary>
/// HttpMessageHandler that records the outbound request and returns a canned
/// response. Lets us instantiate a real Refit interface and inspect exactly
/// what HTTP call it would make — the layer Moq-on-the-interface tests skip.
/// </summary>
internal class CapturingHandler : HttpMessageHandler
{
    public HttpRequestMessage LastRequest { get; private set; }
    public string LastBody { get; private set; }
    public Encoding LastBodyEncoding { get; private set; }
    public HttpResponseMessage Response { get; set; } = new HttpResponseMessage(HttpStatusCode.OK)
    {
        Content = new StringContent(
            "{\"data\":null,\"status\":\"successful\",\"message\":\"\"}",
            Encoding.UTF8,
            "application/json"),
    };

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        if (request.Content != null)
        {
            LastBody = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            LastBodyEncoding = Encoding.UTF8;
        }
        return Response;
    }
}
