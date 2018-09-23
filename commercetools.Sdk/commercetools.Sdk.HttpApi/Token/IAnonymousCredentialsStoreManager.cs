namespace commercetools.Sdk.HttpApi
{
    public interface IAnonymousCredentialsStoreManager : ITokenStoreManager
    {
        string AnonymousId { get; set; }
    }
}