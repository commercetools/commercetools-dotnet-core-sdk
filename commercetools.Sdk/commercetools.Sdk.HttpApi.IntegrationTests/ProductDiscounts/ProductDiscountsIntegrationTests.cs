using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.ProductDiscounts.UpdateActions;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.Domain;
using Xunit;
using Xunit.Abstractions;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.ProductDiscounts.UpdateActions.SetDescriptionUpdateAction;


namespace commercetools.Sdk.HttpApi.IntegrationTests.ProductDiscounts
{
    [Collection("Integration Tests")]
    public class ProductDiscountsIntegrationTests : IClassFixture<ProductDiscountsFixture>
    {
        private readonly ProductDiscountsFixture productDiscountFixture;

        public ProductDiscountsIntegrationTests(ProductDiscountsFixture productDiscountsFixture)
        {
            this.productDiscountFixture = productDiscountsFixture;
        }

        [Fact]
        public void CreateProductDiscount()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            
            //Create Product
            Product product = this.productDiscountFixture.CreateProductWithVariant();
            
            //Then Create Product Discount for this Product
            ProductDiscountDraft productDiscountDraft = this.productDiscountFixture.GetProductDiscountDraft(product.Id);
            ProductDiscount productDiscount = commerceToolsClient
                .ExecuteAsync(new CreateCommand<ProductDiscount>(productDiscountDraft)).Result;
            this.productDiscountFixture.ProductDiscountsToDelete.Add(productDiscount);
            Assert.Equal(productDiscountDraft.Name["en"], productDiscount.Name["en"]);
        }


        [Fact]
        public void GetProductDiscountById()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            this.productDiscountFixture.ProductDiscountsToDelete.Add(productDiscount);
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<ProductDiscount>(new Guid(productDiscount.Id))).Result;
            Assert.Equal(productDiscount.Id, retrievedProductDiscount.Id);
        }

        [Fact]
        public void QueryProductDiscount()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            this.productDiscountFixture.ProductDiscountsToDelete.Add(productDiscount);
            string id = productDiscount.Id;
            QueryPredicate<ProductDiscount> queryPredicate = new QueryPredicate<ProductDiscount>(pd => pd.Id == id);
            QueryCommand<ProductDiscount> queryCommand = new QueryCommand<ProductDiscount>();
            queryCommand.SetWhere(queryPredicate);
            PagedQueryResult<ProductDiscount> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, pd => pd.Id == productDiscount.Id);
        }

        [Fact]
        public void DeleteProductDiscountById()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(
                    new DeleteByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version))
                .Result;
            Assert.ThrowsAsync<HttpApiClientException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<ProductDiscount>(new Guid(retrievedProductDiscount.Id))));
        }
        
        [Fact]
        public void GetMatchingProductDiscountValidQuery()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            
            //Create Product with product variant
            Product product = this.productDiscountFixture.CreateProductWithVariant();
            
            //Create Product Discount for this product
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount(product);
            this.productDiscountFixture.ProductDiscountsToDelete.Add(productDiscount);
            
            var masterVariant = product.MasterData.Staged.MasterVariant;
            
            Assert.NotEmpty(masterVariant.Prices);
            
            var masterVariantPrice = masterVariant.Prices[0];
            
            var matchingProductDiscountParams= new GetMatchingProductDiscountParameters()
            {
                Staged = true,
                Price = masterVariantPrice,
                ProductId = new Guid(product.Id),
                VariantId = masterVariant.Id
            };
            ProductDiscount matchingProductDiscount = commerceToolsClient
                .ExecuteAsync(new GetMatchingProductDiscountCommand(matchingProductDiscountParams))
                .Result;
            Assert.NotNull(matchingProductDiscount);
        }

        
        #region UpdateActions
        
        [Fact]
        public void UpdateProductDiscountByIdChangeIsActive()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            
            List<UpdateAction<ProductDiscount>> updateActions = new List<UpdateAction<ProductDiscount>>();
            ChangeIsActiveUpdateAction changeIsActiveUpdateAction = new ChangeIsActiveUpdateAction()
            {
                IsActive = !productDiscount.IsActive
            };
            updateActions.Add(changeIsActiveUpdateAction);
            
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version, updateActions))
                .Result;
                        
            this.productDiscountFixture.ProductDiscountsToDelete.Add(retrievedProductDiscount);
            Assert.NotEqual(retrievedProductDiscount.IsActive, productDiscount.IsActive);
        }

        [Fact]
        public void UpdateProductDiscountByIdChangeValue()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            
            //creating new product discount value
            var newProductDiscountValue = this.productDiscountFixture.GetProductDiscountValueAsAbsolute();
            
            List<UpdateAction<ProductDiscount>> updateActions = new List<UpdateAction<ProductDiscount>>();
            ChangeValueUpdateAction changeValueUpdateAction = new ChangeValueUpdateAction()
                {Value = newProductDiscountValue};
            updateActions.Add(changeValueUpdateAction);
            
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version, updateActions))
                .Result;
                        
            this.productDiscountFixture.ProductDiscountsToDelete.Add(retrievedProductDiscount);
            Assert.NotEqual(productDiscount.Version, retrievedProductDiscount.Version);
        }
        
        [Fact]
        public void UpdateProductDiscountByIdChangePredicate()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();

            Product product = this.productDiscountFixture.CreateProductWithVariant();
            
            //creating new predicate based on product "product.id = "ae3630c9-365a-4abe-a9b3-c14bb8b9ce95" "
            string predicate = this.productDiscountFixture.GetProductDiscountPredicateBasedonProduct(product.Id); 
            
            List<UpdateAction<ProductDiscount>> updateActions = new List<UpdateAction<ProductDiscount>>();
            ChangePredicateUpdateAction changePredicateUpdateAction = new ChangePredicateUpdateAction()
                {Predicate = predicate};
            updateActions.Add(changePredicateUpdateAction);
            
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version, updateActions))
                .Result;
                        
            this.productDiscountFixture.ProductDiscountsToDelete.Add(retrievedProductDiscount);
            Assert.NotEqual(productDiscount.Predicate, retrievedProductDiscount.Predicate);
        }
        
        [Fact]
        public void UpdateProductDiscountByIdSetValidFrom()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            
            List<UpdateAction<ProductDiscount>> updateActions = new List<UpdateAction<ProductDiscount>>();
            SetValidFromUpdateAction setValidFromUpdateAction = new SetValidFromUpdateAction()
            {
                ValidFrom = DateTime.Today
            };
            updateActions.Add(setValidFromUpdateAction);
            
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version, updateActions))
                .Result;
                        
            this.productDiscountFixture.ProductDiscountsToDelete.Add(retrievedProductDiscount);
            Assert.NotEqual(retrievedProductDiscount.ValidFrom, productDiscount.ValidFrom);
        }
        [Fact]
        public void UpdateProductDiscountByIdSetValidUntil()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            
            List<UpdateAction<ProductDiscount>> updateActions = new List<UpdateAction<ProductDiscount>>();
            SetValidUntilUpdateAction setValidUntilUpdateAction = new SetValidUntilUpdateAction()
            {
                ValidUntil = DateTime.Today
            };
            updateActions.Add(setValidUntilUpdateAction);
            
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version, updateActions))
                .Result;
                        
            this.productDiscountFixture.ProductDiscountsToDelete.Add(retrievedProductDiscount);
            Assert.NotEqual(retrievedProductDiscount.ValidUntil, productDiscount.ValidUntil);
        }
        [Fact]
        public void UpdateProductDiscountByIdSetValidFromAndUntil()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            
            List<UpdateAction<ProductDiscount>> updateActions = new List<UpdateAction<ProductDiscount>>();
            SetValidFromAndUntilUpdateAction setValidUntilUpdateAction = new SetValidFromAndUntilUpdateAction()
            {
                ValidFrom = productDiscount.ValidFrom.AddDays(2),
                ValidUntil = productDiscount.ValidUntil.AddDays(2)
            };
            updateActions.Add(setValidUntilUpdateAction);
            
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version, updateActions))
                .Result;
                        
            this.productDiscountFixture.ProductDiscountsToDelete.Add(retrievedProductDiscount);
            
            Assert.NotEqual(retrievedProductDiscount.ValidFrom, productDiscount.ValidFrom);
            Assert.NotEqual(retrievedProductDiscount.ValidUntil, productDiscount.ValidUntil);
        }
        
        [Fact]
        public void UpdateProductDiscountByIdChangeName()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();

            string name = this.productDiscountFixture.RandomString(4);
            List<UpdateAction<ProductDiscount>> updateActions = new List<UpdateAction<ProductDiscount>>();
            ChangeNameUpdateAction changeNameUpdateAction = new ChangeNameUpdateAction()
            {
                Name = new LocalizedString() { { "en", name } }
            };
            updateActions.Add(changeNameUpdateAction);
            
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version, updateActions))
                .Result;
                        
            this.productDiscountFixture.ProductDiscountsToDelete.Add(retrievedProductDiscount);
            Assert.Equal(name, retrievedProductDiscount.Name["en"]);
        }
        
        [Fact]
        public void UpdateProductDiscountByIdSetDescription()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();

            string newDescription = this.productDiscountFixture.RandomString(20);
            List<UpdateAction<ProductDiscount>> updateActions = new List<UpdateAction<ProductDiscount>>();
            SetDescriptionUpdateAction setDescriptionUpdateAction = new SetDescriptionUpdateAction()
            {
                Description = new LocalizedString() { { "en", newDescription } }
            };
            updateActions.Add(setDescriptionUpdateAction);
            
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version, updateActions))
                .Result;
                        
            this.productDiscountFixture.ProductDiscountsToDelete.Add(retrievedProductDiscount);
            Assert.Equal(newDescription, retrievedProductDiscount.Description["en"]);
        }
        
        [Fact]
        public void UpdateProductDiscountByIdChangeSortOrder()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            
            List<UpdateAction<ProductDiscount>> updateActions = new List<UpdateAction<ProductDiscount>>();
            ChangeSortOrderUpdateAction changeSortOrderUpdateAction = new ChangeSortOrderUpdateAction()
            {
                SortOrder = this.productDiscountFixture.RandomSortOrder()
            };
            updateActions.Add(changeSortOrderUpdateAction);
            
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version, updateActions))
                .Result;
                        
            this.productDiscountFixture.ProductDiscountsToDelete.Add(retrievedProductDiscount);
            Assert.NotEqual(retrievedProductDiscount.SortOrder, productDiscount.SortOrder);
        }

        #endregion
    }
}