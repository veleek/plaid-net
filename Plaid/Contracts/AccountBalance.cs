namespace Ben.Plaid
{
	public class AccountBalance
	{
		public double? Current { get; set; }
		public double? Available { get; set; }

        public override string ToString()
        {
            return string.Format("Current: {0:C} Available: {1:C}", Current ?? (object)"<N/A>", Available ?? (object)"<N/A>");
        }
    }
}