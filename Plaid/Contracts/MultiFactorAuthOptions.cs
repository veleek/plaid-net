using Newtonsoft.Json;

namespace Ben.Plaid
{
    public class MultiFactorAuthOptions
    {
        [JsonProperty("send_method")]
        public MultiFactorAuthSendMethod SendMethod { get; set; }
    }

    public class MultiFactorAuthSendMethod
    {
        public string Type { get; set; }
        public string Mask { get; set; }
    }
}
