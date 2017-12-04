using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ben.Plaid
{
    public partial class PlaidClient
    {
        public async Task<PlaidResponse> AddConnectAsync(string username, string password, string type,
            string pin = null, ConnectOptions options = null)
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

            FormUrlEncodedContent connectRequestContent = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await this.PostAsync("/connect", connectRequestContent);
            return await HandleResponseAsync<PlaidResponse>(response);
        }

        /// <summary>
        ///     Allows submitting one or more selections for selection based MFA
        /// </summary>
        /// <param name="accessToken">The access token for the request</param>
        /// <param name="selections">The list of selected answes from the available set.</param>
        /// <returns></returns>
        public Task<PlaidResponse> AddConnectStepAsync(string accessToken, string[] selections)
        {
            return this.AddConnectStepAsync(accessToken, JsonConvert.SerializeObject(selections));
        }

        public async Task<PlaidResponse> AddConnectStepAsync(string accessToken, string answer)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentNullException("accessToken");

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", this.clientId},
                {"secret", this.secret},
                {"access_token", accessToken},
                {"mfa", answer}
            };

            FormUrlEncodedContent stepRequestContent = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await this.PostAsync("/connect/step", stepRequestContent);
            return await HandleResponseAsync<PlaidResponse>(response);
        }

        public async Task<PlaidResponse> AddConnectStepAsync(string accessToken, string selection,
            MultiFactorAuthOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentNullException("accessToken");

            if (options == null)
                throw new ArgumentNullException("options");

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", this.clientId},
                {"secret", this.secret},
                {"access_token", accessToken},
                {"mfa", selection},
                {"options", JsonConvert.SerializeObject(options)}
            };

            FormUrlEncodedContent stepRequestContent = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await this.PostAsync("/connect/step", stepRequestContent);
            return await HandleResponseAsync<PlaidResponse>(response);
        }

        public async Task<PlaidResponse> GetTransactionsAsync(string accessToken, TransactionOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentNullException("accessToken");

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"client_id", this.clientId},
                {"secret", this.secret},
                {"access_token", accessToken}
            };

            if (options != null)
                parameters.Add("options", JsonConvert.SerializeObject(options));

            FormUrlEncodedContent getTransactionsContent = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await this.PostAsync("/connect/get", getTransactionsContent);
            return await HandleResponseAsync<PlaidResponse>(response);
        }

    }
}
