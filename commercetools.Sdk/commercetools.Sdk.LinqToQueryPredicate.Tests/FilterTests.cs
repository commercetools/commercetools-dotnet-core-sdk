using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace commercetools.Sdk.LinqToQueryPredicate.Tests
{
    public class FilterTests
    {
        [Fact]
        public void FilterByCategoryId()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Categories.Any(c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e");
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id:\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\"", result);
        }

        [Fact]
        public void FilterByCategoryMissing()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Categories.Missing();
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories:missing", result);
        }

        [Fact]
        public void FilterByCategoryExists()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Categories.Exists();
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories:exist", result);
        }

        [Fact]
        public void FilterByCategorySubtree()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Categories.Any(c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e"));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id: subtree(\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\")", result);
        }

        [Fact]
        public void FilterByCategoryTwoSubtrees()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Categories.Any(c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e") || c.Id.Subtree("2fd1d652-2533-40f1-97d7-713ac24668b1"));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id: subtree(\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\"), subtree(\"2fd1d652-2533-40f1-97d7-713ac24668b1\")", result);
        }

        [Fact]
        public void FilterByCategorySubtreeTwoIds()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Categories.Any(c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e") || c.Id == "2fd1d652-2533-40f1-97d7-713ac24668b1" || c.Id == "51e7da39-4946-4c1d-a948-af7b54874891");
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id: subtree(\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\"), \"2fd1d652-2533-40f1-97d7-713ac24668b1\", \"51e7da39-4946-4c1d-a948-af7b54874891\"", result);
        }

        [Fact]
        public void FilterByPriceCentAmount()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount == 30);
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:30", result);
        }

        [Fact]
        public void FilterByPriceCentAmountRange()
        {
            // TODO Check if this should be done in a more native way with < and >, other than custom extension
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 30));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (1 to 30)", result);
        }

        [Fact]
        public void FilterByPriceCentAmountTwoRanges()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 30) && v.Price.Value.CentAmount.Range(40, 100));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (1 to 30), (40 to 100)", result);
        }

        [Fact]
        public void FilterByPriceCentAmountLowerBoundUknownRange()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(null, 30));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (* to 30)", result);
        }

        [Fact]
        public void FilterByPriceCentAmountUpperBoundUknownRange()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, null));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (1 to *)", result);
        }

        [Fact]
        public void FilterByPricesMissing()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Prices.Missing());
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.prices:missing", result);
        }

        [Fact]
        public void FilterByPricesExists()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Prices.Exists());
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.prices:exists", result);
        }

        [Fact]
        public void FilterByTextAttributeValue()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "color" && ((TextAttribute)a).Value == "red"));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attribute.color:\"red\"", result);
        }

        [Fact]
        public void FilterByNumberAttributeRange()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "size" && ((NumberAttribute)a).Value.Range(36, 42)));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attribute.size:range (36 to 42)", result);
        }

        [Fact]
        public void FilterByNumberAttributeMissing()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Attributes.Where(a => a.Name == "size").Missing());
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attribute.size:missing", result);
        }

        [Fact]
        public void FilterByNumberAttributeExists()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Attributes.Where(a => a.Name == "size").Exists());
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attribute.size:exists", result);
        }

        [Fact]
        public void FilterByEnumAttribute()
        {
            Expression<Func<ProductData, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "enum" && ((EnumAttribute)a).Value.Key == "value"));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attribute.enum.key:\"value\"", result);
        }
    }
}
