using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Products;
using commercetools.Sdk.Domain.Products.UpdateActions;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.Domain;
using Xunit;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.Products.SetDescriptionUpdateAction;

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
        public void DeleteProductById()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new DeleteByIdCommand<Product>(new Guid(product.Id), product.Version)).Result;
            Assert.ThrowsAsync<HttpApiClientException>(() =>
                commerceToolsClient.ExecuteAsync(new GetByIdCommand<Product>(new Guid(retrievedProduct.Id))));
        }

        [Fact]
        public void DeleteProductByKey()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new DeleteByKeyCommand<Product>(product.Key, product.Version)).Result;
            Assert.ThrowsAsync<HttpApiClientException>(() =>
                commerceToolsClient.ExecuteAsync(new GetByIdCommand<Product>(new Guid(retrievedProduct.Id))));
        }

        #region  UpdateActions

        [Fact]
        public void UpdateProductByIdSetKey()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            string newKey = this.productFixture.RandomString(10);
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction() {Key = newKey};
            updateActions.Add(setKeyAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            // The retrieved product has to be deleted and not the created product.
            // The retrieved product will have version 2 and the created product will have version 1.
            // Only the latest version can be deleted.
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(newKey, retrievedProduct.Key);
        }

        [Fact]
        public void UpdateProductByKeyChangeName()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            string name = this.productFixture.RandomString(10);
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            ChangeNameUpdateAction changeNameUpdateAction = new ChangeNameUpdateAction()
                {Name = new LocalizedString() {{"en", name}}};
            updateActions.Add(changeNameUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Product>(product.Key, product.Version, updateActions)).Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(name, retrievedProduct.MasterData.Staged.Name["en"]);
        }

        [Fact]
        public void UpdateProductByIdSetDescription()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            string newDescription = this.productFixture.RandomString(20);
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            SetDescriptionUpdateAction setDescriptionUpdateAction = new SetDescriptionUpdateAction()
                {Description = new LocalizedString() {{"en", newDescription}}};
            updateActions.Add(setDescriptionUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(newDescription, retrievedProduct.MasterData.Staged.Description["en"]);
        }

        [Fact]
        public void UpdateProductByKeyChangeSlug()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            string newSlug = this.productFixture.RandomString(10);
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            ChangeSlugUpdateAction changeSlugUpdateAction = new ChangeSlugUpdateAction()
                {Slug = new LocalizedString() {{"en", newSlug}}};
            updateActions.Add(changeSlugUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Product>(product.Key, product.Version, updateActions)).Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(newSlug, retrievedProduct.MasterData.Staged.Slug["en"]);
        }

        [Fact]
        public void UpdateProductByIdAddProductVariant()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();

            var productCategory = product.MasterData.Staged.Categories[0];
            AddProductVariantUpdateAction addProductVariantUpdateAction = new AddProductVariantUpdateAction()
            {
                Sku = this.productFixture.RandomString(10),
                Key = this.productFixture.RandomString(10),
                Attributes = this.productFixture.GetListOfRandomAttributes(productCategory.Id, ReferenceTypeId.Category)
            };

            updateActions.Add(addProductVariantUpdateAction);

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            var productVariants = product.MasterData.Staged.Variants;
            var retrievedProductVariants = retrievedProduct.MasterData.Staged.Variants;

            Assert.True(retrievedProductVariants.Count > productVariants.Count);
        }

        [Fact]
        public void UpdateProductByIdRemoveProductVariant()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(true);
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();

            var productVariantId = product.MasterData.Staged.Variants[0].Id;
            RemoveProductVariantUpdateAction removeProductVariantUpdateAction = new RemoveProductVariantUpdateAction()
            {
                Id = productVariantId
            };

            updateActions.Add(removeProductVariantUpdateAction);

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            var productVariants = product.MasterData.Staged.Variants;
            var retrievedProductVariants = retrievedProduct.MasterData.Staged.Variants;

            Assert.True(retrievedProductVariants.Count < productVariants.Count);
        }

        [Fact]
        public void UpdateProductByIdAddToCategory()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Category newCategory = this.productFixture.CreateNewCategory();

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            AddToCategoryUpdateAction addToCategoryUpdateAction = new AddToCategoryUpdateAction()
            {
                OrderHint = this.productFixture.RandomSortOrder(),
                Category =  new ResourceIdentifier() { Id = newCategory.Id}
            };
            updateActions.Add(addToCategoryUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Contains(retrievedProduct.MasterData.Current.Categories, c =>c.Id.Equals(newCategory.Id));

        }
        #endregion
    }
}
