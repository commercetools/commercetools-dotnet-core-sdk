using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace commercetools.Sdk.HttpApi
{
    public class InMemoryAnonymousCredentialsStoreManager : InMemoryTokenStoreManager, IAnonymousCredentialsStoreManager
    {
        public string AnonymousId { get; set; }
    }
}