namespace commercetools.Sdk.HttpApi.Tokens
{
    public interface IUserCredentialsStoreManager : ITokenStoreManager
    {
        string Username { get; }

        string Password { get; }
    }
}