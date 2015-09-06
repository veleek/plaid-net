# Plaid.NET

A .NET library for interacting with [Plaid](http://www.plaid.com).

## Usage

The current library includes only a simple client wrapper which defines all of the various Plaid contract classes and an HttpClient interface for calling most of the currently exposed APIs.  This means that nearly all of the functionality is available but for things such as MFA, the user will need to manually process the response and repeatedly call the auth API.  

```
PlaidClient client = new PlaidClient("<client id>", "<client secret>");

// Get all the available institutions.
List<Institution> institutions = await this.Client.GetInstitutionsAsync();

// Adding a user 
Institution institution = institutions.First();
PlaidResponse authResponse = await this.Client.AddAuthAsync("<user name>", "<password>", institution.Type);

// Pull out the access token and get all the transactions
string accessToken = authResponse.AccessToken;
PlaidResponse getTransactionsResponse = await this.Client.GetTransactionsAsync(accessToken);
```
