using commercetools.Sdk.HttpApi.Tokens;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Tokens
{
    public class InMemoryUserCredentialsStoreManager : InMemoryTokenStoreManager, IUserCredentialsStoreManager
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}