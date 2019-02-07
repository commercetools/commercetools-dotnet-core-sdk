using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace commercetools.Sdk.HttpApi.DelegatingHandlers
{
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

            // The logging of the Content is not easy so that it is not done here on purpose.
            // It can be done, however.
            // https://gunnarpeipman.com/aspnet/aspnet-core-request-body/
            logger.LogInformation(request.RequestUri.ToString());
            logger.LogInformation(request.Method.ToString());
            logger.LogInformation(request.Headers.GetValues("X-Correlation-ID").FirstOrDefault());
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response != null)
            {
                logger.LogInformation(response.StatusCode.ToString());
                logger.LogInformation(response.Headers.GetValues("X-Correlation-ID").FirstOrDefault());
            }

            return response;
        }
    }
}