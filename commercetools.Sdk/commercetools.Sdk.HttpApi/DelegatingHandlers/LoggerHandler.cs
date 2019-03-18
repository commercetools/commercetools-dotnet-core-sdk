using System;
using System.Collections.Generic;
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

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // TODO Which name should be set here?
            var logger = this.loggerFactory.CreateLogger("commercetoolsLoggerHandler");

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (Log.BeginRequestPipelineScope(logger, request))
            {
                Log.RequestPipelineStart(logger, request);
                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                Log.RequestPipelineEnd(logger, response);

                return response;
            }
        }
    }
}
