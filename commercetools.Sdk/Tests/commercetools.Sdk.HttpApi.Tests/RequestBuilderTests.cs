using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq;
using System.Collections.Generic;
using System.Linq;
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
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Messages;

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
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IQueryParametersBuilder>>()
                );
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(queryCommand);
            Assert.Equal("categories?expand=parent&withTotal=false", httpRequestMessage.RequestUri.ToString());
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
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IQueryParametersBuilder>>());
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(queryCommand);
            Assert.Equal("categories?expand=parent&expand=ancestors%5B0%5D&withTotal=false", httpRequestMessage.RequestUri.ToString());
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
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IQueryParametersBuilder>>());
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(c);
            Assert.Equal("categories?where=id%20%3D%20%22abc%22&where=key%20%3D%20%22def%22&withTotal=false", httpRequestMessage.RequestUri.ToString());
        }

        [Fact]
        public void MultipleWhereParametersUsingSetWhere()
        {
            var c = new QueryCommand<Category>();
            c.SetWhere(new List<QueryPredicate<Category>>
            {
                new QueryPredicate<Category>(category => category.Id == "abc"),
                new QueryPredicate<Category>(category => category.Key == "def")
            });
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(
                this.clientFixture.GetService<IEndpointRetriever>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IQueryParametersBuilder>>());
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(c);
            Assert.Equal("categories?where=id%20%3D%20%22abc%22&where=key%20%3D%20%22def%22&withTotal=false", httpRequestMessage.RequestUri.ToString());
        }

        [Fact]
        public async void SearchProductProjectionsAndFilterByScopedPriceInChannel()
        {
            var channel = new Channel() { Id = "dbb5a6d0-2cbb-4855-bbe7-5cf58f434a82"};
            var searchRequest = new SearchProductProjectionsCommand();
            searchRequest.SetPriceCurrency("USD");
            searchRequest.SetPriceChannel(channel.Id);
            searchRequest.Filter(p => p.Variants.Any(v => v.ScopedPrice.Value.CentAmount.Exists()));

            var httpApiCommandFactory = this.clientFixture.GetService<IHttpApiCommandFactory>();
            var httpApiCommand = httpApiCommandFactory.Create(searchRequest);
            var content = await httpApiCommand.HttpRequestMessage.Content.ReadAsStringAsync();
            Assert.Equal("filter=variants.scopedPrice.value.centAmount%3Aexists&priceCurrency=USD&priceChannel=dbb5a6d0-2cbb-4855-bbe7-5cf58f434a82&withTotal=false", content);
        }

        [Fact]
        public void GetQueryRequestMessageWithSort()
        {
            QueryCommand<Message> queryCommand = new QueryCommand<Message>();
            queryCommand.Sort(message => message.Id).Sort(message => message.CreatedAt, SortDirection.Descending);
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(
                this.clientFixture.GetService<IEndpointRetriever>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IAdditionalParametersBuilder>>(),
                this.clientFixture.GetService<IParametersBuilderFactory<IQueryParametersBuilder>>()
            );
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(queryCommand);
            Assert.Equal("messages?sort=id%20asc&sort=createdAt%20desc&withTotal=false", httpRequestMessage.RequestUri.ToString());
        }
    }
}
