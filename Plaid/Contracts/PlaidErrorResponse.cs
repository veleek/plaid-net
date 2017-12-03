namespace Ben.Plaid
{
    public class PlaidErrorResponse
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Resolve { get; set; }

        public override string ToString()
        {
            return $"Error: {this.Message} ({this.Code}).  {this.Resolve}";
        }
    }
}
