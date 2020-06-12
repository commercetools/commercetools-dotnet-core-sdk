using System;

namespace commercetools.Sdk.HttpApi.DelegatingHandlers
{
    public class DefaultCorrelationIdProvider : ICorrelationIdProvider
    {
        public IClientConfiguration ClientConfiguration { get; set; }

        public string CorrelationId => $"{this.ClientConfiguration.ProjectKey}/{Guid.NewGuid()}";
    }
}
