namespace Ben.Plaid
{
    /// <summary>
    /// Indicates a specific Plaid API environment.
    /// </summary>
    public enum PlaidEnvironment
    {
        Unknown,

        /// <summary>
        /// Used to test against Plaid's sandbox environment
        /// using fake user credentials and accounts.
        /// </summary>
        Sandbox,

        /// <summary>
        /// Used to test with live users and credentials.
        /// </summary>
        Development,

        /// <summary>
        /// Used for live services.
        /// </summary>
        Production
    }
}
