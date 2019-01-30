using System;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.Domain;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ProductDiscounts
{
    [Collection("Integration Tests")]
    public class ProductDiscountsIntegrationTests  : IClassFixture<ProductDiscountsFixture>
    {
        private readonly ProductDiscountsFixture productDiscountFixture;

        public ProductDiscountsIntegrationTests(ProductDiscountsFixture productDiscountsFixture)
        {
            this.productDiscountFixture = productDiscountsFixture;
        }
        
        [Fact(Skip = "Temp Skip")]
        public void CreateProductDiscount()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscountDraft productDiscountDraft = this.productDiscountFixture.GetProductDiscountDraft();
            ProductDiscount productDiscount = commerceToolsClient.ExecuteAsync(new CreateCommand<ProductDiscount>(productDiscountDraft)).Result;
            this.productDiscountFixture.ProductDiscountsToDelete.Add(productDiscount);
            Assert.Equal(productDiscountDraft.Name["en"], productDiscount.Name["en"]);
        }
        
        [Fact(Skip = "Temp Skip")]
        public void GetProductDiscountById()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            this.productDiscountFixture.ProductDiscountsToDelete.Add(productDiscount);
            ProductDiscount retrievedProductDiscount = commerceToolsClient.ExecuteAsync(new GetByIdCommand<ProductDiscount>(new Guid(productDiscount.Id))).Result;
            Assert.Equal(productDiscount.Id, retrievedProductDiscount.Id);
        }
        
        [Fact(Skip = "Temp Skip")]
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
        [Fact(Skip = "Temp Skip")]
        public void DeleteProductDiscountById()
        {
            IClient commerceToolsClient = this.productDiscountFixture.GetService<IClient>();
            ProductDiscount productDiscount = this.productDiscountFixture.CreateProductDiscount();
            ProductDiscount retrievedProductDiscount = commerceToolsClient
                .ExecuteAsync(new DeleteByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version)).Result;
            Assert.ThrowsAsync<HttpApiClientException>(() =>
                commerceToolsClient.ExecuteAsync(new GetByIdCommand<ProductDiscount>(new Guid(retrievedProductDiscount.Id))));
        }
    }
}