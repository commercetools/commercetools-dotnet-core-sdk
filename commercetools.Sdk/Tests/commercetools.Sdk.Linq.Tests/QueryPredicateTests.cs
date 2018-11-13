using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace commercetools.Sdk.LinqToQueryPredicate.Tests
{
    public class QueryPredicateTests
    {
        [Fact]
        public void ExpressionStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Key == "c14";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("key = \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringNotEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("key != \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringAnd()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" && c.Version == 30;
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("key != \"c14\" and version = 30", result);
        }

        [Fact]
        public void ExpressionStringOr()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" || c.Version == 30;
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("key != \"c14\" or version = 30", result);
        }

        [Fact]
        public void ExpressionStringAndOr()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" && c.Name["en"] == "men" || c.Version == 30;
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("key != \"c14\" and name(en = \"men\") or version = 30", result);
        }

        [Fact]
        public void ExpressionNotStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => !(c.Key == "c14");
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("not(key = \"c14\")", result);
        }

        [Fact]
        public void ExpressionNotStringAnd()
        {
            Expression<Func<Category, bool>> expression = c => !(c.Key == "c14" && c.Version == 30);
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("not(key = \"c14\" and version = 30)", result);
        }

        [Fact]
        public void ExpressionPropertyTwoLevelStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Parent.Id == "13c4ee51-ff35-490f-8e43-349e39c34646";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("parent(id = \"13c4ee51-ff35-490f-8e43-349e39c34646\")", result);
        }

        [Fact]
        public void ExpressionPropertyThreeLevelStringEqual()
        {
            Expression<Func<ProductCatalogData, bool>> expression = p => p.Current.MasterVariant.Key == "p15";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("current(masterVariant(key = \"p15\"))", result);
        }

        [Fact]
        public void ExpressionPropertyIntEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Version == 30;
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("version = 30", result);
        }        

        [Fact]
        public void ExpressionPropertyIntLessThan()
        {
            Expression<Func<Category, bool>> expression = c => c.Version < 30;
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("version < 30", result);
        }

        [Fact]
        public void ExpressionPropertyTwoLevelIntLessThan()
        {
            Expression<Func<ProductData, bool>> expression = p => p.MasterVariant.Id < 30;
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("masterVariant(id < 30)", result);
        }

        [Fact]
        public void ExpressionPropertyInString()
        {
            Expression<Func<Category, bool>> expression = c => c.Key.In("c14", "c15");
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("key in (\"c14\", \"c15\")", result);
        }

        [Fact]
        public void ExpressionNotPropertyInString()
        {
            Expression<Func<Category, bool>> expression = c => !c.Key.In("c14", "c15");
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("not(key in (\"c14\", \"c15\"))", result);
        }

        [Fact]
        public void ExpressionPropertyNotInString()
        {
            Expression<Func<Category, bool>> expression = c => c.Key.NotIn("c14", "c15");
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("key not in (\"c14\", \"c15\")", result);
        }

        [Fact]
        public void ExpressionPropertyContainsAllString()
        {
            Expression<Func<Customer, bool>> expression = c => c.ShippingAddressIds.ContainsAll("c14", "c15");
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("shippingAddressIds contains all (\"c14\", \"c15\")", result);
        }

        [Fact]
        public void ExpressionPropertyDictionaryEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Name["en"] == "men";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("name(en = \"men\")", result);
        }

        [Fact]
        public void ExpressionPropertyThreeLevelDictionaryEqual()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.Slug["en"] == "product";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("masterData(current(slug(en = \"product\")))", result);
        }

        [Fact]
        public void ExpressionPropertyTextAttributeValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => ((TextAttribute)a).Name == "text-name" && ((TextAttribute)a).Value == "text-value");
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("attributes(name = \"text-name\" and value = \"text-value\")", result);
        }

        [Fact]
        public void ExpressionPropertyLocalizedTextAttributeValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => ((LocalizedTextAttribute)a).Name == "text-name" && ((LocalizedTextAttribute)a).Value["en"] == "text-value-en");
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("attributes(name = \"text-name\" and value(en = \"text-value-en\"))", result);
        }

        [Fact]
        public void ExpressionPropertyEnumAttributeValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => ((EnumAttribute)a).Name == "enum-name" && ((EnumAttribute)a).Value.Key == "enum-value");
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("attributes(name = \"enum-name\" and value(key = \"enum-value\"))", result);
        }

        [Fact]
        public void ContainsAll()
        {
            List<string> list = new List<string>() { "a", "b", "c" };
            bool result = list.ContainsAll("a", "b", "c");
            Assert.True(result);
        }

        [Fact]
        public void ExpressionPropertyPropertyGrouping()
        {
            Expression<Func<Category, bool>> expression = c => c.Parent.Id == "some id" && c.Parent.TypeId == "some type id";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("parent(id = \"some id\" and typeId = \"some type id\")", result);
        }

        [Fact]
        public void ExpressionPropertyLocalizedTextAttributeValueEqualGrouped()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => ((LocalizedTextAttribute)a).Name == "text-name" && (((LocalizedTextAttribute)a).Value["en"] == "text-value-en" || ((LocalizedTextAttribute)a).Value["de"] == "text-value-de"));
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("attributes(name = \"text-name\" and value(en = \"text-value-en\" or de = \"text-value-de\"))", result);
        }
    }      
}
