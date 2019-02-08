using System.Net.Http;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace commercetools.Sdk.HttpApi
{
    /// <summary>
    /// Responsible for Creating HTTP Exceptions based on the status code return from unsuccessful requests
    /// </summary>
    public class ApiExceptionFactory : IApiExceptionFactory
    {
        private readonly IClientConfiguration clientConfiguration;

        public ApiExceptionFactory(IClientConfiguration clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
        }

        /// <summary>
        /// Create API Exception based on status code
        /// </summary>
        /// <param name="request">The http Request</param>
        /// <param name="response">The http Response</param>
        /// <param name="projectKey">client project key</param>
        /// <returns>Api Exception or any exception inherit from it</returns>
        public ApiException CreateApiException(HttpRequestMessage request, HttpResponseMessage response)
        {
            var apiException = new ApiException("Inner exception")
            {
                Request = request,
                Response = response,
                ProjectKey = this.clientConfiguration.ProjectKey
            };
            return apiException;
        }
    }
}