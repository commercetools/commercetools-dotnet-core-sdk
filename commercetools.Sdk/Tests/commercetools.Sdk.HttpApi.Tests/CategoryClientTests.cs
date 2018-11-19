using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Serialization;
using Moq;
using Moq.Protected;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryClientTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public CategoryClientTests(ClientFixture clientFixture)
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
                Content = new StringContent(serialized),
            })
            .Verifiable();
            IClient commerceToolsClient = new Client(mockHttpClientFactory.Object, this.clientFixture.GetService<IHttpApiCommandFactory>(), this.clientFixture.GetService<ISerializerService>());
            string categoryId = "2bafc816-4223-4ff0-ac8a-0f08a8f29fd6";
            Category category = commerceToolsClient.ExecuteAsync(new GetByIdCommand<Category>(new Guid(categoryId))).Result;
            Assert.Equal(categoryId, category.Id.ToString());
        }
    }
}