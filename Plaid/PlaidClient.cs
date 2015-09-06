using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ben.Plaid
{
	public class PlaidClient : HttpClient
	{
		private readonly string clientId;
		private readonly string clientSecret;

		public PlaidClient(string clientId, string secret, bool useProduction = false)
		{
			this.clientId = clientId;
			this.clientSecret = secret;
			this.BaseAddress = new Uri(useProduction ? "https://api.plaid.com" : "https://tartan.plaid.com");
		}

		#region Connect Methods

		public async Task<PlaidResponse> AddConnectAsync(string username, string password, string type, string pin = null, ConnectOptions options = null)
		{
			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"username", username},
				{"password", password},
				{"type", type},
			};

            if (type.ToLower() == "usaa")
            {
                if (pin == null)
                {
                    throw new ArgumentException("You must provide a pin code with USAA.", "pin");
                }

                parameters.Add("pin", pin);
            }
            else if (pin != null)
            {
                throw new ArgumentException("Pin code should only be provided with USAA.", "pin");
            }

            if (options != null)
			{
				parameters.Add("options", JsonConvert.SerializeObject(options));
			}

			var connectRequestContent = new FormUrlEncodedContent(parameters);
			var response = await this.PostAsync("/connect", connectRequestContent);
			return await HandleResponseAsync<PlaidResponse>(response);
		}

        /// <summary>
        /// Allows submitting one or more selections for selection based MFA
        /// </summary>
        /// <param name="accessToken">The access token for the request</param>
        /// <param name="selections">The list of selected answes from the available set.</param>
        /// <returns></returns>
        public Task<PlaidResponse> AddConnectStepAsync(string accessToken, string[] selections)
        {
            return AddConnectStepAsync(accessToken, JsonConvert.SerializeObject(selections));
        }

        public async Task<PlaidResponse> AddConnectStepAsync(string accessToken, string answer)
		{
			if (string.IsNullOrWhiteSpace(accessToken))
			{
				throw new ArgumentNullException("accessToken");
			}

			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"access_token", accessToken},
				{"mfa", answer },
			};

			var stepRequestContent = new FormUrlEncodedContent(parameters);
			var response = await this.PostAsync("/connect/step", stepRequestContent);
			return await HandleResponseAsync<PlaidResponse>(response);
		}

		public async Task<PlaidResponse> AddConnectStepAsync(string accessToken, string selection, MultiFactorAuthOptions options = null)
		{
			if (string.IsNullOrWhiteSpace(accessToken))
			{
				throw new ArgumentNullException("accessToken");
			}

			if (options == null)
			{
				throw new ArgumentNullException("options");
			}

			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"access_token", accessToken},
                {"mfa", selection },
				{"options", JsonConvert.SerializeObject(options)},
			};

			var stepRequestContent = new FormUrlEncodedContent(parameters);
			var response = await this.PostAsync("/connect/step", stepRequestContent);
			return await HandleResponseAsync<PlaidResponse>(response);
		}

		public async Task<PlaidResponse> GetTransactionsAsync(string accessToken, TransactionOptions options = null)
		{
			if (string.IsNullOrWhiteSpace(accessToken))
			{
				throw new ArgumentNullException("accessToken");
			}

			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"access_token", accessToken},
			};

			if (options != null)
			{
				parameters.Add("options", JsonConvert.SerializeObject(options));
			}

			var getTransactionsContent = new FormUrlEncodedContent(parameters);
			var response = await this.PostAsync("/connect/get", getTransactionsContent);
			return await HandleResponseAsync<PlaidResponse>(response);
		}

		#endregion

		#region Auth Methods

		public async Task<PlaidResponse> AddAuthAsync(string username, string password, string type, string pin = null, AuthOptions options = null)
		{
			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"username", username},
				{"password", password},
				{"type", type},
			};

            if (type.ToLower() == "usaa")
            {
                if (pin == null)
                {
                    throw new ArgumentException("You must provide a pin code with USAA.", "pin");
                }

                parameters.Add("pin", pin);
            }
            else if(pin != null)
            {
                throw new ArgumentException("Pin code should only be provided with USAA.", "pin");
            }

			if (options != null)
			{
				parameters.Add("options", JsonConvert.SerializeObject(options));
			}

			var content = new FormUrlEncodedContent(parameters);

			var response = await this.PostAsync("/auth", content);
			return await HandleResponseAsync<PlaidResponse>(response);
		}

		public async Task<PlaidResponse> GetAuthAsync(string accessToken)
		{
			if (accessToken == null)
			{
				throw new ArgumentNullException("accessToken");
			}

			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"access_token", accessToken},
			};

			var content = new FormUrlEncodedContent(parameters);

			var response = await this.PostAsync("/auth/get", content);
			return await HandleResponseAsync<PlaidResponse>(response);
		}

		public async Task<PlaidResponse> UpdateAuthAsync(string accessToken, string username, string password, string pin = null)
		{
			if (accessToken == null)
			{
				throw new ArgumentNullException("accessToken");
			}

			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"access_token", accessToken},
				{"username", username},
				{"password", password},
			};

			if (pin != null)
			{
				parameters.Add("pin", pin);
			}

			var updateRequest = new HttpRequestMessage(new HttpMethod("PATCH"), "/auth")
			{
				Content = new FormUrlEncodedContent(parameters),
			};

			var response = await this.SendAsync(updateRequest);
			return await HandleResponseAsync<PlaidResponse>(response);
		}

		public async Task<PlaidResponse> GetBalanceAsync(string accessToken)
		{
			if (accessToken == null)
			{
				throw new ArgumentNullException("accessToken");
			}

			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"access_token", accessToken},
			};

			var balanceRequest = new HttpRequestMessage(HttpMethod.Post, "/balance")
			{
				Content = new FormUrlEncodedContent(parameters),
			};

			var response = await this.SendAsync(balanceRequest);
			return await HandleResponseAsync<PlaidResponse>(response);
		}

		#endregion

		public async Task<PlaidResponse> UpgradeUserAsync(string accessToken, string upgradeTo)
		{
			if (accessToken == null)
			{
				throw new ArgumentNullException("accessToken");
			}

			if (upgradeTo == null)
			{
				throw new ArgumentNullException("upgradeTo");
			}

			if (upgradeTo != "auth" && upgradeTo != "connect")
			{
				// Note: should we do this, or just let the server handle it?  Might be better not to 
				// just in case it changes to support more services.
				throw new ArgumentException("You must specify either 'auth' or 'connect' to upgrade an account to");
			}

			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"access_token", accessToken},
				{"upgrade_to", upgradeTo},
			};

			var balanceRequest = new HttpRequestMessage(HttpMethod.Post, "/upgrade")
			{
				Content = new FormUrlEncodedContent(parameters),
			};

			var response = await this.SendAsync(balanceRequest);
			return await HandleResponseAsync<PlaidResponse>(response);
		}

		public async Task<string> ExchangeToken(string publicToken)
		{
			if (string.IsNullOrWhiteSpace(publicToken))
			{
				throw new ArgumentNullException("publicToken");
			}

			var parameters = new Dictionary<string, string>()
			{
				{"client_id", this.clientId},
				{"secret", this.clientSecret},
				{"public_token", publicToken},
			};

			var exchangeTokenRequest = new HttpRequestMessage(HttpMethod.Post, "/exchange_token")
			{
				Content = new FormUrlEncodedContent(parameters),
			};

			var response = await this.SendAsync(exchangeTokenRequest);
			var authContent = await HandleResponseAsync<PlaidResponse>(response);

			return authContent.AccessToken;
		}


		public async Task<List<Institution>> GetInstitutionsAsync()
		{
			var response = await this.GetAsync("/institutions");

			return await HandleResponseAsync<List<Institution>>(response);
		}

		public async Task<Institution> GetInstitutionAsync(string id)
		{
			var response = await this.GetAsync(string.Format("/institutions/{0}", id));

			return await HandleResponseAsync<Institution>(response);
		}

		public async Task<List<Category>> GetCategoriesAsync()
		{
			var response = await this.GetAsync("/categories");

			return await HandleResponseAsync<List<Category>>(response);
		}

		public async Task<Category> GetCategoryAsync(int id)
		{
			var response = await this.GetAsync(string.Format("/categories/{0}", id));

			return await HandleResponseAsync<Category>(response);
		}

		private static async Task<TResult> HandleResponseAsync<TResult>(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				PlaidErrorResponse error = await response.Content.ReadAsJsonAsync<PlaidErrorResponse>();
				throw new PlaidException(error);
			}

			TResult result = await response.Content.ReadAsJsonAsync<TResult>();

            PlaidResponse plaidResponse = result as PlaidResponse;
            if(plaidResponse != null)
            {
                plaidResponse.StatusCode = response.StatusCode;
            }

			return result;
		}
	}
}