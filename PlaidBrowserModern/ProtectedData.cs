using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace PlaidBrowserModern
{
    public static class ProtectedData
    {
        /// <summary>
        /// Unprotect a string containing data protection provider protected data from the current machine.
        /// </summary>
        /// <param name="encodedProtectedString">The Base64 encoded protected data</param>
        /// <returns>A UTF8 encoding string of the unprotected data</returns>
        public static async Task<string> UnprotectStringAsync(string encodedProtectedString)
        {
            DataProtectionProvider provider = new DataProtectionProvider(); 
            byte[] protectedData = Convert.FromBase64String(encodedProtectedString);
            byte[] unprotectedData = (await provider.UnprotectAsync(protectedData.AsBuffer())).ToArray();
            return Encoding.UTF8.GetString(unprotectedData, 0, unprotectedData.Length);
        }

        /// <summary>
        /// Protectd a string for use on a local machine using the data protection providers.
        /// </summary>
        /// <param name="unprotectedString">The unprotected string to protect</param>
        /// <returns></returns>
        public static async Task<string> ProtectStringAsync(string unprotectedString)
        {
            var provider = new DataProtectionProvider("LOCAL=machine");
            byte[] unprotectedData = Encoding.UTF8.GetBytes(unprotectedString);
            IBuffer protectedBuffer = await provider.ProtectAsync(unprotectedData.AsBuffer());
            byte[] protectedData = protectedBuffer.ToArray();

            return Convert.ToBase64String(protectedData);
        }
    }
}
