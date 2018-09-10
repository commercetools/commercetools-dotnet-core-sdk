using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class MockHttpClientFactory : IHttpClientFactory
    {
        private AuthorizationHandler authorizationHandler;

        public MockHttpClientFactory(AuthorizationHandler authorizationHandler)
        {
            this.authorizationHandler = authorizationHandler;
        }

        public HttpClient CreateClient(string name)
        {
            if (name == "api")
            {
                this.authorizationHandler.InnerHandler = new HttpClientHandler();
                HttpClient client = new HttpClient(this.authorizationHandler);
                return client;
            }
            return new HttpClient();
        }
    }
}
