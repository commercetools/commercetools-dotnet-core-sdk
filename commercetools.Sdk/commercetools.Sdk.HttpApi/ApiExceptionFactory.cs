using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using commercetools.Sdk.Domain.Errors;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using commercetools.Sdk.HttpApi.Extensions;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.Http;

namespace commercetools.Sdk.HttpApi
{
    /// <summary>
    /// Responsible for Creating HTTP Exceptions based on the status code return from unsuccessful requests
    /// </summary>
    public class ApiExceptionFactory : IApiExceptionFactory
    {
        private readonly IClientConfiguration clientConfiguration;
        private readonly ISerializerService serializerService;
        private List<HttpResponseExceptionResponsibility> responsibilities;

        public ApiExceptionFactory(IClientConfiguration clientConfiguration, ISerializerService serializerService)
        {
            this.clientConfiguration = clientConfiguration;
            this.serializerService = serializerService;
            this.FillResponsibilities();
        }

        /// <summary>
        /// Create API Exception based on status code
        /// </summary>
        /// <param name="request">The http Request</param>
        /// <param name="response">The http Response</param>
        /// <returns>General Api Exception or any exception inherit from it based on the status code</returns>
        public ApiException CreateApiException(HttpRequestMessage request, HttpResponseMessage response)
        {
            ApiException apiException = null;
            string content = response.ExtractResponseBody();
            var responsibility = this.responsibilities.FirstOrDefault(r => r.Predicate(response));
            if (responsibility?.ExceptionCreator != null)
            {
                apiException = responsibility.ExceptionCreator(response);
            }

            // set the common info for all exception types
            if (apiException != null)
            {
                apiException.Request = request;
                apiException.Response = response;
                apiException.ProjectKey = this.clientConfiguration.ProjectKey;
                if (!string.IsNullOrEmpty(content) && response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    var errorResponse = this.serializerService.Deserialize<ErrorResponse>(content);
                    if (errorResponse.Errors != null)
                    {
                        apiException.ErrorResponse = errorResponse;
                    }
                }
            }

            return apiException;
        }

        /// <summary>
        /// Fill responsibilities which factory will use to create the exception
        /// each responsibility contains predicate and func, so when the http response meet the criteria of the predicate, then we use delegate Func to create the exception
        /// </summary>
        private void FillResponsibilities()
        {
            this.responsibilities = new List<HttpResponseExceptionResponsibility>();

            // Add responsibilities based on status code
            WhenStatus(403, response => new ForbiddenException());
            WhenStatus(500, response => new InternalServerErrorException());
            WhenStatus(502, response => new BadGatewayException());
            WhenStatus(503, response => new ServiceUnavailableException());
            WhenStatus(504, response => new GatewayTimeoutException());
            WhenStatus(413, response => new RequestEntityTooLargeException());
            WhenStatus(404, response => new NotFoundException());
            WhenStatus(409, response => new ConcurrentModificationException());
            WhenStatus(401, response =>
            {
                var exception = response.ExtractResponseBody().Contains("invalid_token")
                    ? new InvalidTokenException()
                    : new UnauthorizedException();
                return exception;
            });
            WhenStatus(400, response =>
            {
                if (response.ExtractResponseBody().Contains("invalid_scope"))
                {
                    return new InvalidTokenException();
                }
                else
                {
                    return new ErrorResponseException();
                }
            });

            // Add the other responsibilities
            When(response => response.IsServiceNotAvailable(), response => new ServiceUnavailableException());
            When(response => true, response => new ApiException());
        }

        /// <summary>
        /// Add Responsibility when the Response meet the criteria of the predicate
        /// </summary>
        /// <param name="predicate">predicate which we check if the response message meet it's criteria</param>
        /// <param name="exceptionCreator">delegate function we use to create the right exception based on the status</param>
        private void When(Predicate<HttpResponseMessage> predicate, Func<HttpResponseMessage, ApiException> exceptionCreator)
        {
            this.AddResponsibility(predicate, exceptionCreator);
        }

        /// <summary>
        /// Add Responsibility when the Response status equal to the passed status
        /// </summary>
        /// <param name="status">response status code</param>
        /// <param name="exceptionCreator">delegate func to create the right exception</param>
        private void WhenStatus(int status, Func<HttpResponseMessage, ApiException> exceptionCreator)
        {
            this.AddResponsibility(response => (int)response.StatusCode == status, exceptionCreator);
        }

        /// <summary>
        /// Add Responsibility to responsibilities list
        /// </summary>
        /// <param name="predicate">predicate which we check if the response message meet it's criteria</param>
        /// <param name="exceptionCreator">delegate function we use to create the right exception based on the status</param>
        private void AddResponsibility(Predicate<HttpResponseMessage> predicate, Func<HttpResponseMessage, ApiException> exceptionCreator)
        {
            if (this.responsibilities == null)
            {
                return;
            }

            var responsibility = new HttpResponseExceptionResponsibility(predicate, exceptionCreator);
            this.responsibilities.Add(responsibility);
        }
    }
}
