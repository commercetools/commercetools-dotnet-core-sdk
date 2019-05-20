using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.HttpApi.HttpApiCommands;
using commercetools.Sdk.HttpApi.IntegrationTests.Products;
using commercetools.Sdk.Registration;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ProductProjectionSearchIntegrationTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly ProductFixture productFixture;

        public ProductProjectionSearchIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.productFixture = new ProductFixture(serviceProviderFixture);
        }

        [Fact(Skip = "Depends on the indexed products which might not be there.")]
        public void GetProductProjectionsFilterByCentAmountRange()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand();
            searchProductProjectionsCommand.Filter(p =>
                p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            Assert.Equal(19, results.Count);
        }

        [Fact(Skip = "Depends on the indexed products which might not be there.")]
        public void GetProductProjectionsFilterQueryByCentAmountRange()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand();
            searchProductProjectionsCommand.FilterQuery(p =>
                p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            Assert.Equal(19, results.Count);
        }

        [Fact(Skip = "Depends on the indexed products which might not be there.")]
        public void GetProductProjectionsFilterFacetByCentAmountRange()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand();
            searchProductProjectionsCommand.FilterFacets(p =>
                p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            Assert.Equal(20, results.Count);
        }

        [Fact(Skip = "Depends on the indexed products which might not be there.")]
        public void GetProductProjectionsTermFacet()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Facet<ProductProjection> colorFacet = new TermFacet<ProductProjection>(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault());

            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFacets(new List<Facet<ProductProjection>>() { colorFacet });
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand(searchParameters);
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            int facetCount = ((TermFacetResult)results.Facets["variants.attributes.color.key"]).Terms.Count();
            Assert.Equal(18, facetCount);
        }

        [Fact(Skip = "Depends on the indexed products which might not be there.")]
        public void GetProductProjectionsTermFacetAlias()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Facet<ProductProjection> colorFacet = new TermFacet<ProductProjection>(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault());
            colorFacet.Alias = "color";
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFacets(new List<Facet<ProductProjection>>() { colorFacet });
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand(searchParameters);
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            int facetCount = ((TermFacetResult)results.Facets["color"]).Terms.Count();
            Assert.Equal(18, facetCount);
        }

        [Fact(Skip = "Depends on the indexed products which might not be there.")]
        public void GetProductProjectionsTermFacetCountingProducts()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Facet<ProductProjection> colorFacet = new TermFacet<ProductProjection>(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault());
            colorFacet.IsCountingProducts = true;

            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFacets(new List<Facet<ProductProjection>>() { colorFacet });
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand(searchParameters);
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            var facetCount = ((TermFacetResult)results.Facets["variants.attributes.color.key"]).Terms[0].ProductCount;
            Assert.Equal(572, facetCount);
        }

        [Fact]
        public void UseLinqProvider()
        {
            var commerceToolsClient = this.productFixture.GetService<IClient>();

            var search = from p in commerceToolsClient.SearchProducts()
                where p.Categories.Any(reference => reference.Id == "abc")
                select p;

            search.Expand(p => p.ProductType)
                .Expand(p => p.TaxCategory)
                .Filter(p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "color" && ((TextAttribute)a).Value == "red")))
                .FilterQuery(p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "size" && ((TextAttribute)a).Value == "48")))
                .TermFacet(projection => projection.Key);

            var command = ((ClientProductProjectionSearchProvider) search.Provider).Command;
            var commandFactory = this.productFixture.GetService<IHttpApiCommandFactory>();;
            var httpApiCommand = commandFactory.Create(command);

            var request = httpApiCommand.HttpRequestMessage;
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("filter=variants.attributes.color%3A%22red%22&filter.query=categories.id%3A%22abc%22&filter.query=variants.attributes.size%3A%2248%22&facet=key&expand=productType&expand=taxCategory", request.Content.ReadAsStringAsync().Result);
        }
    }
}
