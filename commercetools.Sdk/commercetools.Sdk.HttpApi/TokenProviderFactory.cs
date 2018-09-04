using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.HttpApi
{
    public class TokenProviderFactory : ITokenProviderFactory
    {
        private IEnumerable<ITokenProvider> tokenProviders;

        public TokenProviderFactory(IEnumerable<ITokenProvider> tokenProviders)
        {
            this.tokenProviders = tokenProviders;
        }
       
        public ITokenProvider GetTokenProviderByFlow(TokenFlow tokenFlow)
        {
            return tokenProviders.Where(x => x.TokenFlow == tokenFlow).FirstOrDefault();
        }        
    }
}