using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ben.Plaid.Test
{
    [TestClass]
    public class TestBase
    {
        private const string ProtectedClientId = "<Redacted>";
        private const string ProtectedClientSecret = "<Redacted>";

        protected const string DefaultInstitutionId = "5301aa096b3f822b440001cb";
        protected const string DefaultInstitutionType = "wells";
        protected const string DefaultInstitutionName = "Wells Fargo";

        protected PlaidClient Client;

        [TestInitialize]
        public void TestInitialize()
        {
            this.Client = new PlaidClient(UnprotectString(ProtectedClientId), UnprotectString(ProtectedClientSecret), PlaidEnvironment.Sandbox);
        }

        /// <summary>
        ///     Unprotect a string containing DPAPI protected data from the current machine.
        /// </summary>
        /// <param name="encodedProtectedString">The Base64 encoded protected data</param>
        /// <returns>A UTF8 encoding string of the unprotected data</returns>
        protected static string UnprotectString(string encodedProtectedString)
        {
            byte[] protectedData = Convert.FromBase64String(encodedProtectedString);
            byte[] unprotectedData = ProtectedData.Unprotect(protectedData, DataProtectionScope.LocalMachine);

            return Encoding.UTF8.GetString(unprotectedData, 0, unprotectedData.Length);
        }

        /// <summary>
        ///     Protectd a string for use on a local machine using DPAPI
        /// </summary>
        /// <param name="unprotectedString">The unprotected string to protect</param>
        /// <returns></returns>
        protected static string ProtectString(string unprotectedString)
        {
            byte[] unprotectedData = Encoding.UTF8.GetBytes(unprotectedString);
            byte[] protectedData = ProtectedData.Protect(unprotectedData, DataProtectionScope.LocalMachine);

            return Convert.ToBase64String(protectedData);
        }

        protected async Task<string> GetTestAccessToken()
        {
            PlaidResponse auth =
                await this.Client.AddAuthAsync(PlaidTest.UserName, PlaidTest.Password, DefaultInstitutionType);
            return auth.AccessToken;
        }
    }
}