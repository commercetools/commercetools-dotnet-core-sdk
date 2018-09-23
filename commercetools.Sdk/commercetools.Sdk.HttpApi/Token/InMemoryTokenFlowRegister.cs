namespace commercetools.Sdk.HttpApi
{
    using System.Collections.Generic;

    public class InMemoryTokenFlowRegister : ITokenFlowRegister
    {
        public TokenFlow TokenFlow { get; set; }
    }
}