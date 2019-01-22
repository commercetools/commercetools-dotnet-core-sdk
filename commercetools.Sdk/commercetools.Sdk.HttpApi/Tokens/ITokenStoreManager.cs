using commercetools.Sdk.HttpApi.Domain;

namespace commercetools.Sdk.HttpApi.Tokens
{
    public interface ITokenStoreManager
    {
        Token Token { get; set; }
    }
}