using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Linq.Query;
using Xunit;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.Linq.Tests
{
    public class QueryPredicateTests : IClassFixture<LinqFixture>
    {
        private readonly LinqFixture linqFixture;

        public QueryPredicateTests(LinqFixture linqFixture)
        {
            this.linqFixture = linqFixture;
        }

        [Fact]
        public void ExpressionStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Key == "c14";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key = \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringEqualVar()
        {
            string key = "c14";
            Expression<Func<Category, bool>> expression = c => c.Key == key;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key = \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringEqualVarProperty()
        {
            Category category = new Category
            {
                Key = "c14"
            };
            // Only local variables are supported.
            // In case a comparison to a property or another expression needs to be done, then it has to be put to a local variable.
            string key = category.Key;
            Expression<Func<Category, bool>> expression = c => c.Key == key;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key = \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringNotEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringAnd()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" && c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\" and version = 30", result);
        }

        [Fact]
        public void ExpressionStringOr()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" || c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\" or version = 30", result);
        }

        [Fact]
        public void ExpressionStringAndOr()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" && c.Name["en"] == "men" || c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\" and name(en = \"men\") or version = 30", result);
        }

        [Fact]
        public void ExpressionDictionaryVar()
        {
            string language = "en";
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" && c.Name[language] == "men" || c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\" and name(en = \"men\") or version = 30", result);
        }

        [Fact]
        public void ExpressionNotStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => !(c.Key == "c14");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("not(key = \"c14\")", result);
        }

        [Fact]
        public void ExpressionNotStringAnd()
        {
            Expression<Func<Category, bool>> expression = c => !(c.Key == "c14" && c.Version == 30);
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("not(key = \"c14\" and version = 30)", result);
        }

        [Fact]
        public void ExpressionPropertyTwoLevelStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Parent.Id == "13c4ee51-ff35-490f-8e43-349e39c34646";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("parent(id = \"13c4ee51-ff35-490f-8e43-349e39c34646\")", result);
        }

        [Fact]
        public void ExpressionPropertyThreeLevelStringEqual()
        {
            Expression<Func<ProductCatalogData, bool>> expression = p => p.Current.MasterVariant.Key == "p15";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("current(masterVariant(key = \"p15\"))", result);
        }

        [Fact]
        public void ExpressionPropertyIntEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("version = 30", result);
        }

        [Fact]
        public void ExpressionPropertyIntLessThan()
        {
            Expression<Func<Category, bool>> expression = c => c.Version < 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("version < 30", result);
        }

        [Fact]
        public void ExpressionPropertyTwoLevelIntLessThan()
        {
            Expression<Func<ProductData, bool>> expression = p => p.MasterVariant.Id < 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterVariant(id < 30)", result);
        }

        [Fact]
        public void ExpressionPropertyInString()
        {
            Expression<Func<Category, bool>> expression = c => c.Key.In("c14", "c15");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key in (\"c14\", \"c15\")", result);
        }

        [Fact]
        public void ExpressionPropertyIsDefined()
        {
            Expression<Func<Category, bool>> expression = c => c.Key.IsDefined();
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key is defined", result);
        }

        [Fact]
        public void ExpressionNotPropertyInString()
        {
            Expression<Func<Category, bool>> expression = c => !c.Key.In("c14", "c15");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("not(key in (\"c14\", \"c15\"))", result);
        }

        [Fact]
        public void ExpressionPropertyNotInString()
        {
            Expression<Func<Category, bool>> expression = c => c.Key.NotIn("c14", "c15");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key not in (\"c14\", \"c15\")", result);
        }

        [Fact]
        public void ExpressionPropertyContainsAllString()
        {
            Expression<Func<Customer, bool>> expression = c => c.ShippingAddressIds.ContainsAll("c14", "c15");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("shippingAddressIds contains all (\"c14\", \"c15\")", result);
        }

        [Fact]
        public void ExpressionPropertyDictionaryEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Name["en"] == "men";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("name(en = \"men\")", result);
        }

        [Fact]
        public void ExpressionPropertyThreeLevelDictionaryEqual()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.Slug["en"] == "product";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(slug(en = \"product\")))", result);
        }

        [Fact]
        public void ExpressionPropertyTextAttributeValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => a.ToTextAttribute().Name == "text-name" && a.ToTextAttribute().Value == "text-value");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"text-name\" and value = \"text-value\")", result);
        }

        [Fact]
        public void ExpressionPropertyTextAttributeCastValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => ((TextAttribute)a).Name == "text-name" && ((TextAttribute)a).Value == "text-value");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"text-name\" and value = \"text-value\")", result);
        }

        [Fact]
        public void ExpressionPropertyLocalizedTextAttributeValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => a.ToLocalizedTextAttribute().Name == "text-name" && a.ToLocalizedTextAttribute().Value["en"] == "text-value-en");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"text-name\" and value(en = \"text-value-en\"))", result);
        }

        [Fact]
        public void ExpressionPropertyEnumAttributeValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => a.ToEnumAttribute().Name == "enum-name" && a.ToEnumAttribute().Value.Key == "enum-value");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"enum-name\" and value(key = \"enum-value\"))", result);
        }

        [Fact]
        public void ExpressionPropertyPropertyGrouping()
        {
            Expression<Func<Category, bool>> expression = c => c.Parent.Id == "some id" || c.Parent.Id == "some other id";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("parent(id = \"some id\" or id = \"some other id\")", result);
        }

        [Fact]
        public void ExpressionPropertyLocalizedTextAttributeValueEqualGrouped()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => a.ToLocalizedTextAttribute().Name == "text-name" && (a.ToLocalizedTextAttribute().Value["en"] == "text-value-en" || a.ToLocalizedTextAttribute().Value["de"] == "text-value-de"));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"text-name\" and value(en = \"text-value-en\" or de = \"text-value-de\"))", result);
        }

        [Fact(Skip = "Not implemented yet")]
        public void WithinCircle()
        {
            Expression<Func<Channel, bool>> expression = c => c.GeoLocation.WithinCircle(13, 52, 1000);
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("geoLocation within circle(13, 52, 1000)", result);
        }
    }
}