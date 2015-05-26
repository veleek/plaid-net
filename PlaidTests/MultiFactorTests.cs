using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Ben.Plaid.Test
{
	[TestClass]
	public class MultiFactorTests : TestBase
	{
		[TestMethod]
		public async Task MultifactorAuthList()
		{
			var response = await this.Client.AddAuthAsync(PlaidTest.MfaUserName, PlaidTest.Password, "citi", new AuthOptions {List = true});
		}

		[TestMethod]
		public async Task MultifactorAuthStart()
		{
			var response = await this.Client.AddAuthAsync(PlaidTest.MfaUserName, PlaidTest.Password, "citi");
		}
	}
}
