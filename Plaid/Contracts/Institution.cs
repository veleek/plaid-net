using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ben.Plaid
{
    public class Institution
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        [JsonProperty("has_mfa")]
        public bool HasMultiFactorAuth { get; set; }

        [JsonProperty("mfa")]
        public List<string> MultiFactorAuth { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<PlaidProduct> Products { get; set; }

        public InstitutionCredentials[] Credentials { get; set; }

        public override string ToString()
        {
            return string.Format("{1} ({0}) Type: {2}, Products: {3}", this.Id, this.Name, this.Type, string.Join(", ", this.Products.Select(p => p.ToString())));
        }
    }
}