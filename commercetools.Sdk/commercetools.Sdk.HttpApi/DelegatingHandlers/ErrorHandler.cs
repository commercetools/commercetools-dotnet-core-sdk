using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using commercetools.Sdk.HttpApi.Domain.Exceptions;

namespace commercetools.Sdk.HttpApi.DelegatingHandlers
{
    /// <summary>
    /// Responsible for Catching API Exceptions from client and Throw them in the right exception type for example ( NotFoundException, BadRequestException, ..etc)
    /// </summary>
    public class ErrorHandler : DelegatingHandler
    {
        private readonly IApiExceptionFactory apiExceptionFactory;

        public ErrorHandler(IApiExceptionFactory apiExceptionFactory)
        {
            this.apiExceptionFactory = apiExceptionFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            // if there is failed response then build the exception
            if (response != null && !response.IsSuccessStatusCode)
            {
                ApiException exception = this.apiExceptionFactory.CreateApiException(request, response);
                throw exception;
            }

            return response;
        }
    }
}