using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    /// <summary>
    /// Test Api Exception Factory if it's create the correct exception type
    /// </summary>
    public class ApiExceptionFactoryTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public ApiExceptionFactoryTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }


        [Theory]
        [MemberData(nameof(GetExceptionsTestData))]
        public void TestCreateApiExceptionTheory(int statusCode, string responseContent, bool isJsonResponse, Type expectedExceptionType)
        {
            //Arrange
            var clientConfiguration = this.clientFixture.GetService<IClientConfiguration>();
            var serializerService = this.clientFixture.GetService<ISerializerService>();
            var mockHttpRequestMessage = GetHttpRequestMessageMock(); //same request for all
            var exceptionFactory = new ApiExceptionFactory(clientConfiguration,serializerService);


            //Act
            var mockHttpResponseMessage = GetHttpResponseMessageMock(statusCode, responseContent, isJsonResponse);
            var exception =
                exceptionFactory.CreateApiException(mockHttpRequestMessage.Object, mockHttpResponseMessage.Object);

            //Assert Request
            Assert.NotNull(exception);
            Assert.NotNull(exception.Request);
            Assert.True(!string.IsNullOrEmpty(exception.CorrelationId));

            //Assert Response
            Assert.True(!string.IsNullOrEmpty(exception.HttpSummary));

            //Assert Exception
            Assert.IsType(expectedExceptionType, exception);

            //Conditional Asserts Based on Exception Types
            if (exception is ConcurrentModificationException)
            {
                var conCurrentException = (exception as ConcurrentModificationException);
                Assert.NotNull(conCurrentException.GetCurrentVersion());
                Assert.NotNull(conCurrentException.ErrorResponse);
                Assert.Single(conCurrentException.ErrorResponse.Errors);
                Assert.IsType<ConcurrentModificationError>(conCurrentException.ErrorResponse.Errors[0]);
            }

            if (exception is ApiServiceException)
            {
                var apiServiceException = (exception as ApiServiceException);
                //Assert.Equal(statusCode, apiServiceException.StatusCode);
            }

        }
        /// <summary>
        /// Get list of test data, each object contains statusCode, responseContent, isJsonResponse ,expected Exception Type
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetExceptionsTestData()
        {
            string serializedNotFound = File.ReadAllText("Resources/Responses/ResourceNotFound.json");
            string serializedConcurrentModification = File.ReadAllText("Resources/Responses/ConcurrentModification.json");

            var allData = new List<object[]>
            {
                new object[] { 403, "", false, typeof(ForbiddenException) },
                new object[] { 500, "", false, typeof(InternalServerErrorException) },
                new object[] { 502, "", false, typeof(BadGatewayException) },
                new object[] { 503, "", false, typeof(ServiceUnavailableException) },
                new object[] { 504, "", false, typeof(GatewayTimeoutException) },
                new object[] { 413, "", false, typeof(RequestEntityTooLargeException) },
                new object[] { 404, serializedNotFound, true, typeof(NotFoundException) },
                new object[] { 404, "", true, typeof(NotFoundException) },//with empty Body
                new object[] { 409, serializedConcurrentModification, true, typeof(ConcurrentModificationException) },
                new object[] { 401, "invalid_token", false, typeof(InvalidTokenException) },
                new object[] { 401, "", false, typeof(UnauthorizedException) },
                new object[] { 400, "invalid_scope", false, typeof(InvalidTokenException) },
                new object[] { 400, "", false, typeof(ErrorResponseException) },
                new object[] { 505, "<h2>Service Unavailable</h2>", false, typeof(ServiceUnavailableException) }, //hack to check if content contains service unavailable with different status code (505)
                new object[] { 501, "", false, typeof(ApiException) },//General Exception if status not handled
            };

            return allData;
        }

        #region HelperFunctions

        /// <summary>
        /// Create Mock for HttpRequestMessage
        /// </summary>
        /// <returns></returns>
        private Mock<HttpRequestMessage> GetHttpRequestMessageMock()
        {
            var requestMock = new Mock<HttpRequestMessage>();
            var correlationId = "correlationId";
            requestMock.Object.Method = HttpMethod.Get;
            requestMock.Object.Headers.Add("X-Correlation-ID", correlationId);
            return requestMock;
        }
        /// <summary>
        /// Create Mock for the HttpResponseMessage
        /// </summary>
        /// <param name="statusCode">status Code of the response</param>
        /// <param name="content">content of the response</param>
        /// <param name="isJson">is the content type is Json</param>
        /// <returns></returns>
        private Mock<HttpResponseMessage> GetHttpResponseMessageMock(int statusCode, string content, bool isJson = false)
        {
            var responseMock = new Mock<HttpResponseMessage>();
            var mediaType = isJson ? "application/json" : "text/html";
            responseMock.Object.StatusCode = (HttpStatusCode) statusCode;
            responseMock.Object.Content = new StringContent(content, Encoding.UTF8,mediaType);
            return responseMock;
        }

        #endregion

    }
}
