using commercetools.Sdk.HttpApi.Tokens;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Tokens
{
    public class InMemoryAnonymousCredentialsStoreManager : InMemoryTokenStoreManager, IAnonymousCredentialsStoreManager
    {
        public string AnonymousId { get; set; }
    }
}