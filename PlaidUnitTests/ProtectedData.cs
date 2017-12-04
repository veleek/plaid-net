namespace Ben.Plaid.Test
{
    public class ProtectedData
    {
        public static byte[] Unprotect(byte[] data, DataProtectionScope scope)
        {
            return data;
        }

        public static byte[] Protect(byte[] data, DataProtectionScope scope)
        {
            return data;
        }
    }

    public enum DataProtectionScope
    {
        LocalMachine,
        CurrentUser,
    }
}
