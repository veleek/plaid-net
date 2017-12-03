using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ben.Plaid.Test
{
    [TestClass]
    public class MultiFactorTests : TestBase
    {
        [TestMethod]
        public async Task MultifactorAuthList()
        {
            var response = await this.Client.AddAuthAsync(PlaidTest.MfaUserName, PlaidTest.Password, "citi", options: new AuthOptions {List = true});
        }

        [TestMethod]
        public async Task MultifactorAuthStart()
        {
            var response = await this.Client.AddAuthAsync(PlaidTest.MfaUserName, PlaidTest.Password, "citi");
        }

        [TestMethod]
        public async Task MultifactorAuthStepWithSelections()
        {
            var startResponse = await this.Client.AddConnectAsync(PlaidTest.MfaUserName, PlaidTest.Password, "citi");
            Assert.AreEqual("selections", startResponse.MfaType);

            string[] selections = startResponse.Questions.Select(q =>
            {
                if (q.Answers.Contains("tomato")) return "tomato";
                if (q.Answers.Contains("ketchup")) return "ketchup";

                throw new ArgumentException("Unable to determine an appropriate answer.");
            }).ToArray();

            var stepResponse = await this.Client.AddConnectStepAsync(startResponse.AccessToken, selections);
            Assert.IsNotNull(stepResponse);
        }

        [TestMethod]
        public async Task MultifactorAuthStepWithQuestions()
        {
            var startResponse = await this.Client.AddConnectAsync(PlaidTest.UserName, PlaidTest.Password, "usaa", PlaidTest.Pin);
            Assert.IsTrue(startResponse.MfaStepRequired, "Auth Start");
            Assert.AreEqual("questions", startResponse.MfaType, "MFA Type");
            Assert.IsNotNull(startResponse.Questions);
            Assert.AreEqual(1, startResponse.Questions.Length);

            // USAA Has 3 questions
            PlaidResponse stepResponse;
            stepResponse = await this.Client.AddConnectStepAsync(startResponse.AccessToken, "again");
            Assert.IsTrue(stepResponse.MfaStepRequired, "First MFA Step");

            stepResponse = await this.Client.AddConnectStepAsync(startResponse.AccessToken, "again");
            Assert.IsTrue(stepResponse.MfaStepRequired, "Second MFA Step");

            stepResponse = await this.Client.AddConnectStepAsync(startResponse.AccessToken, "tomato");
            Assert.IsFalse(stepResponse.MfaStepRequired, "Final MFA Step");
            Assert.IsNotNull(stepResponse.Accounts);
            Assert.IsNotNull(stepResponse.Transactions);
        }
    }
}
