namespace commercetools.Sdk.HttpApi.Tokens
{
    public interface IAnonymousCredentialsStoreManager : ITokenStoreManager
    {
        string AnonymousId { get; }
    }
}