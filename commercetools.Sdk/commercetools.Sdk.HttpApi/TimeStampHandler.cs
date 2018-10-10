namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.HttpApi.Domain;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class TimestampHandler : DelegatingHandler
    {
        private readonly ILogger logger;

        public TimestampHandler(ILogger logger)
        {
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // TODO See in which format the date should be logged
            this.logger.LogInformation(DateTime.Now.ToString());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}