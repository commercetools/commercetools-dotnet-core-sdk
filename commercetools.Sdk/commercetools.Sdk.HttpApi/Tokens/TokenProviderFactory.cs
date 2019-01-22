using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.HttpApi.Tokens
{
    internal class TokenProviderFactory : ITokenProviderFactory
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