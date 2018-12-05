using System.Linq;

namespace commercetools.Sdk.HttpApi
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoggerHandler : DelegatingHandler
    {
        private readonly ILoggerFactory loggerFactory;

        public LoggerHandler(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // TODO Which name should be set here?
            var logger = this.loggerFactory.CreateLogger("LoggerHandler");
            logger.LogInformation(request.RequestUri.ToString());
            logger.LogInformation(request.Method.ToString());
            logger.LogInformation(request.Headers.GetValues("X-Correlation-ID").FirstOrDefault());
            var response = await base.SendAsync(request, cancellationToken);
            if (response != null)
            {
                logger.LogInformation(response.StatusCode.ToString());
                logger.LogInformation(response.Headers.GetValues("X-Correlation-ID").FirstOrDefault());
            }
            return response;
        }
    }
}