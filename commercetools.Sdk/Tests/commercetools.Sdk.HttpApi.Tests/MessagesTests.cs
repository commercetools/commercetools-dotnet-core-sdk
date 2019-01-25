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
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.Messages.Categories;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class MessagesTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public MessagesTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void GetMessageByIdForCategoryCreatedAction()
        {
            string serialized = File.ReadAllText("Resources/Responses/CategoryCreatedMessage.json");
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
            IClient commerceToolsClient = new Client(mockHttpClientFactory.Object, this.clientFixture.GetService<IHttpApiCommandFactory>(), this.clientFixture.GetService<ISerializerService>()) { Name = "api" };
            string messageId = "174adf2f-783f-4ce5-a2d5-ee7d3ee7caf4";
            CategoryCreatedMessage categoryCreatedMessage = commerceToolsClient.ExecuteAsync(new GetByIdCommand<Message>(new Guid(messageId))).Result as CategoryCreatedMessage;
            Assert.Equal(messageId, categoryCreatedMessage.Id);
        }
    }
}
