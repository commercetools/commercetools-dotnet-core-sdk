namespace commercetools.Sdk.HttpApi
{
    public interface IUserCredentialsStoreManager : ITokenStoreManager
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}