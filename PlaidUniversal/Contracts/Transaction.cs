using System;
using Newtonsoft.Json;

namespace Ben.Plaid
{
	public class Transaction
	{
		[JsonProperty("_id")]
		public string Id { get; set; }

		[JsonProperty("_account")]
		public string AccountId { get; set; }

		public double Amount { get; set; }

		public DateTime Date { get; set; }

		public string Name { get; set; }

		public TransactionMetadata Meta { get; set; }

		public bool Pending { get; set; }

		public TransactionType Type { get; set; }

		public string[] Category;

		[JsonProperty("category_id")]
		public string CategoryId;
	}

	public class TransactionMetadata
	{
		public TransactionLocation Location { get; set; }
	}

	public class TransactionLocation
	{
		public string Address { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string Zip { get; set; }

		public Coordinates Coordinates { get; set; }
	}

	public class Coordinates
	{
		[JsonProperty("lat")]
		public double Latitude { get; set; }

		[JsonProperty("lon")]
		public double Longitude { get; set; }
	}

	public class TransactionType
	{
		public string Primary { get; set; }
	}

	public class TransactionScore
	{
		public double Name { get; set; }

		public TransactionLocationScore Location { get; set; }
	}

	public class TransactionLocationScore
	{
		public double Address { get; set; }

		public double City { get; set; }

		public double State { get; set; }

		public double Zip { get; set; }

		public double Coordinates { get; set; }
	}
}