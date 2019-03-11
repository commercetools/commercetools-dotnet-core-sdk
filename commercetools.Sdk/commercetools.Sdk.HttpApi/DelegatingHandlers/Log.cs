using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace commercetools.Sdk.HttpApi.DelegatingHandlers
{
    internal static class Log
    {
        private static class EventIds
        {
            public static readonly EventId PipelineStart = new EventId(100, "RequestPipelineStart");
            public static readonly EventId PipelineEnd = new EventId(101, "RequestPipelineEnd");
        }

        private static readonly Func<ILogger, HttpMethod, Uri, string, IDisposable> _beginRequestPipelineScope =
            LoggerMessage.DefineScope<HttpMethod, Uri, string>(
                "HTTP {HttpMethod} {Uri} {CorrelationId}");

        private static readonly Action<ILogger, HttpMethod, Uri, string, Exception> _requestPipelineStart =
            LoggerMessage.Define<HttpMethod, Uri, string>(
                LogLevel.Information,
                EventIds.PipelineStart,
                "Start processing HTTP request {HttpMethod} {Uri} [Correlation: {CorrelationId}]");

        private static readonly Action<ILogger, HttpStatusCode, Exception> _requestPipelineEnd =
            LoggerMessage.Define<HttpStatusCode>(
                LogLevel.Information,
                EventIds.PipelineEnd,
                "End processing HTTP request - {StatusCode}");

        public static IDisposable BeginRequestPipelineScope(ILogger logger, HttpRequestMessage request)
        {
            var correlationId = GetCorrelationIdFromRequest(request);
            return _beginRequestPipelineScope(logger, request.Method, request.RequestUri, correlationId);
        }

        public static void RequestPipelineStart(ILogger logger, HttpRequestMessage request)
        {
            var correlationId = GetCorrelationIdFromRequest(request);
            _requestPipelineStart(logger, request.Method, request.RequestUri, correlationId, null);
        }

        public static void RequestPipelineEnd(ILogger logger, HttpResponseMessage response)
        {
            _requestPipelineEnd(logger, response.StatusCode, null);
        }

        private static string GetCorrelationIdFromRequest(HttpRequestMessage request)
        {
            var correlationId = "Not set";

            if (request.Headers.TryGetValues("X-Correlation-ID", out var values))
            {
                correlationId = values.First();
            }

            return correlationId;
        }
    }
}