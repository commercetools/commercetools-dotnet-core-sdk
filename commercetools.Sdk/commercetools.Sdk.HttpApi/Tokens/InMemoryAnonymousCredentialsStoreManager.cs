namespace commercetools.Sdk.HttpApi.Tokens
{
    public class InMemoryAnonymousCredentialsStoreManager : InMemoryTokenStoreManager, IAnonymousCredentialsStoreManager
    {
        public string AnonymousId { get; set; }
    }
}
