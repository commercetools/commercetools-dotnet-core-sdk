using commercetools.Sdk.HttpApi.Tokens;
using System;

namespace commercetools.Sdk.MultipleClients
{
    public class AnonymousCredentialsStoreManager : InMemoryTokenStoreManager, IAnonymousCredentialsStoreManager
    {
        public string AnonymousId => "multiple-clients-example" + DateTime.Now.ToString("yyyyMMddhhmmss");
    }
}
