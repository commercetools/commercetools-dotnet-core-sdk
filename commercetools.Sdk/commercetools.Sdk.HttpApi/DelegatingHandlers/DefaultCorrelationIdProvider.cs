using System;

namespace commercetools.Sdk.HttpApi.DelegatingHandlers
{
    internal class DefaultCorrelationIdProvider : ICorrelationIdProvider
    {
        private readonly IClientConfiguration clientConfiguration;

        public DefaultCorrelationIdProvider(IClientConfiguration clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
        }

        public string CorrelationId => $"{this.clientConfiguration.ProjectKey}/{Guid.NewGuid()}";
    }
}