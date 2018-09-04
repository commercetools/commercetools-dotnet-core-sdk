using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace commercetools.Sdk.HttpApi
{
    public class InMemoryTokenStoreManager : ITokenStoreManager
    {
        public Token Token { get; set; }
    }
}