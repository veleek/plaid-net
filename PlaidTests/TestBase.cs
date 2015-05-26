using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Ben.Plaid.Test
{
	[TestClass]
	public class TestBase
	{
		// In order to avoid exposing my client id and secret, but still making it easy to test, I'm just using
		// the DPAPI protected versions.  These can be easily replaced by anyone else, but are only usable
		// on my local computer
		private const string ProtectedClientId =
			"AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAtzho9uqYs0KQW3KkglRs6AQAAAACAAAAAAADZgAAwAAAABAAAADb1SU9JA8+VgbjsdPFq5TpAAAAAASAAACgAAAAEAAAAMTzKZtBJvnYj+cEnAYLHakgAAAA3RMf59q2SZSo1oZYbO5+HOoXAZLizUjHi2SvDS7VV5YUAAAAsgpFanuTt00tyfC9ngh0A77oZO8=";

		private const string ProtectedClientSecret =
			"AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAtzho9uqYs0KQW3KkglRs6AQAAAACAAAAAAADZgAAwAAAABAAAAAvHgmXNU6At2pxj8eHHOgCAAAAAASAAACgAAAAEAAAADvIo7fMaNBzDRmCvQTDfD0gAAAA3TBn61dmYG7owRDr7MYAM6wd5/ogdSLtoJE8ZoVdPkMUAAAAVNq1K5BQ4tATv1yl7jjx3u17+1k=";

		protected const string DefaultInstitutionId = "5301aa096b3f822b440001cb";
		protected const string DefaultInstitutionType = "wells";
		protected const string DefaultInstitutionName = "Wells Fargo";

		protected PlaidClient Client;

		[TestInitialize]
		public void TestInitialize()
		{
			//this.Client = new PlaidClient("54f60f8f0effc0670b8c92cd", "679d5445d341633d791e30cd829711");
			this.Client = new PlaidClient(UnprotectString(ProtectedClientId), UnprotectString(ProtectedClientSecret));
		}

		/// <summary>
		/// Unprotect a string containing DPAPI protected data from the current machine.
		/// </summary>
		/// <param name="encodedProtectedString">The Base64 encoded protected data</param>
		/// <returns>A UTF8 encoding string of the unprotected data</returns>
		protected static string UnprotectString(string encodedProtectedString)
		{
			byte[] protectedData = Convert.FromBase64String(encodedProtectedString);
			byte[] unprotectedData = ProtectedData.Unprotect(protectedData, null, DataProtectionScope.LocalMachine);

			return Encoding.UTF8.GetString(unprotectedData, 0, unprotectedData.Length);
		}

		/// <summary>
		/// Protectd a string for use on a local machine using DPAPI
		/// </summary>
		/// <param name="unprotectedString">The unprotected string to protect</param>
		/// <returns></returns>
		protected static string ProtectString(string unprotectedString)
		{
			byte[] unprotectedData = Encoding.UTF8.GetBytes(unprotectedString);
			byte[] protectedData = ProtectedData.Protect(unprotectedData, null, DataProtectionScope.LocalMachine);

			return Convert.ToBase64String(protectedData);
		}

		protected async Task<string> GetTestAccessToken()
		{
			var auth = await this.Client.AddAuthAsync(PlaidTest.UserName, PlaidTest.Password, DefaultInstitutionType);
			return auth.AccessToken;
		}
	}
}