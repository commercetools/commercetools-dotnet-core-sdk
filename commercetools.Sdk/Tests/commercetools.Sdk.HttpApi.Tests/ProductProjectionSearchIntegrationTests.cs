using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Products.Attributes;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ProductProjectionSearchIntegrationTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public ProductProjectionSearchIntegrationTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void GetProductProjectionsFilterByCentAmountRange()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Filter<ProductProjection> centAmountFilter = new Filter<ProductProjection>(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand();
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFilter(new List<Filter<ProductProjection>>() { centAmountFilter });
            searchProductProjectionsCommand.SearchParameters = searchParameters;
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            Assert.Equal(19, results.Count);
        }

        [Fact]
        public void GetProductProjectionsFilterQueryByCentAmountRange()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Filter<ProductProjection> centAmountFilter = new Filter<ProductProjection>(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand();
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFilterQuery(new List<Filter<ProductProjection>>() { centAmountFilter });
            searchProductProjectionsCommand.SearchParameters = searchParameters;
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            Assert.Equal(19, results.Count);
        }

        [Fact]
        public void GetProductProjectionsFilterFacetByCentAmountRange()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Filter<ProductProjection> centAmountFilter = new Filter<ProductProjection>(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand();
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFilterFacets(new List<Filter<ProductProjection>>() { centAmountFilter });
            searchProductProjectionsCommand.SearchParameters = searchParameters;
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            Assert.Equal(20, results.Count);
        }

        [Fact]
        public void GetProductProjectionsTermFacet()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Facet<ProductProjection> colorFacet = new TermFacet<ProductProjection>(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault());
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand();
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFacets(new List<Facet<ProductProjection>>() { colorFacet });
            searchProductProjectionsCommand.SearchParameters = searchParameters;
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            int facetCount = ((TermFacetResult)results.Facets["variants.attributes.color.key"]).Terms.Count();
            Assert.Equal(18, facetCount);
        }

        [Fact]
        public void GetProductProjectionsTermFacetAlias()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Facet<ProductProjection> colorFacet = new TermFacet<ProductProjection>(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault());
            colorFacet.Alias = "color";
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand();
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFacets(new List<Facet<ProductProjection>>() { colorFacet });
            searchProductProjectionsCommand.SearchParameters = searchParameters;
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            int facetCount = ((TermFacetResult)results.Facets["color"]).Terms.Count();
            Assert.Equal(18, facetCount);
        }

        [Fact]
        public void GetProductProjectionsTermFacetCountingProducts()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Facet<ProductProjection> colorFacet = new TermFacet<ProductProjection>(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault());
            colorFacet.IsCountingProducts = true;
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand();
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFacets(new List<Facet<ProductProjection>>() { colorFacet });
            searchProductProjectionsCommand.SearchParameters = searchParameters;
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            var facetCount = ((TermFacetResult)results.Facets["variants.attributes.color.key"]).Terms[0].ProductCount;
            Assert.Equal(572, facetCount);
        }
    }
}