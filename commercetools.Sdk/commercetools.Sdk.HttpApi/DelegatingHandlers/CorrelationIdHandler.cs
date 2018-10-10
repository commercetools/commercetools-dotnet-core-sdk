namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.HttpApi.Domain;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class CorrelationIdHandler : DelegatingHandler
    {
        private readonly ILogger logger;

        public CorrelationIdHandler(ILogger logger)
        {
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response != null)
            {
                var correlationId = response.Headers.GetValues("X-Correlation-ID").FirstOrDefault(); 
                this.logger.LogInformation(correlationId);
            }
            return response;
        }
    }
}