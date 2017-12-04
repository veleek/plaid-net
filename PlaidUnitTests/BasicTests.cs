using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ben.Plaid.Test
{
    [TestClass]
    public class BasicTests : TestBase
    {
        [TestMethod]
        public async Task GetInstitutions()
        {
            List<Institution> institutions = await this.Client.GetInstitutionsAsync(1);

            Assert.IsNotNull(institutions);
            Assert.AreNotEqual(0, institutions.Count);

            institutions.Dump();
        }

        [TestMethod]
        public async Task GetInstitution()
        {
            Institution institution = await this.Client.GetInstitutionAsync(DefaultInstitutionId);

            Assert.IsNotNull(institution);

            institution.Dump();

            Assert.AreEqual(DefaultInstitutionId, institution.Id);
            Assert.AreEqual(DefaultInstitutionType, institution.Type);
            Assert.AreEqual(DefaultInstitutionName, institution.Name);
        }

        [TestMethod]
        public async Task AddAuth()
        {
            PlaidResponse auth =
                await this.Client.AddAuthAsync(PlaidTest.UserName, PlaidTest.Password, DefaultInstitutionType);

            Assert.IsNotNull(auth);
            auth.Dump();

            Assert.IsNotNull(auth.AccessToken);
            auth.AccessToken.Dump();
        }

        [TestMethod]
        public async Task AddAuthUsaa()
        {
            PlaidResponse auth =
                await this.Client.AddAuthAsync(PlaidTest.UserName, PlaidTest.Password, "usaa", PlaidTest.Pin);

            Assert.IsNotNull(auth);
            auth.Dump();

            Assert.IsNotNull(auth.AccessToken);
            auth.AccessToken.Dump();
        }

        [TestMethod]
        public async Task GetAuth()
        {
            string token = await this.GetTestAccessToken();

            PlaidResponse getAuth = await this.Client.GetAuthAsync(token);
            Assert.IsNotNull(getAuth);
            getAuth.Dump();
        }

        [TestMethod]
        public async Task AddConnect()
        {
            PlaidResponse addResponse =
                await this.Client.AddConnectAsync(PlaidTest.UserName, PlaidTest.Password, DefaultInstitutionType);
            addResponse.Dump();
        }

        [TestMethod]
        public async Task AddConnectOptions()
        {
            ConnectOptions options = new ConnectOptions {LoginOnly = false, StartDate = "5 days ago", List = true};
            PlaidResponse addResponse = await this.Client.AddConnectAsync(PlaidTest.UserName, PlaidTest.Password,
                DefaultInstitutionType, options: options);
            addResponse.Dump();
        }

        [TestMethod]
        public async Task GetTransactions()
        {
            string accessToken = await this.GetTestAccessToken();
            PlaidResponse getTransactionsResponse = await this.Client.GetTransactionsAsync(accessToken);
            getTransactionsResponse.Transactions.Dump();
        }

        [TestMethod]
        public async Task GetTransactionsOptions()
        {
            string accessToken = await this.GetTestAccessToken();

            TransactionOptions options = new TransactionOptions
            {
                MinimumDate = DateTime.UtcNow.AddDays(-7),
                MaximumDate = DateTime.UtcNow.AddDays(-1)
            };

            PlaidResponse getTransactionsResponse = await this.Client.GetTransactionsAsync(accessToken, options);
            getTransactionsResponse.Transactions.Dump();
        }

        [Ignore]
        [TestMethod]
        public async Task Scratch()
        {
            Institution institution = await this.Client.GetInstitutionAsync("fidelity");
        }
    }
}