using System;
using Newtonsoft.Json;

namespace Ben.Plaid
{
    /// <summary>
    /// Details about an individual transaction.
    /// </summary>
	public class Transaction
	{
        /// <summary>
        /// The unique id of the transaction.
        /// </summary>
		[JsonProperty("_id")]
		public string Id { get; set; }

        /// <summary>
        /// The id of the account in which this transaction occurred.
        /// </summary>
		[JsonProperty("_account")]
		public string AccountId { get; set; }

        /// <summary>
        /// The settled dollar value.Positive values when money moves out of the account; negative values when money moves in.
        /// </summary>
		public double Amount { get; set; }

        /// <summary>
        /// The date that the transaction was posted by the financial institution.
        /// </summary>
		public DateTime Date { get; set; }

        /// <summary>
        /// The name of the merchant/account associated with the transaction.
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Additional metadata about a transaction
        /// </summary>
		public TransactionMetadata Meta { get; set; }

        /// <summary>
        /// When true, identifies the transaction as pending or unsettled.  Pending transaction details (name, type, amount) may change before they are settled.
        /// </summary>
		public bool Pending { get; set; }

        /// <summary>
        /// Gets or sets the id of a posted transaction's associated pending transcation - where applicable.
        /// </summary>
        [JsonProperty("_pendingTransaction")]
        public string PendingTransactionId { get; set; }

        /// <summary>
        /// The type of the transaction.
        /// </summary>
		public TransactionType Type { get; set; }

        /// <summary>
        /// An hierarchical array of the categories to which this transaction belongs. See category.
        /// </summary>
		public string[] Category { get; set; }

        /// <summary>
        /// The id of the category to which this transaction belongs. See category.
        /// </summary>
		[JsonProperty("category_id")]
		public string CategoryId { get; set; }

        /// <summary>
        /// A numeric representation of our confidence in the meta data we attached to the transaction. In the case of a score &lt;.9 we will default to guaranteed and known information.
        /// </summary>
        public TransactionScore Score { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1:C} - {2:d} - {3} | {4} | {5}", Name, Amount, Date.Date, Pending ? "Pending" : "Complete", Type, string.Join(", ", Category ?? new string[0]));
        }
    }

	public class TransactionMetadata
	{
        /// <summary>
        /// Detailed merchant location data including address, city, state, zip code, and geocoordinates where available.
        /// </summary>
		public TransactionLocation Location { get; set; }

        /// <summary>
        /// Phone number associated with the merchant.
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Foursquare ID where available.
        /// </summary>
        public string Ids { get; set; }
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

    /// <summary>
    /// Information about the type of a transaction.
    /// </summary>
	public class TransactionType
	{
        /// <summary>
        /// Gets or sets the primary transaction type.
        /// </summary>
		public PrimaryTransactionType Primary { get; set; }

        public override string ToString()
        {
            return Primary.ToString();
        }
    }

    /// <summary>
    /// An enumeration of the various different primary types of transactions.
    /// </summary>
    public enum PrimaryTransactionType
    {
        None,
        Place,
        Digital,
        Special,
    }

    /// <summary>
    /// A numeric representation of our confidence in the metadata associated with a transaction.
    /// </summary>
	public class TransactionScore
	{
        /// <summary>
        /// The overall confidence for the request.
        /// </summary>
		public double Master { get; set; }

        /// <summary>
        /// Details about the confidence of specific values the provided metadata.
        /// </summary>
		public TransactionDetailScore Detail { get; set; }
	}

    /// <summary>
    /// Details about the confidence of specific values the provided metadata.
    /// </summary>
	public class TransactionDetailScore
	{
        public double Name { get; set; }

		public double Address { get; set; }

		public double City { get; set; }

		public double State { get; set; }

		public double Zip { get; set; }

		public double Coordinates { get; set; }
	}
}