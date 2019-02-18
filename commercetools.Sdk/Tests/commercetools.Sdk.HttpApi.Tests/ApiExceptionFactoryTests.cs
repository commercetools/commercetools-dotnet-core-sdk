using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ApiExceptionFactoryTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public ApiExceptionFactoryTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void TestCreateApiException()
        {
            //Arrange
            var clientConfiguration = this.clientFixture.GetService<IClientConfiguration>();
            var serializerService = this.clientFixture.GetService<ISerializerService>();
            var mockHttpRequestMessage = GetHttpRequestMessageMock();
            var exceptionFactory = new ApiExceptionFactory(clientConfiguration,serializerService);
            
            
            //Act
            var mockHttpResponseMessage = GetHttpResponseMessageMock(403, "");//ForbiddenException
            var exception =
                exceptionFactory.CreateApiException(mockHttpRequestMessage.Object, mockHttpResponseMessage.Object);
            
            //Assert Request
            Assert.NotNull(exception);
            Assert.NotNull(exception.Request);
            Assert.True(!string.IsNullOrEmpty(exception.CorrelationId));
            
            //Assert Response
            Assert.True(!string.IsNullOrEmpty(exception.HttpSummary));
            
            //Assert Exception
            Assert.IsType<ForbiddenException>(exception);
            
        }
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
    }
}