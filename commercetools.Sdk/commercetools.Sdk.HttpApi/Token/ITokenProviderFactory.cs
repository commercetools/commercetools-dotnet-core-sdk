namespace commercetools.Sdk.HttpApi
{
    public interface ITokenProviderFactory
    {
        ITokenProvider GetTokenProviderByFlow(TokenFlow tokenFlow);
    }
}