namespace commercetools.Sdk.HttpApi
{
    public class InMemoryAnonymousCredentialsStoreManager : InMemoryTokenStoreManager, IAnonymousCredentialsStoreManager
    {
        public string AnonymousId { get; set; }
    }
}