using Newtonsoft.Json;

namespace Ben.Plaid
{
	public class ConnectOptions
	{
		public string WebHook { get; set; }

		public bool Pending { get; set; }

		[JsonProperty("login_only")]
		public bool LoginOnly { get; set; }

		public bool List { get; set; }

		public string StartDate { get; set; }

		public string EndDate { get; set; }
	}
}