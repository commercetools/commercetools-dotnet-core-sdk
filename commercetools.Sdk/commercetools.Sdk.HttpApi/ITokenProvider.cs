using commercetools.Sdk.HttpApi.Domain;

namespace commercetools.Sdk.HttpApi
{
    public interface ITokenProvider
    {
        Token Token { get; }
        TokenFlow TokenFlow { get; }
    }
}