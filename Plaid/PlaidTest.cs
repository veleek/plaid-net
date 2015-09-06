using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ben.Plaid
{
	public static class PlaidTest
	{
		public const string ClientId = "test_id";
		public const string ClientSecret = "test_secret";

        public const string UserName = "plaid_test";
		public const string MfaUserName = "plaid_selections";
		public const string Password = "plaid_good";
		public const string LockedPassword = "plaid_locked";
		public const string Pin = "1234";

		public static string GetTestAccessToken(this Institution institution)
		{
			return GetTestAccessToken(institution.Type);
		}

		public static string GetTestAccessToken(string type)
		{
			return string.Format("test_{0}", type);
		}

	}
}
