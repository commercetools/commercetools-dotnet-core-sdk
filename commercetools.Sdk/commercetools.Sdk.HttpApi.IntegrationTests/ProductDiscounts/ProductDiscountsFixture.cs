using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.HttpApi.IntegrationTests;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ProductDiscounts
{
    public class ProductDiscountsFixture: ClientFixture, IDisposable
    {
        private readonly ProductTypeFixture productTypeFixture;
        public List<ProductDiscount> ProductDiscountsToDelete { get; }
        
        public ProductDiscountsFixture()
        {
            this.ProductDiscountsToDelete = new List<ProductDiscount>();
            this.productTypeFixture = new ProductTypeFixture();
        }
        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ProductDiscountsToDelete.Reverse();
            foreach (ProductDiscount productDiscount in this.ProductDiscountsToDelete)
            {
                ProductDiscount deletedType = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<ProductDiscount>(new Guid(productDiscount.Id), productDiscount.Version)).Result;
            }
            this.productTypeFixture.Dispose();
        }
        public ProductDiscountDraft GetProductDiscountDraft()
        {
            var random = new Random();
            ProductType productType = this.productTypeFixture.CreateProductType();
            this.productTypeFixture.ProductTypesToDelete.Add(productType);
            string predicate = $"productType.id = \"{productType.Id}\"";
            
            ProductDiscountDraft productDiscountDraft = new ProductDiscountDraft();
            productDiscountDraft.Name = new LocalizedString() {{"en", this.RandomString(4)}};
            productDiscountDraft.Value = GetProductDiscountValue();
            productDiscountDraft.Predicate = predicate;
            productDiscountDraft.SortOrder = "0.5";
            productDiscountDraft.ValidFrom = DateTime.Today.AddMonths(random.Next(-5,-1));
            productDiscountDraft.ValidUntil = DateTime.Today.AddMonths(random.Next(1,5));
            
            return productDiscountDraft;
        }
        public ProductDiscount CreateProductDiscount()
        {
            return this.CreateProductDiscount(this.GetProductDiscountDraft());
        }

        public ProductDiscount CreateProductDiscount(ProductDiscountDraft productDiscountDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            ProductDiscount productDiscount = commerceToolsClient.ExecuteAsync(new CreateCommand<ProductDiscount>(productDiscountDraft)).Result;
            return productDiscount;
        }

        /// <summary>
        /// Return Relative Product Discount
        /// </summary>
        /// <returns></returns>
        public ProductDiscountValue GetProductDiscountValue()
        {
            var random = new Random();
            var productDiscountValue = new RelativeProductDiscountValue()
            {
                Permyriad = random.Next(1,10)
            };
            return productDiscountValue;
        }
    }
}