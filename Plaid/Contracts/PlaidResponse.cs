using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Ben.Plaid
{
    public class PlaidResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public List<PlaidAccount> Accounts { get; set; }

        public List<Transaction> Transactions { get; set; }

        public bool MfaStepRequired { get { return this.StatusCode == HttpStatusCode.Created; } }

        /// <summary>
        /// Gets or sets the type of the MFA required if MFA is required
        /// </summary>
        [JsonProperty("type")]
        public string MfaType { get; set; }

        public JToken Mfa { get; set; }

        public MfaQuestion[] Questions => this.MfaType == "questions" || this.MfaType == "selections" ? this.Mfa.ToObject<MfaQuestion[]>() : null;

        public MfaCodeType[] Codes => this.MfaType == "codes" ? this.Mfa.ToObject<MfaCodeType[]>() : null;

        public MfaCodeResult CodeResult => this.Mfa.ToObject<MfaCodeResult>();

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }

    public class MfaQuestion
    {
        /// <summary>
        /// Gets or sets them question of a MFA request
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the available answers for a selections based MFA request
        /// </summary>
        public string[] Answers { get; set; }
    }

    public class MfaCodeType
    {
        /// <summary>
        /// Gets or sets the mask for a code-based MFA option
        /// </summary>
        public string Mask { get; set; }

        /// <summary>
        /// Gets or sets the type of the mask for a given MFA code option.
        /// </summary>
        public string Type { get; set; }
    }

    public class MfaCodeResult
    {
        public string Message { get; set; }
    }
}