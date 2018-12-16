using System;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Linq.Filter;
using Xunit;

namespace commercetools.Sdk.Linq.Tests
{
    public class FilterTests : IClassFixture<LinqFixture>
    {
        private readonly LinqFixture linqFixture;

        public FilterTests(LinqFixture linqFixture)
        {
            this.linqFixture = linqFixture;
        }

        [Fact]
        public void FilterByCategoryId()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e");
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id:\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\"", result);
        }

        [Fact]
        public void FilterByCategoryMissing()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Missing();
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories:missing", result);
        }

        [Fact]
        public void FilterByCategoryExists()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Exists();
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories:exists", result);
        }

        [Fact]
        public void FilterByCategorySubtree()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e"));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id: subtree(\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\")", result);
        }

        [Fact]
        public void FilterByCategoryTwoSubtrees()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e") || c.Id.Subtree("2fd1d652-2533-40f1-97d7-713ac24668b1"));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id: subtree(\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\"), subtree(\"2fd1d652-2533-40f1-97d7-713ac24668b1\")", result);
        }

        [Fact]
        public void FilterByCategorySubtreeTwoIds()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e") || c.Id == "2fd1d652-2533-40f1-97d7-713ac24668b1" || c.Id == "51e7da39-4946-4c1d-a948-af7b54874891");
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id: subtree(\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\"),\"2fd1d652-2533-40f1-97d7-713ac24668b1\",\"51e7da39-4946-4c1d-a948-af7b54874891\"", result);
        }

        [Fact]
        public void FilterByPriceCentAmount()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount == 30);
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:30", result);
        }

        [Fact]
        public void FilterByPriceCentAmountRange()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 30));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (1 to 30)", result);
        }

        [Fact]
        public void FilterByPriceCentAmountTwoRanges()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 30) || v.Price.Value.CentAmount.Range(40, 100));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (1 to 30), (40 to 100)", result);
        }

        [Fact]
        public void FilterByPriceCentAmountLowerBoundUnknownRange()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(null, 30));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (* to 30)", result);
        }

        [Fact]
        public void FilterByPriceCentAmountUpperBoundUnknownRange()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, null));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (1 to *)", result);
        }

        [Fact]
        public void FilterByPricesMissing()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Prices.Missing());
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.prices:missing", result);
        }

        [Fact]
        public void FilterByPricesExists()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Prices.Exists());
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.prices:exists", result);
        }

        [Fact]
        public void FilterByTextAttributeValue()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "color" && ((TextAttribute)a).Value == "red"));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color:\"red\"", result);
        }

        [Fact]
        public void FilterByNumberAttributeRange()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "size" && ((NumberAttribute)a).Value.Range(36, 42)));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.size:range (36 to 42)", result);
        }

        [Fact]
        public void FilterByNumberAttributeRangeVar()
        {
            int from = 36;
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "size" && ((NumberAttribute)a).Value.Range(from, 42)));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.size:range (36 to 42)", result);
        }

        [Fact]
        public void FilterByNumberAttributeMissing()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Where(a => a.Name == "size").Missing());
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.size:missing", result);
        }

        [Fact]
        public void FilterByNumberAttributeExists()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Where(a => a.Name == "size").Exists());
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.size:exists", result);
        }

        [Fact]
        public void FilterByEnumAttribute()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "color" && ((EnumAttribute)a).Value.Key == "grey"));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color.key:\"grey\"", result);
        }

        [Fact]
        public void FilterByIsOnStock()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Availability.IsOnStock == true);
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.availability.isOnStock:true", result);
        }

        [Fact]
        public void FilterByIsOnStockPerChannel()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Availability.Channels["1a3c451e-792a-43b5-8def-88d0db22eca8"].IsOnStock == true);
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.availability.channels.1a3c451e-792a-43b5-8def-88d0db22eca8.isOnStock:true", result);
        }

        [Fact]
        public void FilterByIsOnStockPerChannelVar()
        {
            string channel = "1a3c451e-792a-43b5-8def-88d0db22eca8";
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Availability.Channels[channel].IsOnStock == true);
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.availability.channels.1a3c451e-792a-43b5-8def-88d0db22eca8.isOnStock:true", result);
        }

        [Fact]
        public void FilterByAvailabilityIsOnStockInChannels()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Availability.IsOnStockInChannels("1a3c451e-792a-43b5-8def-88d0db22eca8", "110321ab-8fd7-4d4c-8b7c-4c69761411fc"));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.availability.isOnStockInChannels:\"1a3c451e-792a-43b5-8def-88d0db22eca8\",\"110321ab-8fd7-4d4c-8b7c-4c69761411fc\"", result);
        }

        [Fact]
        public void FilterBySearchKeywords()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.SearchKeywords["en"].Any(s => s.Text == "jeans");
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("searchKeywords.en.text:\"jeans\"", result);
        }

        [Fact]
        public void FilterBySearchKeywordsVar()
        {
            string language = "en";
            Expression<Func<ProductProjection, bool>> expression = p => p.SearchKeywords[language].Any(s => s.Text == "jeans");
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("searchKeywords.en.text:\"jeans\"", result);
        }

        [Fact]
        public void FilterByCreatedAt()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.CreatedAt.Range(DateTime.Parse("2015-06-04T12:27:55.344Z"), DateTime.Parse("2016-06-04T12:27:55.344Z"));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("createdAt:range (\"2015-06-04T12:27:55.344Z\" to \"2016-06-04T12:27:55.344Z\")", result);
        }

        [Fact]
        public void FilterByCreatedAtParseVar()
        {
            string date = "2015-06-04T12:27:55.344Z";
            Expression<Func<ProductProjection, bool>> expression = p => p.CreatedAt.Range(DateTime.Parse(date), DateTime.Parse("2016-06-04T12:27:55.344Z"));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("createdAt:range (\"2015-06-04T12:27:55.344Z\" to \"2016-06-04T12:27:55.344Z\")", result);
        }
    }
}