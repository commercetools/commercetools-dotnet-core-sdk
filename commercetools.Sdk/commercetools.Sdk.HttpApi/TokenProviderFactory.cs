using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.HttpApi
{
    public class TokenProviderFactory : ITokenProviderFactory
    {
        // TODO, see how to register providers
        private IList<ITokenProvider> registeredTokenProviders = new List<ITokenProvider>();

        public ITokenProvider GetTokenProviderByFlow(TokenFlow tokenFlow)
        {
            return registeredTokenProviders.Where(x => x.TokenFlow == tokenFlow).FirstOrDefault();
        }

        public ITokenProvider GetTokenProviderForClient(string clientName)
        {
            return null;
        }

        public void RegisterTokenProvider(ITokenProvider tokenProvider)
        {
            registeredTokenProviders.Add(tokenProvider);
        }
    }
}