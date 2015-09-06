using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ben.Plaid
{
	public static class HttpContentExtensions
	{
		public static async Task<object> ReadAsJsonAsync(this HttpContent self)
		{
			string rawContent = await self.ReadAsStringAsync();
			object content = JsonConvert.DeserializeObject(rawContent);

			return content;
		}

		public static async Task<TContent> ReadAsJsonAsync<TContent>(this HttpContent self)
		{
			string rawContent = await self.ReadAsStringAsync();
			TContent content = JsonConvert.DeserializeObject<TContent>(rawContent);

			return content;
		}
	}
}
