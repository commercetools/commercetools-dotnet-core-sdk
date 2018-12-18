using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Net.Http;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.RequestBuilders;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ProductRequestMessageTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public ProductRequestMessageTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void ProductGetByIdRequestMessageWithPriceCurrency()
        {
            ProductAdditionalParameters productAdditionalParameters = new ProductAdditionalParameters();
            productAdditionalParameters.PriceCurrency = "EUR";
            GetByIdCommand<Product> command = new GetByIdCommand<Product>(new Guid("2bafc816-4223-4ff0-ac8a-0f08a8f29fd6"), productAdditionalParameters);
            GetRequestMessageBuilder requestMessageBuilder = new GetRequestMessageBuilder(
                this.clientFixture.GetService<IClientConfiguration>(),
                this.clientFixture.GetService<IEndpointRetriever>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>());
            HttpRequestMessage httpRequestMessage = requestMessageBuilder.GetRequestMessage(command);
            Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
            Assert.Equal("https://api.sphere.io/portablevendor/products/2bafc816-4223-4ff0-ac8a-0f08a8f29fd6?priceCurrency=EUR", httpRequestMessage.RequestUri.ToString());
        }
    }
}