using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System.Collections.Generic;
using System.Linq;
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
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(new SearchProductProjectionsCommand() { Filter = new List<Filter<ProductProjection>>() { centAmountFilter } }).Result;
            Assert.Equal(19, results.Count);
        }

        [Fact]
        public void GetProductProjectionsFilterQueryByCentAmountRange()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Filter<ProductProjection> centAmountFilter = new Filter<ProductProjection>(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(new SearchProductProjectionsCommand() { FilterQuery = new List<Filter<ProductProjection>>() { centAmountFilter } }).Result;
            Assert.Equal(19, results.Count);
        }

        [Fact]
        public void GetProductProjectionsFilterFacetByCentAmountRange()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Filter<ProductProjection> centAmountFilter = new Filter<ProductProjection>(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(new SearchProductProjectionsCommand() { FilterFacets = new List<Filter<ProductProjection>>() { centAmountFilter } }).Result;
            Assert.Equal(20, results.Count);
        }

        [Fact]
        public void GetProductProjectionsTermFacet()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Facet<ProductProjection> colorFacet = new TermFacet<ProductProjection>(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault());
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(new SearchProductProjectionsCommand() { Facets = new List<Facet<ProductProjection>>() { colorFacet } }).Result;
            int facetCount = ((TermFacetResult)results.Facets["variants.attributes.color.key"]).Terms.Count();
            Assert.Equal(18, facetCount);
        }

        [Fact]
        public void GetProductProjectionsTermFacetAlias()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Facet<ProductProjection> colorFacet = new TermFacet<ProductProjection>(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault());
            colorFacet.Alias = "color";
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(new SearchProductProjectionsCommand() { Facets = new List<Facet<ProductProjection>>() { colorFacet } }).Result;
            int facetCount = ((TermFacetResult)results.Facets["color"]).Terms.Count();
            Assert.Equal(18, facetCount);
        }

        [Fact]
        public void GetProductProjectionsTermFacetCountingProducts()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            Facet<ProductProjection> colorFacet = new TermFacet<ProductProjection>(p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault());
            colorFacet.IsCountingProducts = true;
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(new SearchProductProjectionsCommand() { Facets = new List<Facet<ProductProjection>>() { colorFacet } }).Result;
            var facetCount = ((TermFacetResult)results.Facets["variants.attributes.color.key"]).Terms[0].ProductCount;
            Assert.Equal(572, facetCount);
        }
    }
}