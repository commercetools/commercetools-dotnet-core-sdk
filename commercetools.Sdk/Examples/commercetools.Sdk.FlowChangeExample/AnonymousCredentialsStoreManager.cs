using commercetools.Sdk.HttpApi.Tokens;
using System;

namespace commercetools.Sdk.FlowChangeExample
{
    public class AnonymousCredentialsStoreManager : InMemoryTokenStoreManager, IAnonymousCredentialsStoreManager
    {
        public string AnonymousId => "flow-change-example" + DateTime.Now.ToString("yyyyMMddhhmmss");
    }
}
