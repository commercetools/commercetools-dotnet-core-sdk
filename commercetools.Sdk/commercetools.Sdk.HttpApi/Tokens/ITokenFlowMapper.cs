using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.Tokens
{
    public interface ITokenFlowMapper
    {
        ITokenFlowRegister TokenFlowRegister { get; }

        ITokenFlowRegister GetTokenFlowRegisterForClient(string name);
    }
}