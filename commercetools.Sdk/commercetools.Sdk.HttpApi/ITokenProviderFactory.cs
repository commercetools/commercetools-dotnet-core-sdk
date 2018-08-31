namespace commercetools.Sdk.HttpApi
{
    public interface ITokenProviderFactory
    {
        ITokenProvider GetTokenProviderForClient(string clientName);

        ITokenProvider GetTokenProviderByFlow(TokenFlow tokenFlow);

        void RegisterTokenProvider(ITokenProvider tokenProvider);
    }
}