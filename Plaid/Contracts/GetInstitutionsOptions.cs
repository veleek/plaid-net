using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ben.Plaid.Contracts
{
    public class GetInstitutionsRequest : PlaidRequest
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public GetInstitutionsOptions Options { get; set; }
    }

    public class GetInstitutionsOptions
    {
        public string Products { get; set; }
    }

    public class GetInstitutionsResponse
    {
        [JsonProperty("institutions")]
        public List<Institution> Institutions { get; set; }
    }
}
