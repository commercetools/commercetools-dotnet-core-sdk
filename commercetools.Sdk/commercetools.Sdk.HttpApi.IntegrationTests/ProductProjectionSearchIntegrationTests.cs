using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Products.Attributes;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ProductProjectionSearchIntegrationTests : IClassFixture<ProductFixture>
    {
        private readonly ProductFixture productFixture;

        public ProductProjectionSearchIntegrationTests(ProductFixture productFixture)
        {
            this.productFixture = productFixture;
        }

        [Fact(Skip = "Depends on the indexed products which might not be there.")]
        public void GetProductProjectionsFilterByCentAmountRange()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Filter<ProductProjection> centAmountFilter = new Filter<ProductProjection>(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));      
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFilter(new List<Filter<ProductProjection>>() { centAmountFilter });
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand(searchParameters);
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            Assert.Equal(19, results.Count);
        }

        [Fact(Skip = "Depends on the indexed products which might not be there.")]
        public void GetProductProjectionsFilterQueryByCentAmountRange()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Filter<ProductProjection> centAmountFilter = new Filter<ProductProjection>(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFilterQuery(new List<Filter<ProductProjection>>() { centAmountFilter });
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand(searchParameters);
            PagedQueryResult<ProductProjection> results = commerceToolsClient.ExecuteAsync(searchProductProjectionsCommand).Result;
            Assert.Equal(19, results.Count);
        }

        [Fact(Skip = "Depends on the indexed products which might not be there.")]
        public void GetProductProjectionsFilterFacetByCentAmountRange()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Filter<ProductProjection> centAmountFilter = new Filter<ProductProjection>(p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 3000)));
            ProductProjectionSearchParameters searchParameters = new ProductProjectionSearchParameters();
            searchParameters.SetFilterFacets(new List<Filter<ProductProjection>>() { centAmountFilter });
            SearchProductProjectionsCommand searchProductProjectionsCommand = new SearchProductProjectionsCommand(searchParameters);
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
    }
}