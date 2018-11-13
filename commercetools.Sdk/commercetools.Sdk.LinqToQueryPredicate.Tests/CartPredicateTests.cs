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
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemCount(product.id = \"45224437-12bd-4742-830c-3a36b52541d3\") > 10", result);
        }

        [Fact]
        public void LineItemCountTrueGreaterThanTen()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(true) > 10;
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemCount(true) > 10", result);
        }

        [Fact]
        public void LineItemTotal()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(true) > 10;
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemCount(true) > 10", result);
        }

        [Fact]
        public void CartPredicateCustomTypeId()
        {
            Expression<Func<Cart, bool>> expression = c => c.Custom.Type.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.type.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

        [Fact]
        public void CartPredicateCurrency()
        {
            Expression<Func<Cart, bool>> expression = c => c.Currency() == "EUR";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("currency = \"EUR\"", result);
        }

        [Fact]
        public void CartPredicateCustomerFirstName()
        {
            Expression<Func<Cart, bool>> expression = c => c.Customer().FirstName == "John";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.firstName = \"John\"", result);
        }

        [Fact]
        public void CartPredicateCustomerGroupKey()
        {
            Expression<Func<Cart, bool>> expression = c => c.Customer().CustomerGroupKey() == "key12";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.customerGroup.key = \"key12\"", result);
        }

        [Fact]
        public void CartPredicateCustomTypeKey()
        {
            Expression<Func<Cart, bool>> expression = c => c.CustomTypeKey() == "my-category";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.type.key = \"my-category\"", result);
        }

        [Fact]
        public void CartPredicateTotalPrice()
        {
            Expression<Func<Cart, bool>> expression = c => c.TotalPrice == Money.Parse("10 USD");
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("totalPrice = \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateTotalNet()
        {
            Expression<Func<Cart, bool>> expression = c => c.TaxedPrice.TotalNet > Money.Parse("10 USD");
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("taxedPrice.net > \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateTotalGross()
        {
            Expression<Func<Cart, bool>> expression = c => c.TaxedPrice.TotalGross <= Money.Parse("10 USD");
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("taxedPrice.gross <= \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateCustomerId()
        {
            Expression<Func<Cart, bool>> expression = c => c.CustomerId == "45224437-12bd-4742-830c-3a36b52541d3";
            Expression<Func<Cart, bool>> expressionSimilar = c => c.Customer().Id == "45224437-12bd-4742-830c-3a36b52541d3";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void CartPredicateCustomerEmail()
        {
            //Expression<Func<Cart, bool>> expression = c => c.CustomerEmail == "email@domain.com";
            Expression<Func<Cart, bool>> expression = c => c.Customer().Email == "email@domain.com";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.email = \"email@domain.com\"", result);
        }

        [Fact]
        public void CartPredicateCustomerGroupId()
        {
            //Expression<Func<Cart, bool>> expression = c => c.CustomerGroup.Id == "45224437-12bd-4742-830c-3a36b52541d3";
            Expression<Func<Cart, bool>> expression = c => c.Customer().CustomerGroup.Id == "45224437-12bd-4742-830c-3a36b52541d3";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.customerGroup.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateProductId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductId == "45224437-12bd-4742-830c-3a36b52541d3";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("product.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateProductTypeId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductType.Id == "45224437-12bd-4742-830c-3a36b52541d3";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("productType.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateProductKey()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductKey() == "key123";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("product.key = \"key123\"", result);
        }

        [Fact]
        public void LineItemPredicateVariantId()
        {
            //Expression<Func<LineItem, bool>> expression = l => l.Variant.Id == 123;
            Expression<Func<LineItem, bool>> expression = l => l.VariantId() == 123;
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variantId = 123", result);
        }

        [Fact]
        public void LineItemPredicateCatalogId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.CatalogId() == "45224437-12bd-4742-830c-3a36b52541d3";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("catalog.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateMoneyAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "money" && ((MoneyAttribute)a).Value >= Money.Parse("18.00 EUR"));
            // TODO Create converters for attributes
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.money >= \"18.00 EUR\"", result);
        }

        [Fact]
        public void LineItemPredicateMoneyCentAmountAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "money" && ((MoneyAttribute)a).Value.CentAmount == 1800);
            // TODO Create converters for attributes
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.money.centAmount = 1800", result);
        }

        [Fact]
        public void LineItemPredicateTextAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "color" && ((TextAttribute)a).Value == "green");
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.color = \"green\"", result);
        }

        [Fact]
        public void LineItemPredicateEnumAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "color" && ((EnumAttribute)a).Value.Key == "green");
            // TODO Create converters for attributes
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.color = \"green\"", result);
        }

        [Fact]
        public void LineItemPredicateTextSetAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "colors" && ((SetTextAttribute)a).Value.IsEmpty());
            // TODO Create converters for attributes
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.colors is empty", result);
        }

        [Fact]
        public void CartPredicateCountryIsDefined()
        {
            Expression<Func<Cart, bool>> expression = c => c.Country.IsDefined();
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("country is defined", result);
        }

        [Fact]
        public void LineItemPredicateTextSetContainsAnyAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "colors" && ((SetTextAttribute)a).Value.ContainsAny("green", "blue"));
            // TODO Create converters for attributes
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.colors contains any (\"green\", \"blue\")", result);
        }

        [Fact]
        public void LineItemPredicateEnumSetContainsAnyAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "colors" && ((SetEnumAttribute)a).Value.Select(e => e.Value.Key).ContainsAny("green", "blue"));
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.colors contains any (\"green\", \"blue\")", result);
        }

        [Fact]
        public void LineItemPredicatePrice()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.Value == Money.Parse("10.00 EUR");
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("price = \"10.00 EUR\"", result);
        }

        [Fact]
        public void LineItemPredicatePriceCentAmount()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.Value.CentAmount == 1000;
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("price.centAmount = 1000", result);
        }

        [Fact]
        public void LineItemPredicatePriceDiscountId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.DiscountId() == "45224437-12bd-4742-830c-3a36b52541d3";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("price.discount.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicatePriceCustomerGroupKey()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.CustomerGroupKey() == "key123";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("price.customerGroup.key = \"key123\"", result);
        }

        [Fact]
        public void LineItemPredicateSkuAndTaxRate()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Sku() == "SKU-123" && l.TaxRate.IncludedInPrice == false;
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("sku = \"SKU-123\" and taxRate.includedInPrice = false", result);
        }

        [Fact]
        public void LineItemPredicateGrouping()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductType.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" && l.Attributes().Any(a => a.Name == "rating" && ((NumberAttribute)a).Value > 3) && (l.ProductId == "abcd9a23-14e3-40d0-aee2-3e612fcbefgh" || l.ProductId == "ba3e4ee7-30fa-400b-8155-46ebf423d793");
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("productType.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\" and attributes.rating > 3 and (product.id = \"abcd9a23-14e3-40d0-aee2-3e612fcbefgh\" or product.id = \"ba3e4ee7-30fa-400b-8155-46ebf423d793\")", result);
        }

        [Fact]
        public void LineItemPredicateCategoriesIdNotEqual()
        {
            Expression<Func<LineItem, bool>> expression = l => l.CategoriesId() != "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id != \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

        [Fact]
        public void CustomLineItemPredicateMoneyAndTaxRate()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Money > Money.Parse("10.50 EUR") && c.TaxRate.IncludedInPrice == false;
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("money > \"10.50 EUR\" and taxRate.includedInPrice = false", result);
        }

        [Fact]
        public void CustomLineItemPredicateSlug()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Slug == "adidas";
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = TestUtils.CreateCartPredicateExpressionVisitor();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("slug = \"adidas\"", result);
        }

        // TODO custom.<field> 

        // TODO not()


    }
}
