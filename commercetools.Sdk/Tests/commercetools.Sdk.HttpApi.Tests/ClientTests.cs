using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Serialization;
using Moq;
using Moq.Protected;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using commercetools.Sdk.HttpApi.DelegatingHandlers;
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
            mockHttpClientFactory.Setup(x => x.CreateClient(DefaultClientNames.Api)).Returns(new HttpClient(mockHandler.Object){ BaseAddress = new Uri("https://api.sphere.io/test-project/") });
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
            IClient commerceToolsClient = new CtpClient(
                mockHttpClientFactory.Object,
                this.clientFixture.GetService<IHttpApiCommandFactory>(),
                this.clientFixture.GetService<ISerializerService>(),
                this.clientFixture.GetService<IUserAgentProvider>()
                );
            string categoryId = "2bafc816-4223-4ff0-ac8a-0f08a8f29fd6";
            Category category = commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(new Guid(categoryId))).Result;
            Assert.Equal(categoryId, category.Id.ToString());
        }

        [Fact]
        public void TestUserAgent()
        {
            var userAgent = this.clientFixture.GetService<IUserAgentProvider>().UserAgent;
            Assert.Matches(@"commercetools-dotnet-core-sdk/[1-9]{1,4}(\.[0-9]{1,6}){3} dotnetCore/[1-9]{1,3}(\.[0-9]{1,6}){3}( \((WINDOWS|OSX|LINUX|FreeBSD)/[1-9]{1,3}(\.[0-9]{1,6}){3}\))?", userAgent);

            var c = new HttpRequestMessage(HttpMethod.Get, "/");
            c.Headers.UserAgent.ParseAdd(userAgent);
        }
    }
}
