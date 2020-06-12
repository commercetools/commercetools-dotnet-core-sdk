using System;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.DelegatingHandlers;

namespace commercetools.Sdk.IntegrationTests
{
    public class CustomCorrelationIdProvider : ICorrelationIdProvider
    {
        public IClientConfiguration ClientConfiguration { get; set; }
        
        public string CorrelationId => "Custom_" + Guid.NewGuid();
    }
}