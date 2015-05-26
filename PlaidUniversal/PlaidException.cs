using System;

namespace Ben.Plaid
{
	public class PlaidException : Exception
	{
		public PlaidException(PlaidErrorResponse error)
			: base(CreateMessage(error))
		{
		}

		private static string CreateMessage(PlaidErrorResponse error)
		{
			return string.Format("{0} ({1}): {2}", error.Message, error.Code, error.Resolve);
		}
	}
}