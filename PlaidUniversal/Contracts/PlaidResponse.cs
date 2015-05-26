using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ben.Plaid
{
	public class PlaidResponse
	{
		public List<PlaidAccount> Accounts { get; set; }

		public List<Transaction> Transactions { get; set; }

		/// <summary>
		/// Gets or sets the type of the MFA required if MFA is required
		/// </summary>
		public string Type { get; set; }

		public JObject Mfa { get; set; }

		public MfaQuestion[] Questions => this.Mfa.ToObject<MfaQuestion[]>();

		public MfaCodeType[] Codes => this.Mfa.ToObject<MfaCodeType[]>();

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