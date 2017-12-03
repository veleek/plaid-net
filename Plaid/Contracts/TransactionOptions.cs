using System;
using Newtonsoft.Json;

namespace Ben.Plaid
{
    public class TransactionOptions
    {
        [JsonProperty("account")]
        public string AccountId { get; set; }

        public bool Pending { get; set; }

        [JsonProperty("gte")]
        public DateTime MinimumDate { get; set; }

        [JsonProperty("lte")]
        public DateTime MaximumDate { get; set; }
    }
}