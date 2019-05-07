using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq;
using System;
using System.Net.Http;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.RequestBuilders;
using Xunit;
using commercetools.Sdk.Domain.Categories;

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
            string apiBaseAddressWithProjectKey = this.clientFixture.GetAPIBaseAddressWithProjectKey("Client");
            GetByIdCommand<Category> command = new GetByIdCommand<Category>(new Guid("2bafc816-4223-4ff0-ac8a-0f08a8f29fd6"));
            GetRequestMessageBuilder requestMessageBuilder = new GetRequestMessageBuilder(
                this.clientFixture.GetService<IEndpointRetriever>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>());
            HttpRequestMessage httpRequestMessage = requestMessageBuilder.GetRequestMessage(command);
            Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
            Assert.Equal("categories/2bafc816-4223-4ff0-ac8a-0f08a8f29fd6", httpRequestMessage.RequestUri.ToString());
        }
    }
}
