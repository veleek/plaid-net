using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ben.Plaid
{
    public partial class PlaidClient
    {

        #region Auth Methods

        public async Task<PlaidResponse> AddAuthAsync(string username, string password, string type, string pin = null,
            AuthOptions options = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", this.clientId},
                {"secret", this.secret},
                {"username", username},
                {"password", password},
                {"type", type}
            };

            if (type.ToLower() == "usaa")
            {
                if (pin == null)
                    throw new ArgumentException("You must provide a pin code with USAA.", "pin");

                parameters.Add("pin", pin);
            }
            else if (pin != null)
            {
                throw new ArgumentException("Pin code should only be provided with USAA.", "pin");
            }

            if (options != null)
                parameters.Add("options", JsonConvert.SerializeObject(options));

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            string c = content.ReadAsStringAsync().Result;
            HttpResponseMessage response = await this.PostAsync("/auth", content);
            return await HandleResponseAsync<PlaidResponse>(response);
        }

        public async Task<PlaidResponse> GetAuthAsync(string accessToken)
        {
            if (accessToken == null)
                throw new ArgumentNullException("accessToken");

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", this.clientId},
                {"secret", this.secret},
                {"access_token", accessToken}
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);

            HttpResponseMessage response = await this.PostAsync("/auth/get", content);
            return await HandleResponseAsync<PlaidResponse>(response);
        }

        public async Task<PlaidResponse> UpdateAuthAsync(string accessToken, string username, string password,
            string pin = null)
        {
            if (accessToken == null)
                throw new ArgumentNullException("accessToken");

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", this.clientId},
                {"secret", this.secret},
                {"access_token", accessToken},
                {"username", username},
                {"password", password}
            };

            if (pin != null)
                parameters.Add("pin", pin);

            HttpRequestMessage updateRequest = new HttpRequestMessage(new HttpMethod("PATCH"), "/auth")
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            HttpResponseMessage response = await this.SendAsync(updateRequest);
            return await HandleResponseAsync<PlaidResponse>(response);
        }

        public async Task<PlaidResponse> GetBalanceAsync(string accessToken)
        {
            if (accessToken == null)
                throw new ArgumentNullException("accessToken");

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", this.clientId},
                {"secret", this.secret},
                {"access_token", accessToken}
            };

            HttpRequestMessage balanceRequest = new HttpRequestMessage(HttpMethod.Post, "/balance")
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            HttpResponseMessage response = await this.SendAsync(balanceRequest);
            return await HandleResponseAsync<PlaidResponse>(response);
        }

        #endregion
    }
}
