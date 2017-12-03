using Newtonsoft.Json;

namespace Ben.Plaid
{
    public class PlaidAccount
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        public string Type { get; set; }
        public AccountBalance Balance { get; set; }
        public AccountMetadata Meta { get; set; }
    }
}