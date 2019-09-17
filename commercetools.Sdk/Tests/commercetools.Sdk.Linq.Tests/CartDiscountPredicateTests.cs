using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Linq.Discount;
using Xunit;

namespace commercetools.Sdk.Linq.Tests
{
    public class CartDiscountPredicateTests : IClassFixture<LinqFixture>
    {
        private readonly LinqFixture linqFixture;

        public CartDiscountPredicateTests(LinqFixture linqFixture)
        {
            this.linqFixture = linqFixture;
        }

        [Fact]
        public void LineItemCountByProductIdGreaterThanTen()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(l => l.ProductId == "45224437-12bd-4742-830c-3a36b52541d3") > 10;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemCount(product.id = \"45224437-12bd-4742-830c-3a36b52541d3\") > 10", result);
        }

        [Fact]
        public void LineItemCountByProductIdGreaterThanTenVar()
        {
            string id = "45224437-12bd-4742-830c-3a36b52541d3";
            int count = 10;
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(l => l.ProductId == id) > count;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemCount(product.id = \"45224437-12bd-4742-830c-3a36b52541d3\") > 10", result);
        }

        [Fact]
        public void LineItemCountTrueGreaterThanTen()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(true) > 10;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemCount(true) > 10", result);
        }

        [Fact]
        public void LineItemCountWithPredicate()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(l => l.Attributes().Any(a => a.Name == "size" && a.ToTextAttribute().Value.In("xxl", "xl"))) == 2;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemCount(attributes.size in (\"xxl\", \"xl\")) = 2", result);
        }
        
        [Fact]
        public void LineItemTotalWithDecimalPoints()
        {
            var money = Money.FromDecimal("USD", 10.67m);
            Expression<Func<Cart, bool>> expression = c => c.LineItemTotal(true) > money.moneyString();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemTotal(true) > \"10.67 USD\"", result);
        }
        
        [Fact]
        public void LineItemTotal()
        {
            var money = Money.FromDecimal("USD", 10m);
            Expression<Func<Cart, bool>> expression = c => c.LineItemTotal(true) > money.moneyString();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemTotal(true) > \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateCustomTypeId()
        {
            Expression<Func<Cart, bool>> expression = c => c.Custom.Type.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.type.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

        [Fact]
        public void CartPredicateCurrency()
        {
            Expression<Func<Cart, bool>> expression = c => c.Currency() == "EUR";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("currency = \"EUR\"", result);
        }

        [Fact]
        public void CartPredicateCustomerFirstName()
        {
            Expression<Func<Cart, bool>> expression = c => c.Customer().FirstName == "John";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.firstName = \"John\"", result);
        }

        [Fact]
        public void CartPredicateCustomerGroupKey()
        {
            Expression<Func<Cart, bool>> expression = c => c.Customer().CustomerGroupKey() == "key12";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.customerGroup.key = \"key12\"", result);
        }

        [Fact]
        public void CartPredicateCustomTypeKey()
        {
            Expression<Func<Cart, bool>> expression = c => c.CustomTypeKey() == "my-category";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.type.key = \"my-category\"", result);
        }

        [Fact]
        public void CartPredicateTotalPrice()
        {
            var money = Money.FromDecimal("USD", 10);
            Expression<Func<Cart, bool>> expression = c => c.TotalPrice == money.moneyString();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("totalPrice = \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateTotalPriceVar()
        {
            Expression<Func<Cart, bool>> expression = c => c.TotalPrice == Money.FromDecimal("USD", 10, MidpointRounding.ToEven).moneyString();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("totalPrice = \"10 USD\"", result);
        }
        
        [Fact]
        public void CartPredicateMinimumTotalPrice()
        {
            var totalMoney = Money.FromDecimal("EUR", 800);
            var money2 = Money.FromDecimal("EUR", 10.50m);
            Expression<Func<Cart, bool>> expression = c => c.TotalPrice > totalMoney.moneyString() 
                                                           && c.LineItemCount( l => l.Price.Value > money2.moneyString() && l.ProductType.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" && l.Attributes().Any(a => a.Name == "size" && a.ToTextAttribute().Value.In("xl", "xxl")) || l.ProductId == "abcd9a23-14e3-40d0-aee2-3e612fcbefgh") > 0;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("totalPrice > \"800 EUR\" and lineItemCount(price > \"10.5 EUR\" and productType.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\" and attributes.size in (\"xl\", \"xxl\") or product.id = \"abcd9a23-14e3-40d0-aee2-3e612fcbefgh\") > 0", result);
        }
        
        [Fact]
        public void CartPredicateCustomDate()
        {
            var startDate = DateTime.Parse("2019-09-11T15:00:00.000+02:00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToLocalTime();
            Expression<Func<Cart, bool>> expression = c => c.Custom.Fields["bookingStart"] == startDate.AsDate() && c.Custom.Fields["bookingEnd"] == DateTime.Parse("2019-10-11", CultureInfo.InvariantCulture).AsDate();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.bookingStart = \"2019-09-11\" and custom.bookingEnd = \"2019-10-11\"", result);
        }
        
        [Fact]
        public void CartPredicateCustomDateTime()
        {
            var startDate = DateTime.Parse("2019-09-11T15:00:00.000+02:00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToLocalTime();
            Assert.Equal(DateTimeKind.Local, startDate.Kind);//local time
            
            Expression<Func<Cart, bool>> expression = c => c.Custom.Fields["bookingStart"] == startDate.AsDateTime() && c.Custom.Fields["bookingEnd"] == DateTime.Parse("2019-10-11T15:00:00.000+02:00", CultureInfo.GetCultureInfo("de-DE"), DateTimeStyles.AdjustToUniversal).AsDateTime();//utc time
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.bookingStart = \"2019-09-11T13:00:00.000Z\" and custom.bookingEnd = \"2019-10-11T13:00:00.000Z\"", result);
        }
        
        [Fact]
        public void CartPredicateCustomTime()
        {
            var startTime = new TimeSpan(0,13, 22, 12, 123 );
            
            Expression<Func<Cart, bool>> expression = c => c.Custom.Fields["bookingStart"] == startTime.AsTime() && c.Custom.Fields["bookingEnd"] == DateTime.Parse("2019-10-11T15:33:11.123+02:00", CultureInfo.GetCultureInfo("de-DE"), DateTimeStyles.AdjustToUniversal).TimeOfDay.AsTime();//utc time
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.bookingStart = \"13:22:12.123\" and custom.bookingEnd = \"13:33:11.123\"", result);
        }
        
        [Fact]
        public void CartPredicateCustomField()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(l=> l.Custom.Fields["age"].ToString() == "adult") >= 2 && c.LineItemCount(l=> l.Custom.Fields["age"].ToString() == "youth") >= 1 ;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("lineItemCount(custom.age = \"adult\") >= 2 and lineItemCount(custom.age = \"youth\") >= 1", result);
        }

        [Fact]
        public void CartPredicateTotalNet()
        {
            Expression<Func<Cart, bool>> expression = c => c.TaxedPrice.TotalNet > Money.FromDecimal("USD", 10, MidpointRounding.ToEven).moneyString();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("taxedPrice.net > \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateTotalGross()
        {
            Expression<Func<Cart, bool>> expression = c => c.TaxedPrice.TotalGross <= Money.FromDecimal("USD", 10, MidpointRounding.ToEven).moneyString();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("taxedPrice.gross <= \"10 USD\"", result);
        }

        [Fact]
        public void CartPredicateCustomerId()
        {
            Expression<Func<Cart, bool>> expression = c => c.CustomerId == "45224437-12bd-4742-830c-3a36b52541d3";
            Expression<Func<Cart, bool>> expressionSimilar = c => c.Customer().Id == "45224437-12bd-4742-830c-3a36b52541d3";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void CartPredicateCustomerIdNonLocal()
        {
            Customer customer = new Customer()
            {
                Id = "45224437-12bd-4742-830c-3a36b52541d3"
            };
            Expression<Func<Cart, bool>> expression = c => c.CustomerId == customer.Id.valueOf();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void CartPredicateCustomerEmail()
        {
            //Expression<Func<Cart, bool>> expression = c => c.CustomerEmail == "email@domain.com";
            Expression<Func<Cart, bool>> expression = c => c.Customer().Email == "email@domain.com";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.email = \"email@domain.com\"", result);
        }

        [Fact]
        public void CartPredicateCustomerGroupId()
        {
            //Expression<Func<Cart, bool>> expression = c => c.CustomerGroup.Id == "45224437-12bd-4742-830c-3a36b52541d3";
            Expression<Func<Cart, bool>> expression = c => c.Customer().CustomerGroup.Id == "45224437-12bd-4742-830c-3a36b52541d3";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.customerGroup.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }
        
        [Fact]
        public void CartPredicateCustomerInformation()
        {
            Expression<Func<Cart, bool>> expression = c => c.Customer().Email == "john@example.com" && c.Customer().CustomerGroup.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("customer.email = \"john@example.com\" and customer.customerGroup.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

        [Fact]
        public void LineItemPredicateProductId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductId == "45224437-12bd-4742-830c-3a36b52541d3";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("product.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateProductTypeId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductType.Id == "45224437-12bd-4742-830c-3a36b52541d3";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("productType.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateProductKey()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductKey() == "key123";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("product.key = \"key123\"", result);
        }

        [Fact]
        public void LineItemPredicateVariantId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.VariantId() == 123;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variant.id = 123", result);
        }

        [Fact]
        public void LineItemPredicateCatalogId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.CatalogId() == "45224437-12bd-4742-830c-3a36b52541d3";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("catalog.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicateMoneyAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "money" && ((MoneyAttribute)a).Value >= Money.FromDecimal("EUR", 18, MidpointRounding.ToEven).moneyString());
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.money >= \"18 EUR\"", result);
        }

        [Fact]
        public void LineItemPredicateMoneyAttributeVar()
        {
            string money = "money";
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == money && ((MoneyAttribute)a).Value >= Money.FromDecimal("EUR", 18, MidpointRounding.ToEven).moneyString());
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.money >= \"18 EUR\"", result);
        }

        [Fact]
        public void LineItemPredicateMoneyAttributeCast()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "money" && a.ToMoneyAttribute().Value >= Money.FromDecimal("EUR", 18, MidpointRounding.ToEven).moneyString());
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.money >= \"18 EUR\"", result);
        }

        [Fact]
        public void LineItemPredicateMoneyCentAmountAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "money" && ((MoneyAttribute)a).Value.CentAmount == 1800);
            // TODO Create converters for attributes
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.money.centAmount = 1800", result);
        }

        [Fact]
        public void LineItemPredicateTextAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "color" && ((TextAttribute)a).Value == "green");
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.color = \"green\"", result);
        }

        [Fact]
        public void LineItemPredicateTextAttributeCaseSensitive()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "Color" && ((TextAttribute)a).Value == "Green");
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.Color = \"Green\"", result);
        }

        [Fact]
        public void LineItemPredicateEnumAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "color" && ((EnumAttribute)a).Value.Key == "green");
            // TODO Create converters for attributes
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.color = \"green\"", result);
        }

        [Fact]
        public void LineItemPredicateTextSetAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "colors" && ((SetTextAttribute)a).Value.IsEmpty());
            // TODO Create converters for attributes
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.colors is empty", result);
        }

        [Fact]
        public void CartPredicateCountryIsDefined()
        {
            Expression<Func<Cart, bool>> expression = c => c.Country.IsDefined();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("country is defined", result);
        }

        [Fact]
        public void LineItemPredicateTextSetContainsAnyAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "colors" && ((SetTextAttribute)a).Value.ContainsAny("green", "blue"));
            // TODO Create converters for attributes
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.colors contains any (\"green\", \"blue\")", result);
        }

        [Fact]
        public void LineItemPredicateEnumSetContainsAnyAttribute()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Attributes().Any(a => a.Name == "colors" && ((SetEnumAttribute)a).Value.Select(e => e.Key).ContainsAny("green", "blue"));
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.colors contains any (\"green\", \"blue\")", result);
        }

        [Fact]
        public void LineItemPredicatePrice()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.Value == Money.FromDecimal("EUR", 10, MidpointRounding.ToEven).moneyString();
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("price = \"10 EUR\"", result);
        }

        [Fact]
        public void LineItemPredicatePriceCentAmount()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.Value.CentAmount == 1000;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("price.centAmount = 1000", result);
        }

        [Fact]
        public void LineItemPredicatePriceDiscountId()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.DiscountId() == "45224437-12bd-4742-830c-3a36b52541d3";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("price.discount.id = \"45224437-12bd-4742-830c-3a36b52541d3\"", result);
        }

        [Fact]
        public void LineItemPredicatePriceCustomerGroupKey()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Price.CustomerGroupKey() == "key123";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("price.customerGroup.key = \"key123\"", result);
        }

        [Fact]
        public void LineItemPredicateSkuAndTaxRate()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Sku() == "SKU-123" && l.TaxRate.IncludedInPrice == false;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("sku = \"SKU-123\" and taxRate.includedInPrice = false", result);
        }

        [Fact]
        public void LineItemPredicateGrouping()
        {
            Expression<Func<LineItem, bool>> expression = l => l.ProductType.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" && l.Attributes().Any(a => a.Name == "rating" && ((NumberAttribute)a).Value > 3) && (l.ProductId == "abcd9a23-14e3-40d0-aee2-3e612fcbefgh" || l.ProductId == "ba3e4ee7-30fa-400b-8155-46ebf423d793");
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("productType.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\" and attributes.rating > 3 and (product.id = \"abcd9a23-14e3-40d0-aee2-3e612fcbefgh\" or product.id = \"ba3e4ee7-30fa-400b-8155-46ebf423d793\")", result);
        }
        
        [Fact]
        public void LineItemPredicateCustomField()
        {
            Expression<Func<LineItem, bool>> expression = l => l.Custom.Fields["gender"].ToString() == "alien";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.gender = \"alien\"", result);
        }
        

        [Fact]
        public void LineItemPredicateCategoriesIdNotEqual()
        {
            Expression<Func<LineItem, bool>> expression = l => l.CategoriesId().IsNotIn("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7");
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id != (\"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\")", result);
        }

        [Fact]
        public void CustomLineItemPredicateMoneyAndTaxRate()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Money > Money.FromDecimal("EUR", 10.51M, MidpointRounding.ToEven).moneyString() && c.TaxRate.IncludedInPrice == false;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("money > \"10.51 EUR\" and taxRate.includedInPrice = false", result);
        }

        [Fact]
        public void CustomLineItemPredicateSlug()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Slug == "adidas-superstar-2";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("slug = \"adidas-superstar-2\"", result);
        }

        [Fact]
        public void CustomLineItemPredicateCustomField()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Custom.Fields["brand"].ToString() == "adidas";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.brand = \"adidas\"", result);
        }
        
        [Fact]
        public void CustomLineItemPredicateCustomFieldWithDash()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Custom.Fields["item-brand"].ToString() == "adidas";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.`item-brand` = \"adidas\"", result);
        }
        
        [Fact]
        public void CustomLineItemPredicateCustomFieldStartWithNumber()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Custom.Fields["4brand"].ToString() == "adidas";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.`4brand` = \"adidas\"", result);
        }

        [Fact]
        public void CustomLineItemPredicateCustomFieldVar()
        {
            string brand = "brand";
            Expression<Func<CustomLineItem, bool>> expression = c => c.Custom.Fields[brand].ToString() == "adidas";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.brand = \"adidas\"", result);
        }

        [Fact]
        public void CustomLineItemPredicateCustomFieldMoney()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Custom.Fields["price"].ToString() == "18.00 EUR";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.price = \"18.00 EUR\"", result);
        }
        
        [Fact]
        public void CustomLineItemPredicateCustomFieldMoneyCurrencyCode()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Custom.Fields["price"].ToMoney().CurrencyCode == "EUR";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.price.currencyCode = \"EUR\"", result);
        }
        
        [Fact]
        public void CustomLineItemPredicateCustomFieldMoneyCentAmount()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Custom.Fields["price"].ToMoney().CentAmount == 18;
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.price.centAmount = 18", result);
        }

        [Fact]
        public void LineItemPredicatePriceNot()
        {
            Expression<Func<LineItem, bool>> expression = l => !(l.Price.Value == Money.FromDecimal("EUR", 10, MidpointRounding.ToEven).moneyString());
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("not(price = \"10 EUR\")", result);
        }
        
        [Fact]
        public void CustomLineItemPredicateCustomFieldReferenceType()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Custom.Fields["reference-field"].ToString() == "bac1a3a5-3807-4f5b-9d07-0611984ecae8";
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.`reference-field` = \"bac1a3a5-3807-4f5b-9d07-0611984ecae8\"", result);
        }
        
        [Fact]
        public void CustomLineItemPredicateCustomFieldReferenceTypeInCollection()
        {
            Expression<Func<CustomLineItem, bool>> expression = c => c.Custom.Fields["reference-field"].ContainsAny("bac1a3a5-3807-4f5b-9d07-0611984ecae8", "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7");
            IDiscountPredicateExpressionVisitor cartPredicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = cartPredicateExpressionVisitor.Render(expression);
            Assert.Equal("custom.`reference-field` contains any (\"bac1a3a5-3807-4f5b-9d07-0611984ecae8\", \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\")", result);
        }
    }
}
