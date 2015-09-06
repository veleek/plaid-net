using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ben.Plaid;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ben.Plaid.Test
{
    [TestClass]
    public class BasicTests : TestBase
    {
	    [TestMethod]
	    public async Task GetInstitutions()
	    {
		    var institutions = await this.Client.GetInstitutionsAsync();

		    Assert.IsNotNull(institutions);
		    Assert.AreNotEqual(0, institutions.Count);

			institutions.Dump();
		}

		[TestMethod]
		public async Task GetInstitution()
		{
			var institution = await this.Client.GetInstitutionAsync(DefaultInstitutionId);

			Assert.IsNotNull(institution);

			institution.Dump();

			Assert.AreEqual(DefaultInstitutionId, institution.Id);
			Assert.AreEqual(DefaultInstitutionType, institution.Type);
			Assert.AreEqual(DefaultInstitutionName, institution.Name);
		}

        [TestMethod]
        public async Task AddAuth()
        {
            var auth = await this.Client.AddAuthAsync(PlaidTest.UserName, PlaidTest.Password, DefaultInstitutionType);

            Assert.IsNotNull(auth);
            auth.Dump();

            Assert.IsNotNull(auth.AccessToken);
            auth.AccessToken.Dump();
        }

        [TestMethod]
        public async Task AddAuthUsaa()
        {
            var auth = await this.Client.AddAuthAsync(PlaidTest.UserName, PlaidTest.Password, "usaa", PlaidTest.Pin);

            Assert.IsNotNull(auth);
            auth.Dump();

            Assert.IsNotNull(auth.AccessToken);
            auth.AccessToken.Dump();
        }

        [TestMethod]
		public async Task GetAuth()
		{
			var token = await this.GetTestAccessToken();

			var getAuth = await this.Client.GetAuthAsync(token);
			Assert.IsNotNull(getAuth);
			getAuth.Dump();
		}

		[TestMethod]
		public async Task AddConnect()
		{
			var addResponse = await this.Client.AddConnectAsync(PlaidTest.UserName, PlaidTest.Password, DefaultInstitutionType);
			addResponse.Dump();
		}

		[TestMethod]
		public async Task AddConnectOptions()
		{
			var options = new ConnectOptions {LoginOnly = false, StartDate = "5 days ago", List = true};
			var addResponse = await this.Client.AddConnectAsync(PlaidTest.UserName, PlaidTest.Password, DefaultInstitutionType, options: options);
			addResponse.Dump();
		}

		[TestMethod]
		public async Task GetTransactions()
		{
			var accessToken = await this.GetTestAccessToken();
			var getTransactionsResponse = await this.Client.GetTransactionsAsync(accessToken);
			getTransactionsResponse.Transactions.Dump();
		}

		[TestMethod]
		public async Task GetTransactionsOptions()
		{
			var accessToken = await this.GetTestAccessToken();

			var options = new TransactionOptions
			{
				MinimumDate = DateTime.UtcNow.AddDays(-7),
				MaximumDate = DateTime.UtcNow.AddDays(-1),
			};

			var getTransactionsResponse = await this.Client.GetTransactionsAsync(accessToken, options);
			getTransactionsResponse.Transactions.Dump();
		}

		[Ignore]
		[TestMethod]
	    public async Task Scratch()
	    {
		    var institution = await this.Client.GetInstitutionAsync("fidelity");
	    }
    }
}
