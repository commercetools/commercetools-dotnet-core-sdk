using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using commercetools.Sdk.Domain.Products;
using commercetools.Sdk.Domain.Query;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    [Collection("Integration Tests")]
    public class ProductIntegrationTests : IClassFixture<ProductFixture>
    {
        private readonly ProductFixture productFixture;

        public ProductIntegrationTests(ProductFixture productFixture)
        {
            this.productFixture = productFixture;
        }

        [Fact]
        public void CreateProduct()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            ProductDraft productDraft = this.productFixture.GetProductDraft();
            Product product = commerceToolsClient.ExecuteAsync(new CreateCommand<Product>(productDraft)).Result;
            this.productFixture.ProductsToDelete.Add(product);
            Assert.Equal(productDraft.Name["en"], product.MasterData.Current.Name["en"]);
        }

        [Fact]
        public void UploadProductImage()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            Stream imageStream = File.OpenRead("Resources/commercetools.png");
            UploadProductImageCommand uploadCommand =
                new UploadProductImageCommand(new Guid(product.Id), imageStream, "image/png");
            Product updatedProduct = commerceToolsClient.ExecuteAsync(uploadCommand).Result;
            this.productFixture.ProductsToDelete.Add(updatedProduct);
            Assert.NotNull(updatedProduct.MasterData.Staged.MasterVariant.Images[0].Url);
        }

        [Fact]
        public void GetProductById()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            this.productFixture.ProductsToDelete.Add(product);
            Product retrievedProduct =
                commerceToolsClient.ExecuteAsync(new GetByIdCommand<Product>(new Guid(product.Id))).Result;
            Assert.Equal(product.Id, retrievedProduct.Id);
        }

        [Fact]
        public void GetProductByIdWithAdditionalParameters()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            ProductAdditionalParameters productAdditionalParameters = new ProductAdditionalParameters();
            productAdditionalParameters.PriceCurrency = "EUR";
            Product product = this.productFixture.CreateProduct();
            this.productFixture.ProductsToDelete.Add(product);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<Product>(new Guid(product.Id), productAdditionalParameters)).Result;
            Assert.Equal(product.Id, retrievedProduct.Id);
        }

        [Fact]
        public void GetProductByKey()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            this.productFixture.ProductsToDelete.Add(product);
            Product retrievedProduct =
                commerceToolsClient.ExecuteAsync(new GetByKeyCommand<Product>(product.Key)).Result;
            Assert.Equal(product.Key, retrievedProduct.Key);
        }

        [Fact]
        public void QueryProduct()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            this.productFixture.ProductsToDelete.Add(product);
            string key = product.Key;
            QueryPredicate<Product> queryPredicate = new QueryPredicate<Product>(p => p.Key == key);
            QueryCommand<Product> queryCommand = new QueryCommand<Product>();
            queryCommand.SetWhere(queryPredicate);
            PagedQueryResult<Product> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, p => p.Key == product.Key);
        }


        [Fact]
        public void QueryProductAndExpandParentProductType()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            this.productFixture.ProductsToDelete.Add(product);
            string key = product.Key;
            QueryPredicate<Product> queryPredicate = new QueryPredicate<Product>(p => p.Key == key);
            List<Expansion<Product>> expansions = new List<Expansion<Product>>();
            ReferenceExpansion<Product> expand = new ReferenceExpansion<Product>(p => p.ProductType);
            expansions.Add(expand);
            QueryCommand<Product> queryCommand = new QueryCommand<Product>();
            queryCommand.SetWhere(queryPredicate);
            queryCommand.SetExpand(expansions);
            PagedQueryResult<Product> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, p => p.Key == product.Key && p.ProductType.Obj != null);
        }


        [Fact]
        public void QueryAndSortProducts()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            for (int i = 0; i < 3; i++)
            {
                Product product = this.productFixture.CreateProduct();
                this.productFixture.ProductsToDelete.Add(product);
            }

            List<Sort<Product>> sortPredicates = new List<Sort<Product>>();
            Sort<Product> sort = new Sort<Product>(p => p.Key);
            sortPredicates.Add(sort);
            QueryCommand<Product> queryCommand = new QueryCommand<Product>();
            queryCommand.SetSort(sortPredicates);

            PagedQueryResult<Product> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            var sortedList = returnedSet.Results.OrderBy(p => p.Key);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }
        
        [Fact]
        public void QueryAndSortProductsDescending()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            for (int i = 0; i < 3; i++)
            {
                Product product = this.productFixture.CreateProduct();
                this.productFixture.ProductsToDelete.Add(product);
            }

            List<Sort<Product>> sortPredicates = new List<Sort<Product>>();
            Sort<Product> sort = new Sort<Product>(p => p.Key, SortDirection.Descending);
            sortPredicates.Add(sort);
            QueryCommand<Product> queryCommand = new QueryCommand<Product>();
            queryCommand.SetSort(sortPredicates);

            PagedQueryResult<Product> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            var sortedList = returnedSet.Results.OrderByDescending(p => p.Key);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }
        
        
        [Fact]
        public void UpdateProductByIdSetKey()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            string newKey = this.productFixture.RandomString(5);
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction() { Key = newKey };
            updateActions.Add(setKeyAction);
            Product retrievedProduct = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions)).Result;
            // The retrieved product has to be deleted and not the created category.
            // The retrieved product will have version 2 and the created category will have version 1.
            // Only the latest version can be deleted.
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(newKey, retrievedProduct.Key);
        }
        
       
    }
}