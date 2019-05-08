using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq;
using System.Collections.Generic;
using System.Net.Http;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.HttpApiCommands;
using commercetools.Sdk.HttpApi.RequestBuilders;
using commercetools.Sdk.HttpApi.SearchParameters;
using commercetools.Sdk.Serialization;
using Xunit;
using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class RequestBuilderTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public RequestBuilderTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void CreateHttpApiCommand()
        {
            CreateCommand<Category> createCommand = new CreateCommand<Category>(new CategoryDraft());
            IHttpApiCommandFactory httpApiCommandFactory = this.clientFixture.GetService<IHttpApiCommandFactory>();
            IHttpApiCommand httpApiCommand = httpApiCommandFactory.Create(createCommand);
            Assert.Equal(typeof(CreateHttpApiCommand<Category>), httpApiCommand.GetType());
        }

        [Fact]
        public void SearchHttpApiCommand()
        {
            SearchProductProjectionsCommand searchCommand = new SearchProductProjectionsCommand(new ProductProjectionSearchParameters());
            IHttpApiCommandFactory httpApiCommandFactory = this.clientFixture.GetService<IHttpApiCommandFactory>();
            IHttpApiCommand httpApiCommand = httpApiCommandFactory.Create(searchCommand);
            Assert.Equal(typeof(SearchHttpApiCommand<ProductProjection>), httpApiCommand.GetType());
        }

        [Fact]
        public void GetQueryRequestMessageWithExpand()
        {
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> parentExpansion = new ReferenceExpansion<Category>(c => c.Parent);
            expansions.Add(parentExpansion);
            QueryCommand<Category> queryCommand = new QueryCommand<Category>();
            queryCommand.SetExpand(expansions);
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(
                this.clientFixture.GetService<IEndpointRetriever>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>());
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(queryCommand);
            Assert.Equal("categories?expand=parent", httpRequestMessage.RequestUri.ToString());
        }

        [Fact]
        public void GetQueryRequestMessageWithTwoExpands()
        {
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> parentExpansion = new ReferenceExpansion<Category>(c => c.Parent);
            ReferenceExpansion<Category> firstAncestorExpansion = new ReferenceExpansion<Category>(c => c.Ancestors[0]);
            expansions.Add(parentExpansion);
            expansions.Add(firstAncestorExpansion);
            QueryCommand<Category> queryCommand = new QueryCommand<Category>();
            queryCommand.SetExpand(expansions);
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(
                this.clientFixture.GetService<IEndpointRetriever>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>());
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(queryCommand);
            Assert.Equal( "categories?expand=parent&expand=ancestors%5B0%5D", httpRequestMessage.RequestUri.ToString());
        }

        [Fact]
        public void GetRequestMessageBuilderFromFactory()
        {
            IRequestMessageBuilderFactory requestMessageBuilderFactory = this.clientFixture.GetService<IRequestMessageBuilderFactory>();
            IRequestMessageBuilder requestMessageBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetRequestMessageBuilder>();
            Assert.Equal(typeof(GetRequestMessageBuilder), requestMessageBuilder.GetType());
        }

        [Fact]
        public void GetProductAdditionalParametersBuilderFromFactory()
        {
            ProductAdditionalParameters productAdditionalParameters = new ProductAdditionalParameters();
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory = this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>();
            IAdditionalParametersBuilder additionalParametersMessageBuilder = parametersBuilderFactory.GetParameterBuilder(productAdditionalParameters);
            Assert.Equal(typeof(ProductAdditionalParametersBuilder), additionalParametersMessageBuilder.GetType());
        }


        [Fact]
        public void GetShippingMethodsForCartAdditionalParametersBuilderFromFactory()
        {
            GetShippingMethodsForCartAdditionalParameters productAdditionalParameters = new GetShippingMethodsForCartAdditionalParameters();
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory = this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>();
            IAdditionalParametersBuilder additionalParametersMessageBuilder = parametersBuilderFactory.GetParameterBuilder(productAdditionalParameters);
            Assert.Equal(typeof(GetShippingMethodsForCartAdditionalParametersBuilder), additionalParametersMessageBuilder.GetType());
        }

        [Fact]
        public void GetProductProjectionSearchParametersBuilderFromFactory()
        {
            ProductProjectionSearchParameters productAdditionalParameters = new ProductProjectionSearchParameters();
            IParametersBuilderFactory<ISearchParametersBuilder> parametersBuilderFactory = this.clientFixture.GetService<IParametersBuilderFactory<ISearchParametersBuilder>>();
            ISearchParametersBuilder parametersBuilder = parametersBuilderFactory.GetParameterBuilder(productAdditionalParameters);
            Assert.Equal(typeof(ProductProjectionSearchParametersBuilder), parametersBuilder.GetType());
        }

        [Fact]
        public void GetMatchingProductDiscountRequestMessage()
        {
            GetMatchingProductDiscountParameters parameters = new GetMatchingProductDiscountParameters();
            parameters.Price = new Price();
            GetMatchingProductDiscountCommand matchingCommand = new GetMatchingProductDiscountCommand(parameters);
            GetMatchingRequestMessageBuilder requestMessageBuilder = new GetMatchingRequestMessageBuilder(
                this.clientFixture.GetService<ISerializerService>(),
                this.clientFixture.GetService<IEndpointRetriever>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>());
            HttpRequestMessage httpRequestMessage = requestMessageBuilder.GetRequestMessage(matchingCommand);
            Assert.Equal("product-discounts/matching", httpRequestMessage.RequestUri.ToString());
        }

        [Fact]
        public void MultipleWhereParameters()
        {
            var c = new QueryCommand<Category>();
            c.Where(category => category.Id == "abc").Where(category => category.Key == "def");
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(
                this.clientFixture.GetService<IEndpointRetriever>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>());
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(c);
            Assert.Equal("categories?where=id%20%3D%20%22abc%22&where=key%20%3D%20%22def%22", httpRequestMessage.RequestUri.ToString());
        }
    }
}
