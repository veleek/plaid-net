using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ben.Plaid;

namespace PlaidBrowserModern
{
    public class MainViewModel
    {
        // In order to avoid exposing my client id and secret, but still making it easy to test, I'm just using
        // the DPAPI protected versions.  These can be easily replaced by anyone else, but are only usable
        // on my local computer
        private const string ProtectedClientId =
            "MIIBmAYJKoZIhvcNAQcDoIIBiTCCAYUCAQIxggEnooIBIwIBBDCB5gSBsgEAAADQjJ3fARXREYx6AMBPwpfrAQAAACesN0rJKj9Il5kjEkLowPIEAAAAAgAAAAAAA2YAAMAAAAAQAAAAL1GNcrWLMjTw7MyEy5sBlwAAAAAEgAAAoAAAABAAAABvB6zTJOoUWp836DE0OM/KKAAAAFmNA6NCe0qQFe/qsmA6W65t2vGLV5EGRWM8gPWmVq0FKHzeBu/mCBAUAAAARqT9hdU15k7zLIqDqvR+gTgbKEAwLwYJKwYBBAGCN0oBMCIGCisGAQQBgjdKAQgwFDASMBAMBUxPQ0FMDAdtYWNoaW5lMAsGCWCGSAFlAwQBLQQoraWIDU7A+XMPInG81A53ej83UR0d4XbhtITBUzJAdgbt/4HmldknVDBVBgkqhkiG9w0BBwEwHgYJYIZIAWUDBAEuMBEEDN2Zn3bbF6FSoxHhQAIBEIAoBd0f4YUgm7ylITuFqHrOZfYYkOXCBIOxmjGHTDTXL/9xVGE64RmjTA==";

        private const string ProtectedClientSecret =
            "MIIBngYJKoZIhvcNAQcDoIIBjzCCAYsCAQIxggEnooIBIwIBBDCB5gSBsgEAAADQjJ3fARXREYx6AMBPwpfrAQAAACesN0rJKj9Il5kjEkLowPIEAAAAAgAAAAAAA2YAAMAAAAAQAAAAOhz9sC6dYx8LiRzSCJOWSAAAAAAEgAAAoAAAABAAAACmsfRVe/KQYJI2S+ymU7XFKAAAAEL28Hwcq67DQ4+xeu5VWVz5bCKv2hoBoDIV+EYgfHa3iz4gfuXR1+UUAAAAWONaN2NsUnfDnHPtxaxVl2m3WggwLwYJKwYBBAGCN0oBMCIGCisGAQQBgjdKAQgwFDASMBAMBUxPQ0FMDAdtYWNoaW5lMAsGCWCGSAFlAwQBLQQoP4I/iaaHRES53k0PV/MdzpIEE0pUcvF8b+zjoiO/HdYUn1f0OiRh+jBbBgkqhkiG9w0BBwEwHgYJYIZIAWUDBAEuMBEEDB1Cv+UV6CynFv+CwwIBEIAuQ8sPwPat08XIP2K8Yi+O0bzvnn07P94A4GCy+JUCeRrdjmrXe5gBw+9SSlbsNw==";

        private const string DefaultInstitutionId = "5301aa096b3f822b440001cb";
        private const string DefaultInstitutionType = "wells";
        private const string DefaultInstitutionName = "Wells Fargo";

        private object RefreshClientsLock = new object();
        private Task<List<Institution>> RefreshClientsTask;

        private MainViewModel(PlaidClient client)
        {
            this.Client = client;
        }

        public PlaidClient Client { get; }

        public Task<List<Institution>> RefreshInstitutionsAsync()
        {
            if (RefreshClientsTask == null || RefreshClientsTask.IsCompleted)
            {
                lock (RefreshClientsLock)
                {
                    if (RefreshClientsTask == null || RefreshClientsTask.IsCompleted)
                    {
                        this.RefreshClientsTask = RefreshInstitutionsInternal();
                    }
                }
            }

            return this.RefreshClientsTask;
        }

        private Task<List<Institution>> RefreshInstitutionsInternal()
        {
            return this.Client.GetInstitutionsAsync();
        }

        public static async Task<MainViewModel> Create()
        {
            string clientId = await ProtectedData.UnprotectStringAsync(ProtectedClientId);
            string clientSecret = await ProtectedData.UnprotectStringAsync(ProtectedClientSecret);

            PlaidClient client = new PlaidClient(clientId, clientSecret);

            return new MainViewModel(client);
        }
    }
}
