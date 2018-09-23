namespace commercetools.Sdk.HttpApi
{
    public class InMemoryUserCredentialsStoreManager : InMemoryTokenStoreManager, IUserCredentialsStoreManager
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}