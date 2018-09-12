using commercetools.Sdk.HttpApi.Domain;

namespace commercetools.Sdk.HttpApi
{
    public class InMemoryTokenStoreManager : ITokenStoreManager
    {
        public Token Token { get; set; }
    }
}