using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.HttpApi.Tokens
{
    internal class TokenProviderFactory : ITokenProviderFactory
    {
        private IDictionary<TokenFlow, Type> tokenProviderTypes;

        public TokenProviderFactory(ITypeRetriever retriever)
        {
            this.tokenProviderTypes = new Dictionary<TokenFlow, Type>();
            foreach (var provider in retriever.GetTypes<ITokenProvider>())
            {
                var flow = ((FlowAttribute)provider.GetCustomAttribute(typeof(FlowAttribute))).TokenFlow;
                this.tokenProviderTypes.Add(flow, provider);
            }
        }

        public Type GetTokenProviderByFlow(TokenFlow tokenFlow)
        {
            return this.tokenProviderTypes.FirstOrDefault(pair => pair.Key == tokenFlow).Value;
        }
    }
}
