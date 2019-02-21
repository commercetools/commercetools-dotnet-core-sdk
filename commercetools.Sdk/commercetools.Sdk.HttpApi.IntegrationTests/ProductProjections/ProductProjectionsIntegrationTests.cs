using System;
using System.Linq;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Query;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ProductProjections
{
    [Collection("Integration Tests")]
    public class ProductProjectionsIntegrationTests : IClassFixture<ProductProjectionsFixture>
    {
        private readonly ProductProjectionsFixture productProjectionsFixture;

        public ProductProjectionsIntegrationTests(ProductProjectionsFixture productProjectionsFixture)
        {
            this.productProjectionsFixture = productProjectionsFixture;
        }
        
        [Fact]
        public void GetProductProjectionById()
        {
            //Arrange
            IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
            var stagedProduct = this.productProjectionsFixture.productFixture.CreateProduct(true);
            var publishedProduct = this.productProjectionsFixture.productFixture.CreateProductAndPublishIt(true);
            ProductProjectionAdditionalParameters stagedAdditionalParameters = new ProductProjectionAdditionalParameters();
            stagedAdditionalParameters.Staged = true;
            
            //Act
            var stagedProductProjection = commerceToolsClient.ExecuteAsync(new GetByIdCommand<ProductProjection>(new Guid(stagedProduct.Id), stagedAdditionalParameters)).Result;
            var publishedProductProjection = commerceToolsClient.ExecuteAsync(new GetByIdCommand<ProductProjection>(new Guid(publishedProduct.Id))).Result;

            publishedProduct = this.productProjectionsFixture.productFixture.Unpublish(publishedProduct);//unpublish it before dispose 
            this.productProjectionsFixture.productFixture.ProductsToDelete.Add(stagedProduct);
            this.productProjectionsFixture.productFixture.ProductsToDelete.Add(publishedProduct);
            
            //Assert
            Assert.Equal(stagedProduct.Id, stagedProductProjection.Id);
            Assert.True(stagedProductProjection.Published == false);
            Assert.Equal(publishedProduct.Id, publishedProductProjection.Id);
            Assert.True(publishedProductProjection.Published);
        }
        
        [Fact]
        public void GetProductProjectionByKey()
        {
            //Arrange
            IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
            var stagedProduct = this.productProjectionsFixture.productFixture.CreateProduct(true);
            var publishedProduct = this.productProjectionsFixture.productFixture.CreateProductAndPublishIt(true);
            ProductProjectionAdditionalParameters stagedAdditionalParameters = new ProductProjectionAdditionalParameters();
            stagedAdditionalParameters.Staged = true;
            
            //Act
            var stagedProductProjection = commerceToolsClient.ExecuteAsync(new GetByKeyCommand<ProductProjection>(stagedProduct.Key, stagedAdditionalParameters)).Result;
            var publishedProductProjection = commerceToolsClient.ExecuteAsync(new GetByKeyCommand<ProductProjection>(publishedProduct.Key)).Result;

            publishedProduct = this.productProjectionsFixture.productFixture.Unpublish(publishedProduct);//unpublish it before dispose 
            this.productProjectionsFixture.productFixture.ProductsToDelete.Add(stagedProduct);
            this.productProjectionsFixture.productFixture.ProductsToDelete.Add(publishedProduct);
            
            //Assert
            Assert.Equal(stagedProduct.Key, stagedProductProjection.Key);
            Assert.True(stagedProductProjection.Published == false);
            Assert.Equal(publishedProduct.Key, publishedProductProjection.Key);
            Assert.True(publishedProductProjection.Published);
        }

        [Fact]
        public void QueryCurrentProductProjections()
        {
            //Arrange
            IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();
            var publishedProduct = this.productProjectionsFixture.productFixture.CreateProductAndPublishIt(true);
            
            //Act
            QueryPredicate<ProductProjection> queryPredicate = new QueryPredicate<ProductProjection>(productProjection => productProjection.Key == publishedProduct.Key.valueOf());
            QueryCommand<ProductProjection> queryCommand = new QueryCommand<ProductProjection>();
            queryCommand.SetWhere(queryPredicate);
            PagedQueryResult<ProductProjection> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            
            publishedProduct = this.productProjectionsFixture.productFixture.Unpublish(publishedProduct);//unpublish it before dispose 
            this.productProjectionsFixture.productFixture.ProductsToDelete.Add(publishedProduct);
            
            //Assert
            Assert.NotNull(returnedSet.Results);
            Assert.Contains(returnedSet.Results, pp => pp.Id == publishedProduct.Id);
        }
        
        [Fact]
        public void QueryAndOffsetStagedProductProjections()
        {
            //Arrange
            IClient commerceToolsClient = this.productProjectionsFixture.GetService<IClient>();

            for (int i = 0; i < 3; i++)
            {
                Product stagedProduct = this.productProjectionsFixture.productFixture.CreateProduct(true);
                this.productProjectionsFixture.productFixture.ProductsToDelete.Add(stagedProduct);    
            }

            //Act
            ProductProjectionAdditionalParameters stagedAdditionalParameters = new ProductProjectionAdditionalParameters();
            stagedAdditionalParameters.Staged = true;
            
            QueryPredicate<ProductProjection> queryPredicate = new QueryPredicate<ProductProjection>(productProjection => productProjection.Version == 1);
            QueryCommand<ProductProjection> queryCommand = new QueryCommand<ProductProjection>(stagedAdditionalParameters);
            queryCommand.SetWhere(queryPredicate);
            queryCommand.Offset = 2;
            PagedQueryResult<ProductProjection> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            
            //Assert
            Assert.Single(returnedSet.Results);
            Assert.Equal(3, returnedSet.Total);
        }
    }
}