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
using System.Linq;
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
            mockHttpClientFactory.Setup(x => x.CreateClient(DefaultClientNames.Api)).Returns(new HttpClient(mockHandler.Object));
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

        [Fact(Skip = "Need to be refactored and written in better way")]
        public void TestUserAgent()
        {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpClientFactory.Setup(x => x.CreateClient(DefaultClientNames.Api)).Returns(new HttpClient(mockHandler.Object));
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();
            Client commerceToolsClient = new Client(mockHttpClientFactory.Object, this.clientFixture.GetService<IHttpApiCommandFactory>(), this.clientFixture.GetService<ISerializerService>());
            //var userAgent = commerceToolsClient.GetClientUserAgent();
            var userAgent = "";
            //TODO: Check UserAgent with Regular Expression
            //Assert.Equal("commercetools-dotnet-core-sdk/1.0.0.0 .NET-core/4.6.45454", userAgent.DefaultRequestHeaders.UserAgent.First().ToString());
            Assert.Equal("commercetools-dotnet-core-sdk/1.0.0.0 .NET-core/4.6.45454", userAgent);
        }
    }
}
