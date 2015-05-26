using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ben.Plaid
{
	public class PlaidErrorResponse
	{
		public string Code { get; set; }

		public string Message { get; set; }

		public string Resolve { get; set; }

		public override string ToString()
		{
			return string.Format("Error: {0} ({1}).  {2}", this.Message, this.Code, this.Resolve);
		}
	}
}
