namespace commercetools.Sdk.HttpApi
{
    using System.Collections.Generic;
    using System.Linq;

    public class TokenProviderFactory : ITokenProviderFactory
    {
        private readonly IEnumerable<ITokenProvider> tokenProviders;

        public TokenProviderFactory(IEnumerable<ITokenProvider> tokenProviders)
        {
            this.tokenProviders = tokenProviders;
        }

        public ITokenProvider GetTokenProviderByFlow(TokenFlow tokenFlow)
        {
            return this.tokenProviders.FirstOrDefault(x => x.TokenFlow == tokenFlow);
        }
    }
}