using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class MockHttpClientFactory : IHttpClientFactory
    {
        private AuthorizationHandler authorizationHandler;
        private TimestampHandler timestampHandler;
        private CorrelationIdHandler correlationIdHandler;

        public MockHttpClientFactory(AuthorizationHandler authorizationHandler, TimestampHandler timeStampHandler, CorrelationIdHandler correlationIdHandler)
        {
            this.authorizationHandler = authorizationHandler;
            this.correlationIdHandler = correlationIdHandler;
            this.timestampHandler = timeStampHandler;
        }

        public HttpClient CreateClient(string name)
        {
            if (name == "api")
            {
                this.correlationIdHandler.InnerHandler = new HttpClientHandler();
                this.authorizationHandler.InnerHandler = correlationIdHandler;
                this.timestampHandler.InnerHandler = this.authorizationHandler;
                HttpClient client = new HttpClient(timestampHandler);
                return client;
            }
            return new HttpClient();
        }
    }
}
