using Refit;

namespace Mono.Core.Accounts
{
    public class StatementRequestModels
    { 
        [AliasAs("period")]
        public string Period { get; set; }
        [AliasAs("output")]
        public string Output { get; set; }
    }

    /// <summary>
    /// Query params for the transactions endpoint
    /// </summary>
    public class AccountTransactionsOptionsRequest
    {
        [AliasAs("start")]
        public string Start { get; set; }
        [AliasAs("end")]
        public string End { get; set; }
        [AliasAs("narration")]
        public string Narration { get; set; }
        [AliasAs("type")]
        public string Type { get; set; } 
        [AliasAs("paginate")]
        public bool Paginate { get; set; } 
        [AliasAs("limit")]
        public int Limit { get; set; } 
    }
}
