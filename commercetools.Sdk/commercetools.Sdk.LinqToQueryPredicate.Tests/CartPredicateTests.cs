using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Linq.Extensions.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace commercetools.Sdk.Linq.Tests
{
    public class CartPredicateTests
    {
        [Fact]
        public void LineItemCountByProductIdGreaterThanTen()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(l => l.ProductId == "45224437-12bd-4742-830c-3a36b52541d3") > 10;
            var result = string.Empty;
            Assert.Equal("lineItemCount(product.id = \"45224437-12bd-4742-830c-3a36b52541d3\") > 10", result);
        }

        [Fact]
        public void LineItemCountTrueGreaterThanTen()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(true) > 10;
            var result = string.Empty;
            Assert.Equal("lineItemCount(true) > 10", result);
        }

        [Fact]
        public void LineItemTotal()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(true) > 10;
            var result = string.Empty;
            Assert.Equal("lineItemCount(product.id = \"45224437-12bd-4742-830c-3a36b52541d3\") > 10", result);
        }

        [Fact]
        public void CartPredicateCustomTypeId()
        {
            Expression<Func<Cart, bool>> expression = c => c.Custom.Type.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7";
            var result = string.Empty;
            Assert.Equal("custom.type.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

        [Fact]
        public void CartPredicateCurrency()
        {
            Expression<Func<Cart, bool>> expression = c => c.Currency() == "EUR";
            var result = string.Empty;
            Assert.Equal("currency = \"EUR\"", result);
        }

        [Fact]
        public void CartPredicateCustomerFirstName()
        {
            Expression<Func<Cart, bool>> expression = c => c.Customer().FirstName == "John";
            var result = string.Empty;
            Assert.Equal("customer.firstName = \"John\"", result);
        }

        [Fact]
        public void CartPredicateCustomerGroupKey()
        {
            Expression<Func<Cart, bool>> expression = c => c.CustomerGroupKey() == "key12";
            var result = string.Empty;
            Assert.Equal("customer.customerGroup.key = \"key12\"", result);
        }

        [Fact]
        public void CartPredicateCustomTypeKey()
        {
            Expression<Func<Cart, bool>> expression = c => c.CustomTypeKey() == "my-category";
            var result = string.Empty;
            Assert.Equal("custom.type.key = \"my-category\"", result);
        }

        [Fact]
        public void CartPredicateTotalPrice()
        {
            Expression<Func<Cart, bool>> expression = c => c.TotalPrice.Equal("10 USD");
            var result = string.Empty;
            Assert.Equal("totalPrice = \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateTotalNet()
        {
            Expression<Func<Cart, bool>> expression = c => c.TaxedPrice.TotalNet.GreaterThan("10 USD");
            var result = string.Empty;
            Assert.Equal("taxedPrice.net > \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateTotalGross()
        {
            Expression<Func<Cart, bool>> expression = c => c.TaxedPrice.TotalGross.LessThanOrEqual("10 USD");
            var result = string.Empty;
            Assert.Equal("taxedPrice.gross <= \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateCustomerId()
        {
            Expression<Func<Cart, bool>> expression = c => c.CustomerId == "45224437-12bd-4742-830c-3a36b52541d3";
            Expression<Func<Cart, bool>> expressionSimilar = c => c.Customer().Id == "45224437-12bd-4742-830c-3a36b52541d3";
            var result = string.Empty;
            Assert.Equal("customer.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void CartPredicateCustomerEmail()
        {
            Expression<Func<Cart, bool>> expression = c => c.CustomerEmail == "email@domain.com";
            Expression<Func<Cart, bool>> expressionSimilar = c => c.Customer().Email == "email@domain.com";
            var result = string.Empty;
            Assert.Equal("customer.email = \"email@domain.com\"", result);
        }

        [Fact]
        public void CartPredicateCustomerGroupId()
        {
            Expression<Func<Cart, bool>> expression = c => c.CustomerGroup.Id == "45224437-12bd-4742-830c-3a36b52541d3";
            Expression<Func<Cart, bool>> expressionSimilar = c => c.Customer().CustomerGroup.Id == "45224437-12bd-4742-830c-3a36b52541d3";
            var result = string.Empty;
            Assert.Equal("customer.customerGroup.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateProductId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductId == "45224437-12bd-4742-830c-3a36b52541d3";
            var result = string.Empty;
            Assert.Equal("product.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateProductTypeId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductType.Id == "45224437-12bd-4742-830c-3a36b52541d3";
            var result = string.Empty;
            Assert.Equal("productType.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateProductKey()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductKey() == "key123";
            var result = string.Empty;
            Assert.Equal("product.key = \"key123\"", result);
        }

        [Fact]
        public void LineItemPredicateVariantId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Variant.Id == 123;
            var result = string.Empty;
            Assert.Equal("variantId = 123", result);
        }

        [Fact]
        public void LineItemPredicateCatalogId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.CatalogId() == "45224437-12bd-4742-830c-3a36b52541d3";
            var result = string.Empty;
            Assert.Equal("catalog.id =  \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateMoneyAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "money" && ((MoneyAttribute)a).Value.GreaterThan("18.00 EUR"));
            var result = string.Empty;
            Assert.Equal("attributes.money > \"18.00 EUR\"", result);
        }

        [Fact]
        public void LineItemPredicatePrice()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.Value.Equal("10.00 EUR");
            var result = string.Empty;
            Assert.Equal("price = \"10.00 EUR\"", result);
        }

        [Fact]
        public void LineItemPredicatePriceCentAmount()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.Value.CentAmount == 1000;
            var result = string.Empty;
            Assert.Equal("price.centAmount = 1000", result);
        }

        [Fact]
        public void LineItemPredicatePriceDiscountId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.DiscountId() == "45224437-12bd-4742-830c-3a36b52541d3";
            var result = string.Empty;
            Assert.Equal("price.discount.id =  \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicatePriceCustomerGroupKey()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.CustomerGroupKey() == "key123";
            var result = string.Empty;
            Assert.Equal("price.customerGroup.key = \"key123\"", result);
        }

        // TODO custom.<field> 


    }
}
