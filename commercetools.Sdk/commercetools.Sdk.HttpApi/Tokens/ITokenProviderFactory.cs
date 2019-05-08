using System;

namespace commercetools.Sdk.HttpApi.Tokens
{
    public interface ITokenProviderFactory
    {
        Type GetTokenProviderByFlow(TokenFlow tokenFlow);
    }
}
