using System;
using System.Collections;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Ben.Plaid.Test
{
	public static class TestExtensions
	{
		public static void Dump(this object self)
		{
			DumpInternal(self);
		}

		public static void Dump(this object self, string description)
		{
			DumpInternal(self, description + ": ");
		}

		private static void DumpInternal(this object self, string description, int depth = 0)
		{
			DumpInternal(description + ": " + ToDumpString(self, depth), depth);
		}

		private static void DumpInternal(object self, int depth = 0)
		{
			IEnumerable selfEnumerable = self as IEnumerable;

			if (selfEnumerable != null)
			{
				int index = 0;
				foreach (var element in selfEnumerable)
				{
					DumpInternal(element, string.Format("[{0}]", index), depth+1);
					index++;
				}
			}
			else
			{
				DumpInternal(self.ToString(), depth);
			}
		}

		private static void DumpInternal(string self, int depth = 0, bool noNewLine = false)
		{
			string indent = new string(' ', depth * 4) + '\n';

			self = self.Replace("\r\n", "\n");
			self = self.Replace("\n", indent);

			Debug.WriteLine(self);
		}

		private static string ToDumpString(object o, int depth = 0)
		{
			string self = JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
			string indent = new string(' ', depth * 4) + '\n';

			self = self.Replace("\r\n", "\n");
			self = self.Replace("\n", indent);

			return self;
		}
	}
}