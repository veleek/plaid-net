using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ben.Plaid.Contracts;

namespace Ben.Plaid
{
    public partial class PlaidClient : HttpClient
    {
        private readonly string clientId;
        private readonly string secret;

        public PlaidClient(string clientId, string secret)
            : this(clientId, secret, PlaidEnvironment.Production)
        {
        }

        public PlaidClient(string clientId, string secret, PlaidEnvironment environment)
        {
            this.clientId = clientId;
            this.secret = secret;
            this.BaseAddress = GetApiHost(environment);
        }

        public async Task<PlaidResponse> UpgradeUserAsync(string accessToken, string upgradeTo)
        {
            if (accessToken == null)
                throw new ArgumentNullException("accessToken");

            if (upgradeTo == null)
                throw new ArgumentNullException("upgradeTo");

            if (upgradeTo != "auth" && upgradeTo != "connect")
                throw new ArgumentException("You must specify either 'auth' or 'connect' to upgrade an account to");

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", this.clientId},
                {"secret", this.secret},
                {"access_token", accessToken},
                {"upgrade_to", upgradeTo}
            };

            HttpRequestMessage balanceRequest = new HttpRequestMessage(HttpMethod.Post, "/upgrade")
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            HttpResponseMessage response = await this.SendAsync(balanceRequest);
            return await HandleResponseAsync<PlaidResponse>(response);
        }

        public async Task<string> ExchangeToken(string publicToken)
        {
            if (string.IsNullOrWhiteSpace(publicToken))
                throw new ArgumentNullException("publicToken");

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", this.clientId},
                {"secret", this.secret},
                {"public_token", publicToken}
            };

            HttpRequestMessage exchangeTokenRequest = new HttpRequestMessage(HttpMethod.Post, "/exchange_token")
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            HttpResponseMessage response = await this.SendAsync(exchangeTokenRequest);
            PlaidResponse authContent = await HandleResponseAsync<PlaidResponse>(response);

            return authContent.AccessToken;
        }


        public async Task<List<Institution>> GetInstitutionsAsync(int count = 10, int offset = 0)
        {
            GetInstitutionsRequest request = new GetInstitutionsRequest
            {
                ClientId = this.clientId,
                Secret = this.secret,
                Count = count,
                Offset = offset,
            };

            HttpResponseMessage response = await this.PostAsync("/institutions/get", new JsonContent(request));

            var institutionsResponse = await HandleResponseAsync<GetInstitutionsResponse>(response);
            return institutionsResponse.Institutions;
        }

        public async Task<Institution> GetInstitutionAsync(string id)
        {
            HttpResponseMessage response = await this.GetAsync(string.Format("/institutions/{0}", id));

            return await HandleResponseAsync<Institution>(response);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            HttpResponseMessage response = await this.GetAsync("/categories");

            return await HandleResponseAsync<List<Category>>(response);
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            HttpResponseMessage response = await this.GetAsync(string.Format("/categories/{0}", id));

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
            if (plaidResponse != null)
                plaidResponse.StatusCode = response.StatusCode;

            return result;
        }

        private static Uri GetApiHost(PlaidEnvironment environment)
        {
            switch (environment)
            {
                case PlaidEnvironment.Sandbox:
                    return new Uri("https://sandbox.plaid.com");
                case PlaidEnvironment.Development:
                    return new Uri("https://development.plaid.com");
                case PlaidEnvironment.Production:
                    return new Uri("https://production.plaid.com");
                default:
                    throw new ArgumentOutOfRangeException("environment", string.Format("Unable to determine API host address for environment '{0}'", environment));
            }
        }
    }
}