using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.Products;
using commercetools.Sdk.Domain.Products.UpdateActions;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.Products.UpdateActions.SetDescriptionUpdateAction;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Products
{
    [Collection("Integration Tests")]
    public class ProductIntegrationTests : IClassFixture<ServiceProviderFixture>, IDisposable
    {
        private readonly ProductFixture productFixture;

        public ProductIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.productFixture = new ProductFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.productFixture.Dispose();
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

            ProductType productType = this.productFixture.CreateNewProductType();
            for (int i = 0; i < 3; i++)
            {
                Product product = this.productFixture.CreateProduct(productType);
                this.productFixture.ProductsToDelete.Add(product);
            }

            List<Sort<Product>> sortPredicates = new List<Sort<Product>>();
            Sort<Product> sort = new Sort<Product>(p => p.Key);
            sortPredicates.Add(sort);
            QueryCommand<Product> queryCommand = new QueryCommand<Product>();
            queryCommand.Where(product => product.ProductType.Id == productType.Id.valueOf());
            queryCommand.SetSort(sortPredicates);

            PagedQueryResult<Product> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            var sortedList = returnedSet.Results.OrderBy(p => p.Key);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }

        [Fact]
        public void QueryAndSortProductsDescending()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            ProductType productType = this.productFixture.CreateNewProductType();

            for (int i = 0; i < 3; i++)
            {
                Product product = this.productFixture.CreateProduct(productType);
                this.productFixture.ProductsToDelete.Add(product);
            }

            List<Sort<Product>> sortPredicates = new List<Sort<Product>>();
            Sort<Product> sort = new Sort<Product>(p => p.Key, SortDirection.Descending);
            sortPredicates.Add(sort);
            QueryCommand<Product> queryCommand = new QueryCommand<Product>();
            queryCommand.Where(product => product.ProductType.Id == productType.Id.valueOf());
            queryCommand.SetSort(sortPredicates);

            PagedQueryResult<Product> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            var sortedList = returnedSet.Results.OrderByDescending(p => p.Key);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }

        [Fact]
        public async void DeleteProductById()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new DeleteByIdCommand<Product>(new Guid(product.Id), product.Version)).Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<Product>(new Guid(retrievedProduct.Id))));
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public async void DeleteProductByKey()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new DeleteByKeyCommand<Product>(product.Key, product.Version)).Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<Product>(new Guid(retrievedProduct.Id))));
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void Publish()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            Assert.False(product.MasterData.Published);//not published

            //publish it
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            PublishUpdateAction publishUpdateAction = new PublishUpdateAction()
            {
                Scope = PublishScope.All
            };
            updateActions.Add(publishUpdateAction);

            Product publishedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(publishedProduct);
            Assert.True(publishedProduct.MasterData.Published);//published
        }

        [Fact]
        public void Unpublish()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(publish:true);
            Assert.True(product.MasterData.Published);//published

            //unpublish it
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            UnpublishUpdateAction unpublish = new UnpublishUpdateAction();
            updateActions.Add(unpublish);

            Product unpublishedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(unpublishedProduct);
            Assert.False(unpublishedProduct.MasterData.Published);//published
        }

        #region  UpdateActions

        [Fact]
        public void UpdateProductByIdSetKey()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            string newKey = TestingUtility.RandomString(10);
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
            string name = TestingUtility.RandomString(10);
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
            string newDescription = TestingUtility.RandomString(20);
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
            string newSlug = TestingUtility.RandomString(10);
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
        public void UpdateProductAddProductVariant()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();

            var productCategory = product.MasterData.Staged.Categories[0];
            AddProductVariantUpdateAction addProductVariantUpdateAction = new AddProductVariantUpdateAction()
            {
                Sku = TestingUtility.RandomString(10),
                Key = TestingUtility.RandomString(10),
                Attributes = TestingUtility.GetListOfRandomAttributes(productCategory.Id, ReferenceTypeId.Category)
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
        public void UpdateProductChangeMasterVariantBySku()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withVariants: true);

            Assert.NotEmpty(product.MasterData.Staged.Variants);
            string newMasterVariantSku = product.MasterData.Staged.Variants[0].Sku;

            ChangeMasterVariantUpdateAction changeMasterVariantUpdateAction =
                new ChangeMasterVariantUpdateAction(newMasterVariantSku);
            ;
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>()
                {changeMasterVariantUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Equal(newMasterVariantSku, retrievedProduct.MasterData.Staged.MasterVariant.Sku);
        }

        [Fact]
        public void UpdateProductChangeMasterVariantByVariantId()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withVariants: true);

            Assert.NotEmpty(product.MasterData.Staged.Variants);
            int newMasterVariantId = product.MasterData.Staged.Variants[0].Id;

            ChangeMasterVariantUpdateAction changeMasterVariantUpdateAction =
                new ChangeMasterVariantUpdateAction(newMasterVariantId);
            ;
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>()
                {changeMasterVariantUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Equal(newMasterVariantId, retrievedProduct.MasterData.Staged.MasterVariant.Id);
        }

        [Fact]
        public void UpdateProductAddPriceBySku()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withVariants: true);

            Assert.NotEmpty(product.MasterData.Staged.Variants);
            Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

            string sku = product.MasterData.Staged.MasterVariant.Sku;

            var newProductPrice = TestingUtility.GetPriceDraft(TestingUtility.RandomInt(1000, 5000),
                DateTime.Now.AddMonths(6), DateTime.Now.AddMonths(7));

            AddPriceUpdateAction addPriceUpdateAction =
                    new AddPriceUpdateAction(sku, newProductPrice)
                ;
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>() {addPriceUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Equal(product.MasterData.Staged.MasterVariant.Prices.Count + 1,
                retrievedProduct.MasterData.Staged.MasterVariant.Prices.Count);
        }

        [Fact]
        public void UpdateProductSetPrices()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withVariants: true);

            Assert.NotEmpty(product.MasterData.Staged.Variants);
            Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

            string sku = product.MasterData.Staged.MasterVariant.Sku;

            var prices = TestingUtility.GetRandomListOfPriceDraft(3);

            SetPricesUpdateAction setPricesUpdateAction =
                new SetPricesUpdateAction(sku, prices);
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>() {setPricesUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Prices.Count);
        }


        [Fact]
        public void UpdateProductChangePrice()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

            var oldPrice = product.MasterData.Staged.MasterVariant.Prices[0];

            var newProductPrice = TestingUtility.GetPriceDraft(TestingUtility.RandomInt(1000, 5000),
                DateTime.Now.AddMonths(5), DateTime.Now.AddMonths(6));

            ChangePriceUpdateAction changePriceUpdateAction =
                    new ChangePriceUpdateAction
                    {
                        PriceId = oldPrice.Id,
                        Price = newProductPrice,
                        Staged = true
                    }
                ;
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>() {changePriceUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.NotEmpty(retrievedProduct.MasterData.Staged.MasterVariant.Prices);
            Assert.Equal(newProductPrice.Value, retrievedProduct.MasterData.Staged.MasterVariant.Prices[0].Value);
        }


        [Fact]
        public void UpdateProductRemovePrice()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices); //with 2 prices

            var removePrice = product.MasterData.Staged.MasterVariant.Prices[1];

            RemovePriceUpdateAction removePriceUpdateAction =
                    new RemovePriceUpdateAction
                    {
                        PriceId = removePrice.Id,
                        Staged = true
                    }
                ;
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>() {removePriceUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Single(retrievedProduct.MasterData.Staged.MasterVariant.Prices);
            Assert.NotEqual(removePrice.Id, retrievedProduct.MasterData.Staged.MasterVariant.Prices[0].Id);
        }

        [Fact]
        public void UpdateProductSetPriceCustomType()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

            var price = product.MasterData.Staged.MasterVariant.Prices[0];

            var customType = this.productFixture.CreateCustomType();
            var fields = this.productFixture.CreateNewFields();

            SetPriceCustomTypeUpdateAction setCustomTypeUpdateAction = new SetPriceCustomTypeUpdateAction()
            {
                Type = new ResourceIdentifier<Type>
                {
                    Key = customType.Key
                },
                Fields = fields,
                PriceId = price.Id
            };

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {setCustomTypeUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id),
                    product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Equal(customType.Id, retrievedProduct.MasterData.Staged.MasterVariant.Prices[0].Custom.Type.Id);
        }

        [Fact]
        public void UpdateProductSetPriceCustomField()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

            // first set custom type for the price
            var price = product.MasterData.Staged.MasterVariant.Prices[0];

            var customType = this.productFixture.CreateCustomType();
            var fields = this.productFixture.CreateNewFields();

            SetPriceCustomTypeUpdateAction setCustomTypeUpdateAction = new SetPriceCustomTypeUpdateAction()
            {
                Type = new ResourceIdentifier<Type>
                {
                    Key = customType.Key
                },
                Fields = fields,
                PriceId = price.Id
            };

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {setCustomTypeUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id),
                    product.Version, updateActions))
                .Result;

            //Then update the custom field
            string stringFieldValue = TestingUtility.RandomString(10);
            updateActions.Clear();
            SetPriceCustomFieldUpdateAction setCustomFieldUpdateAction = new SetPriceCustomFieldUpdateAction()
            {
                Name = "string-field",
                Value = stringFieldValue,
                PriceId = price.Id
            };
            updateActions.Add(setCustomFieldUpdateAction);

            retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(retrievedProduct.Id),
                    retrievedProduct.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Equal(stringFieldValue,
                retrievedProduct.MasterData.Staged.MasterVariant.Prices[0].Custom.Fields["string-field"]);
        }


        [Fact]
        public void UpdateProductSetDiscountedPrice()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.NotEmpty(product.MasterData.Staged.MasterVariant.Prices);

            var price = product.MasterData.Staged.MasterVariant.Prices[0];
            var productDiscount = this.productFixture.CreateProductDiscount();
            var discountedPrice = new DiscountedPrice
            {
                Value = Money.Parse("100 EUR"),
                Discount = new Reference<ProductDiscount> {Id = productDiscount.Id}
            };
            SetDiscountedPriceUpdateAction setDiscountedPriceUpdateAction = new SetDiscountedPriceUpdateAction
            {
                PriceId = price.Id,
                Discounted = discountedPrice,
                Staged = true
            };
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {setDiscountedPriceUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id),
                    product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(discountedPrice.Value, retrievedProduct.MasterData.Staged.MasterVariant.Prices[0].Discounted.Value);
            Assert.Equal(discountedPrice.Discount.Id, retrievedProduct.MasterData.Staged.MasterVariant.Prices[0].Discounted.Discount.Id);
        }


        [Fact]
        public void UpdateProductSetAttribute()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            string textAttributeName = "text-attribute-name";
            string sku = product.MasterData.Current.MasterVariant.Sku;
            var newTextAttributeValue = TestingUtility.RandomString(10);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            SetAttributeUpdateAction setAttributeUpdateAction =
                new SetAttributeUpdateAction(sku, textAttributeName, newTextAttributeValue);
            updateActions.Add(setAttributeUpdateAction);

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            var productAttributeValue =
                retrievedProduct.MasterData.Staged.MasterVariant.GetTextAttributeValue(textAttributeName);

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.NotNull(productAttributeValue);
            Assert.Equal(newTextAttributeValue, productAttributeValue);
        }

        [Fact]
        public void UpdateProductSetAttributeInAllVariants()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withVariants: true);

            string textAttributeName = "text-attribute-name";
            var newTextAttributeValue = TestingUtility.RandomString(10);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            SetAttributeInAllVariantsUpdateAction setAttributeUpdateAction =
                new SetAttributeInAllVariantsUpdateAction
                {
                    Name = textAttributeName,
                    Value = newTextAttributeValue,
                    Staged = true
                };
            updateActions.Add(setAttributeUpdateAction);

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            var productAttributeValue =
                retrievedProduct.MasterData.Staged.MasterVariant.GetTextAttributeValue(textAttributeName);
            var productAttributeValueVariant =
                retrievedProduct.MasterData.Staged.Variants[1].GetTextAttributeValue(textAttributeName);

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.NotNull(productAttributeValue);
            Assert.Equal(newTextAttributeValue, productAttributeValue);
            Assert.Equal(newTextAttributeValue, productAttributeValueVariant);
        }

        [Fact]
        public void UpdateProductAddToCategory()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Category newCategory = this.productFixture.CreateNewCategory();

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            AddToCategoryUpdateAction addToCategoryUpdateAction = new AddToCategoryUpdateAction()
            {
                OrderHint = TestingUtility.RandomSortOrder(),
                Category = new ResourceIdentifier<Category> {Key = newCategory.Key}
            };
            updateActions.Add(addToCategoryUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Contains(retrievedProduct.MasterData.Current.Categories, c => c.Id.Equals(newCategory.Id));
        }

        [Fact]
        public void UpdateProductSetCategoryOrderHint()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.Single(product.MasterData.Staged.Categories);

            string newOrderHint = TestingUtility.RandomSortOrder();
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            var categoryId = product.MasterData.Staged.Categories[0].Id;
            SetCategoryOrderHintUpdateAction setOrderHintUpdateAction = new SetCategoryOrderHintUpdateAction()
            {
                OrderHint = newOrderHint,
                CategoryId = categoryId,
                Staged = true
            };
            updateActions.Add(setOrderHintUpdateAction);

            //expansions
            List<Expansion<Product>> expansions = new List<Expansion<Product>>();
            ReferenceExpansion<Product> expand =
                new ReferenceExpansion<Product>(p => p.MasterData.Staged.Categories.ExpandAll());
            expansions.Add(expand);

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions,
                    expansions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Equal(newOrderHint, retrievedProduct.MasterData.Staged.CategoryOrderHints[categoryId]);
        }

        [Fact]
        public void UpdateProductRemoveFromCategory()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.Single(product.MasterData.Staged.Categories);

            var category = product.MasterData.Staged.Categories[0];
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            RemoveFromCategoryUpdateAction removeFromCategoryUpdateAction = new RemoveFromCategoryUpdateAction()
            {
                Staged = true,
                Category = new ResourceIdentifier<Category> {Id = category.Id}
            };
            updateActions.Add(removeFromCategoryUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Empty(retrievedProduct.MasterData.Staged.Categories);
        }

        [Fact]
        public void UpdateProductSetTaxCategory()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(publish: true);

            var taxCategory = this.productFixture.CreateNewTaxCategory();

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            SetTaxCategoryUpdateAction setTaxCategoryUpdateAction = new SetTaxCategoryUpdateAction()
            {
                TaxCategory = new ResourceIdentifier<TaxCategory>
                {
                    Key = taxCategory.Key
                }
            };
            updateActions.Add(setTaxCategoryUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(taxCategory.Id, retrievedProduct.TaxCategory.Id);
        }

        [Fact]
        public void UpdateProductSetSku()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            var sku = TestingUtility.RandomString(10);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            SetSkuUpdateAction setSkuUpdateAction = new SetSkuUpdateAction()
            {
                Sku = sku,
                VariantId = product.MasterData.Staged.MasterVariant.Id,
                Staged = true
            };
            updateActions.Add(setSkuUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(sku, retrievedProduct.MasterData.Staged.MasterVariant.Sku);
        }

        [Fact]
        public void UpdateProductSetProductVariantKey()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            var key = TestingUtility.RandomString(10);
            var sku = product.MasterData.Staged.MasterVariant.Sku;

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            SetProductVariantKeyUpdateAction setKeyUpdateAction = new SetProductVariantKeyUpdateAction(sku, key);
            updateActions.Add(setKeyUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(key, retrievedProduct.MasterData.Staged.MasterVariant.Key);
        }

        [Fact]
        public void UpdateProductAddExternalImage()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.Empty(product.MasterData.Staged.MasterVariant.Images);
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var image = new Image
            {
                Label = $"Test-External-Image-{TestingUtility.RandomInt()}",
                Url = TestingUtility.ExternalImageUrl,
                Dimensions = new Dimensions {W = 50, H = 50}
            };

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            AddExternalImageUpdateAction addExternalImageUpdateAction = new AddExternalImageUpdateAction(sku, image);

            updateActions.Add(addExternalImageUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Single(retrievedProduct.MasterData.Staged.MasterVariant.Images);
            Assert.Equal(image.Url, retrievedProduct.MasterData.Staged.MasterVariant.Images[0].Url);
        }

        [Fact]
        public void UpdateProductMoveImageToPosition()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withImages: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Images.Count);

            //move second image to first position
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var image = product.MasterData.Staged.MasterVariant.Images[1];

            MoveImageToPositionUpdateAction moveImageToPositionUpdateAction =
                new MoveImageToPositionUpdateAction(sku, image.Url, 0, true);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>
                {moveImageToPositionUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Images.Count);
            Assert.Equal(image.Label, retrievedProduct.MasterData.Staged.MasterVariant.Images[0].Label);
        }

        [Fact]
        public void UpdateProductRemoveImage()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withImages: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Images.Count);

            //remove second image
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var image = product.MasterData.Staged.MasterVariant.Images[1];

            RemoveImageUpdateAction removeImageUpdateAction =
                new RemoveImageUpdateAction(sku, image.Url);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {removeImageUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(2, retrievedProduct.MasterData.Staged.MasterVariant.Images.Count);
        }

        [Fact]
        public void UpdateProductSetImageLabel()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withImages: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Images.Count);

            //update the first image label
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var image = product.MasterData.Staged.MasterVariant.Images[0];
            var newLabel = TestingUtility.RandomString(10);

            SetImageLabelUpdateAction setImageLabelUpdateAction =
                new SetImageLabelUpdateAction(sku, image.Url, newLabel);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {setImageLabelUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Images.Count);
            Assert.Equal(newLabel, retrievedProduct.MasterData.Staged.MasterVariant.Images[0].Label);
        }

        [Fact]
        public void UpdateProductAddAsset()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.Empty(product.MasterData.Staged.MasterVariant.Assets);
            var sku = product.MasterData.Staged.MasterVariant.Sku;

            var asset = TestingUtility.GetAssetDraft();

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            AddAssetUpdateAction addAssetUpdateAction = new AddAssetUpdateAction(sku, asset, 0);

            updateActions.Add(addAssetUpdateAction);
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;
            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.Single(retrievedProduct.MasterData.Staged.MasterVariant.Assets);
            Assert.Equal(asset.Key, retrievedProduct.MasterData.Staged.MasterVariant.Assets[0].Key);
        }

        [Fact]
        public void UpdateProductRemoveAssetUsingAssetId()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

            //remove second asset By Sku and assetID
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];

            RemoveAssetUpdateAction removeAssetUpdateAction =
                new RemoveAssetUpdateAction(sku: sku, assetId: asset.Id, assetKey: null, staged: true);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {removeAssetUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(2, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
        }

        [Fact]
        public void UpdateProductRemoveAssetUsingAssetKey()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

            //remove second asset by VariantId and assetKey
            var variantId = product.MasterData.Staged.MasterVariant.Id;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];

            RemoveAssetUpdateAction removeAssetUpdateAction =
                new RemoveAssetUpdateAction(variantId: variantId, assetId: null, assetKey: asset.Key, staged: true);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {removeAssetUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(2, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
        }

        [Fact]
        public void UpdateProductSetAssetKey()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

            //set second asset Key
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];
            var key = TestingUtility.RandomString(10);

            SetAssetKeyUpdateAction removeAssetUpdateAction =
                new SetAssetKeyUpdateAction(sku: sku, assetId: asset.Id, assetKey: key, staged: true);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {removeAssetUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Equal(key, retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Key);
        }

        [Fact]
        public void UpdateProductChangeAssetOrder()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

            //reverse the assets orders
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var assets = product.MasterData.Staged.MasterVariant.Assets;
            assets.Reverse();
            var reversedAssetsOrder = assets.Select(asset => asset.Id).ToList();

            ChangeAssetOrderUpdateAction changeAssetOrderUpdateAction =
                new ChangeAssetOrderUpdateAction(sku, reversedAssetsOrder);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {changeAssetOrderUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            var newAssetsOrder = retrievedProduct.MasterData.Staged.MasterVariant.Assets.Select(asset => asset.Id)
                .ToList();

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Equal(reversedAssetsOrder, newAssetsOrder);
        }

        [Fact]
        public void UpdateProductChangeAssetNameBySku()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

            //change name of the second asset By Sku and Asset Key
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];
            var name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};

            ChangeAssetNameUpdateAction changeAssetNameUpdateAction =
                new ChangeAssetNameUpdateAction(sku: sku, assetKey: asset.Key, name: name);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {changeAssetNameUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Equal(name["en"], retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Name["en"]);
        }

        [Fact]
        public void UpdateProductChangeAssetNameByVariantId()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

            //change name of the second asset By Variant Id and Asset Id
            var variantId = product.MasterData.Staged.MasterVariant.Id;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];
            var name = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};

            ChangeAssetNameUpdateAction changeAssetNameUpdateAction =
                new ChangeAssetNameUpdateAction(variantId: variantId, assetId: asset.Id, name: name);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {changeAssetNameUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Equal(name["en"], retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Name["en"]);
        }

        [Fact]
        public void UpdateProductSetAssetDescriptionBySku()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

            //set description of the second asset By Sku and Asset Key
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];
            var description = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};

            SetAssetDescriptionUpdateAction setAssetDescriptionUpdateAction =
                new SetAssetDescriptionUpdateAction(sku: sku, assetKey: asset.Key, description: description);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>
                {setAssetDescriptionUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Equal(description["en"],
                retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Description["en"]);
        }

        [Fact]
        public void UpdateProductSetAssetDescriptionByVariantId()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

            //set description of the second asset By VariantId and Asset Id
            var variantId = product.MasterData.Staged.MasterVariant.Id;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];
            var description = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};

            SetAssetDescriptionUpdateAction setAssetDescriptionUpdateAction =
                new SetAssetDescriptionUpdateAction(variantId: variantId, assetId: asset.Id, description: description);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>
                {setAssetDescriptionUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Equal(description["en"],
                retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Description["en"]);
        }

        [Fact]
        public void UpdateProductSetAssetTagsBySku()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);

            //set tags of the second asset By Sku and Asset Key
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];
            var newTags = new List<string> {"Tag1"};

            SetAssetTagsUpdateAction setAssetDescriptionUpdateAction =
                new SetAssetTagsUpdateAction(sku: sku, assetKey: asset.Key, tags: newTags);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>
                {setAssetDescriptionUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Equal(newTags, retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Tags);
        }

        [Fact]
        public void UpdateProductSetAssetSourcesBySku()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Single(product.MasterData.Staged.MasterVariant.Assets[1].Sources);

            //set tags of the second asset By Sku and Asset Key
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];
            var assetSources = TestingUtility.GetListOfAssetSource();

            SetAssetSourcesUpdateAction setAssetSourcesUpdateAction =
                new SetAssetSourcesUpdateAction(sku: sku, assetKey: asset.Key, sources: assetSources);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {setAssetSourcesUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Equal(2, retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Sources.Count);
            Assert.Equal(assetSources[0].Key,
                retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Sources[0].Key);
        }

        [Fact]
        public void UpdateProductSetAssetCustomType()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Null(product.MasterData.Staged.MasterVariant.Assets[1].Custom);

            //set custom type of the second asset By Sku and Asset Key
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];
            var customType = this.productFixture.CreateCustomType();
            var fields = this.productFixture.CreateNewFields();
            var typeResourceIdentifier = new ResourceIdentifier<Type> {Key = customType.Key};

            SetAssetCustomTypeUpdateAction setAssetCustomTypeUpdateAction =
                new SetAssetCustomTypeUpdateAction(sku: sku, assetKey: asset.Key, type: typeResourceIdentifier,
                    fields: fields);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>
                {setAssetCustomTypeUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.NotNull(retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Type);
            Assert.Equal(customType.Id, retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Type.Id);
        }

        [Fact]
        public void UpdateProductSetAssetCustomField()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(withAssets: true);

            Assert.Equal(3, product.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.Null(product.MasterData.Staged.MasterVariant.Assets[1].Custom);

            //set custom type of the second asset By Sku and Asset Key
            var sku = product.MasterData.Staged.MasterVariant.Sku;
            var asset = product.MasterData.Staged.MasterVariant.Assets[1];
            var customType = this.productFixture.CreateCustomType();
            var fields = this.productFixture.CreateNewFields();
            var typeResourceIdentifier = new ResourceIdentifier<Type> {Key = customType.Key};

            SetAssetCustomTypeUpdateAction setAssetCustomTypeUpdateAction =
                new SetAssetCustomTypeUpdateAction(sku: sku, assetKey: asset.Key, type: typeResourceIdentifier,
                    fields: fields);

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>
                {setAssetCustomTypeUpdateAction};

            var retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.Equal(3, retrievedProduct.MasterData.Staged.MasterVariant.Assets.Count);
            Assert.NotNull(retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Type);
            Assert.Equal(customType.Id, retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Type.Id);

            //Then update the custom field
            string stringFieldValue = TestingUtility.RandomString(5);
            updateActions.Clear();
            SetAssetCustomFieldUpdateAction setCustomFieldUpdateAction =
                new SetAssetCustomFieldUpdateAction(sku: sku, assetKey: asset.Key, name: "string-field",
                    value: stringFieldValue);

            updateActions.Add(setCustomFieldUpdateAction);

            retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(retrievedProduct.Id),
                    retrievedProduct.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);
            Assert.Equal(stringFieldValue,
                retrievedProduct.MasterData.Staged.MasterVariant.Assets[1].Custom.Fields["string-field"]);
        }

        [Fact]
        public void UpdateProductSetSearchKeywords()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.Empty(product.MasterData.Staged.SearchKeywords);

            var searchKeywords = TestingUtility.GetSearchKeywords();

            SetSearchKeywordsUpdateAction setSearchKeywordsUpdateAction = new SetSearchKeywordsUpdateAction()
            {
                SearchKeywords = searchKeywords,
                Staged = true
            };
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            updateActions.Add(setSearchKeywordsUpdateAction);

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.NotEmpty(retrievedProduct.MasterData.Staged.SearchKeywords);
            Assert.NotEmpty(retrievedProduct.MasterData.Staged.SearchKeywords["en"]);
            Assert.Equal(searchKeywords["en"][0].Text, retrievedProduct.MasterData.Staged.SearchKeywords["en"][0].Text);
        }

        [Fact]
        public void UpdateProductSetMetaTitle()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.Null(product.MasterData.Staged.MetaTitle);

            var metaTitle = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};

            SetMetaTitleUpdateAction setMetaTitleUpdateAction = new SetMetaTitleUpdateAction()
            {
                MetaTitle = metaTitle,
                Staged = true
            };
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            updateActions.Add(setMetaTitleUpdateAction);

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.NotNull(retrievedProduct.MasterData.Staged.MetaTitle);
            Assert.Equal(metaTitle["en"], retrievedProduct.MasterData.Staged.MetaTitle["en"]);
        }

        [Fact]
        public void UpdateProductSetMetaDescription()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.Null(product.MasterData.Staged.MetaDescription);

            var metaDescription = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};

            SetMetaDescriptionUpdateAction setMetaDescriptionUpdateAction = new SetMetaDescriptionUpdateAction()
            {
                MetaDescription = metaDescription,
                Staged = true
            };
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            updateActions.Add(setMetaDescriptionUpdateAction);

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.NotNull(retrievedProduct.MasterData.Staged.MetaDescription);
            Assert.Equal(metaDescription["en"], retrievedProduct.MasterData.Staged.MetaDescription["en"]);
        }

        [Fact]
        public void UpdateProductSetMetaKeywords()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.Null(product.MasterData.Staged.MetaKeywords);

            var metaKeywords = new LocalizedString() {{"en", TestingUtility.RandomString(10)}};

            SetMetaKeywordsUpdateAction setMetaKeywordsUpdateAction = new SetMetaKeywordsUpdateAction()
            {
                MetaKeywords = metaKeywords,
                Staged = true
            };
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            updateActions.Add(setMetaKeywordsUpdateAction);

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.True(retrievedProduct.MasterData.HasStagedChanges);
            Assert.NotNull(retrievedProduct.MasterData.Staged.MetaKeywords);
            Assert.Equal(metaKeywords["en"], retrievedProduct.MasterData.Staged.MetaKeywords["en"]);
        }

        [Fact]
        public void UpdateProductRevertStagedChanges()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(publish:true);
            var oldDescription = product.MasterData.Staged.Description;
            var newDescription = new LocalizedString() {{"en", TestingUtility.RandomString(20)}};
            Assert.NotEqual(newDescription["en"], oldDescription["en"]);

            SetDescriptionUpdateAction setDescriptionUpdateAction = new SetDescriptionUpdateAction
            {
                Description = newDescription,
                Staged = true
            };

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {setDescriptionUpdateAction};
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            // make sure that current description like the old description and only the staged with the new description
            Assert.Equal(newDescription["en"], retrievedProduct.MasterData.Staged.Description["en"]);
            Assert.Equal(oldDescription["en"], retrievedProduct.MasterData.Current.Description["en"]);

            //Create RevertAction and Update the Product to revert changes
            RevertStagedChangesUpdateAction revertStagedChangesUpdateAction = new RevertStagedChangesUpdateAction();
            updateActions.Clear();
            updateActions.Add(revertStagedChangesUpdateAction);

            Product revertedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(retrievedProduct.Id), retrievedProduct.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(revertedProduct);
            //make sure that current and staged has the old description
            Assert.Equal(oldDescription["en"], revertedProduct.MasterData.Current.Description["en"]);
            Assert.Equal(oldDescription["en"], revertedProduct.MasterData.Staged.Description["en"]);
        }

        [Fact]
        public void UpdateProductRevertStagedVariantChanges()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct(publish:true);

            var oldVariantSku = product.MasterData.Staged.MasterVariant.Sku;
            var newVariantSku = TestingUtility.RandomString(20);
            Assert.NotEqual(newVariantSku, oldVariantSku);

            var masterVariantId = product.MasterData.Staged.MasterVariant.Id;
            SetSkuUpdateAction setSkuUpdateAction = new SetSkuUpdateAction()
            {
                Sku = newVariantSku,
                VariantId = masterVariantId,
                Staged = true
            };

            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {setSkuUpdateAction};
            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            // make sure that current variant sku like the old sku and only the staged with the new sku
            Assert.Equal(newVariantSku, retrievedProduct.MasterData.Staged.MasterVariant.Sku);
            Assert.Equal(oldVariantSku, retrievedProduct.MasterData.Current.MasterVariant.Sku);

            //Create RevertVariantAction and Update the Product to revert changes
            RevertStagedVariantChangesUpdateAction revertStagedChangesUpdateAction = new RevertStagedVariantChangesUpdateAction
            {
                VariantId = masterVariantId
            };
            updateActions.Clear();
            updateActions.Add(revertStagedChangesUpdateAction);

            Product revertedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(retrievedProduct.Id), retrievedProduct.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(revertedProduct);
            //make sure that current and staged variant has the old sku
            Assert.Equal(oldVariantSku, revertedProduct.MasterData.Current.MasterVariant.Sku);
            Assert.Equal(oldVariantSku, revertedProduct.MasterData.Staged.MasterVariant.Sku);
        }

        [Fact]
        public void UpdateProductTransitionToNewState()
        {
            IClient commerceToolsClient = this.productFixture.GetService<IClient>();
            Product product = this.productFixture.CreateProduct();

            Assert.Null(product.State);

            var initialProductState = this.productFixture.CreateNewState(StateType.ProductState, initial: true);

            Assert.NotNull(initialProductState.Id);

            TransitionStateUpdateAction transitionStateUpdateAction = new TransitionStateUpdateAction()
            {
               State = new Reference<State>{ Id = initialProductState.Id}
            };
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>> {transitionStateUpdateAction};

            Product retrievedProduct = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version, updateActions))
                .Result;

            this.productFixture.ProductsToDelete.Add(retrievedProduct);

            Assert.NotNull(retrievedProduct.State);
            Assert.Equal(initialProductState.Id, retrievedProduct.State.Id);
        }
        #endregion
    }
}
