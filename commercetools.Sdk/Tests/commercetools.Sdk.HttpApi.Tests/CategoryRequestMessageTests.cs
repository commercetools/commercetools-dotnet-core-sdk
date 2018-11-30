using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq;
using System;
using System.Net.Http;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class CategoryRequestMessageTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public CategoryRequestMessageTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void CategoryGetByIdRequestMessage()
        {
            GetByIdCommand<Category> command = new GetByIdCommand<Category>(new Guid("2bafc816-4223-4ff0-ac8a-0f08a8f29fd6"));
            GetRequestMessageBuilder requestMessageBuilder = new GetRequestMessageBuilder(clientFixture.GetService<IClientConfiguration>(), clientFixture.GetService<IExpansionExpressionVisitor>(), this.clientFixture.GetService<IEndpointRetriever>());
            HttpRequestMessage httpRequestMessage = requestMessageBuilder.GetRequestMessage(command);
            Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
            Assert.Equal("https://api.sphere.io/portablevendor/categories/2bafc816-4223-4ff0-ac8a-0f08a8f29fd6", httpRequestMessage.RequestUri.ToString());
        }
    }
}