using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Categories.UpdateActions;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Serialization;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ClientTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public ClientTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void GetCategoryById()
        {
            string serialized = File.ReadAllText("Resources/Responses/GetCategoryById.json");
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpClientFactory.Setup(x => x.CreateClient("api")).Returns(new HttpClient(mockHandler.Object));
            mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(serialized)
            })
            .Verifiable();
            IClient commerceToolsClient = new Client(mockHttpClientFactory.Object, this.clientFixture.GetService<IHttpApiCommandFactory>(), this.clientFixture.GetService<ISerializerService>());
            string categoryId = "2bafc816-4223-4ff0-ac8a-0f08a8f29fd6";
            Category category = commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(new Guid(categoryId))).Result;
            Assert.Equal(categoryId, category.Id.ToString());
        }

        [Fact]
        public void GetCategoryByIdThrowsNotFoundExceptionEmptyBody()
        { 
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpClientFactory.Setup(x => x.CreateClient("api")).Returns(new HttpClient(mockHandler.Object));
            mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(string.Empty)
            })
            .Verifiable();
            IClient commerceToolsClient = new Client(mockHttpClientFactory.Object, this.clientFixture.GetService<IHttpApiCommandFactory>(), this.clientFixture.GetService<ISerializerService>());
            // Empty response body with 404 happens in case of an invalid guid.
            // However, since we can't pass an invalid guid here, the response body is mocked instead.
            string categoryId = "2b327437-702e-4ab2-96fc-a98afa860b36";
            HttpApiClientException exception = Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(new Guid(categoryId)))).Result;
            Assert.Equal(404, exception.StatusCode);
            Assert.Null(exception.Errors);
        }

        [Fact]
        public void GetCategoryByIdThrowsNotFoundException()
        {
            string serialized = File.ReadAllText("Resources/Responses/ResourceNotFound.json");
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpClientFactory.Setup(x => x.CreateClient("api")).Returns(new HttpClient(mockHandler.Object));
            mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(serialized)
            })
            .Verifiable();
            IClient commerceToolsClient = new Client(mockHttpClientFactory.Object, this.clientFixture.GetService<IHttpApiCommandFactory>(), this.clientFixture.GetService<ISerializerService>());
            string categoryId = "2b327437-702e-4ab2-96fc-a98afa860b36";
            HttpApiClientException exception = Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(new Guid(categoryId)))).Result;
            Assert.Equal(404, exception.StatusCode);
            Assert.Single(exception.Errors);
        }

        [Fact]
        public void GetCategoryByIdThrowsConcurrentModificationException()
        {
            string serialized = File.ReadAllText("Resources/Responses/ConcurrentModification.json");
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpClientFactory.Setup(x => x.CreateClient("api")).Returns(new HttpClient(mockHandler.Object));
            mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Conflict,
                Content = new StringContent(serialized)
            })
            .Verifiable();
            IClient commerceToolsClient = new Client(mockHttpClientFactory.Object, this.clientFixture.GetService<IHttpApiCommandFactory>(), this.clientFixture.GetService<ISerializerService>());
            SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction();
            setKeyAction.Key = "newKey" + this.clientFixture.RandomString(3);
            string categoryId = "8994e5d7-d81f-4480-af60-286dc96c1fe8";
            HttpApiClientException exception = Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<Category>(new Guid(categoryId), 249, new List<UpdateAction<Category>>() { setKeyAction }))).Result;
            Assert.Equal(409, exception.StatusCode);
            Assert.Single(exception.Errors);
            Assert.IsType<ConcurrentModificationError>(exception.Errors[0]);
        }
    }
}
