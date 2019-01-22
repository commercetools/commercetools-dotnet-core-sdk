namespace commercetools.Sdk.HttpApi.Tokens
{
    public interface ITokenProviderFactory
    {
        ITokenProvider GetTokenProviderByFlow(TokenFlow tokenFlow);
    }
}