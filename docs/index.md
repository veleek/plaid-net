---
title: Plaid.NET
description: Easy to use Plaid API bindings for the .NET Framework
---

# What does it offer

# Getting started

Install the [Plaid.NET NuGet Package](http://nuget.org/packages/plaid-net) (To Be Released) and start using Plaid from your code.

```csharp
PlaidClient client = new PlaidClient("<client id>", "<client secret>");

// Get all the available institutions.
List<Institution> institutions = await client.GetInstitutionsAsync();

// Adding a user 
Institution institution = institutions.First();
PlaidResponse authResponse = await client.AddAuthAsync("<user name>", "<password>", institution.Type);

// Pull out the access token and get all the transactions
string accessToken = authResponse.AccessToken;
PlaidResponse getTransactionsResponse = await client.GetTransactionsAsync(accessToken);
```
