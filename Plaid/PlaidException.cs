using System;

namespace Ben.Plaid
{
    public class PlaidException : Exception
    {
        public PlaidException(PlaidErrorResponse error)
            : base(error.ToString())
        {
            this.Error = error;
        }

        public PlaidErrorResponse Error { get; }
    }
}