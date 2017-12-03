using Newtonsoft.Json;

namespace Ben.Plaid.Contracts
{
    public class PlaidRequest
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
    }
}
