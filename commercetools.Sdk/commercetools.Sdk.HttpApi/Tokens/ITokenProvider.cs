using commercetools.Sdk.HttpApi.Domain;

namespace commercetools.Sdk.HttpApi.Tokens
{
    public interface ITokenProvider
    {
        Token Token { get; }

        TokenFlow TokenFlow { get; }
    }
}