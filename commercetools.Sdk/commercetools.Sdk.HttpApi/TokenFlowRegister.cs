namespace commercetools.Sdk.HttpApi
{
    using System.Collections.Generic;

    public class TokenFlowRegister : ITokenFlowRegister
    {
        private IDictionary<string, TokenFlow> flowRegister = new Dictionary<string, TokenFlow>();

        public TokenFlow GetFlow(string clientName)
        {
            if (flowRegister.ContainsKey(clientName))
            {
                return flowRegister[clientName];
            }
            else
            {
                return TokenFlow.ClientCredentials;
            }
        }

        public void RegisterFlow(string clientName, TokenFlow tokenFlow)
        {
            if (!flowRegister.ContainsKey(clientName))
            {
                flowRegister.Add(clientName, tokenFlow);
            }
        }
    }
}