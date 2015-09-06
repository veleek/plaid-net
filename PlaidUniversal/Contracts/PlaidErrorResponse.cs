namespace Ben.Plaid
{
    public class PlaidErrorResponse
	{
		public int Code { get; set; }

		public string Message { get; set; }

		public string Resolve { get; set; }

		public override string ToString()
		{
			return string.Format("Error: {0} ({1}).  {2}", this.Message, this.Code, this.Resolve);
		}
	}
}
