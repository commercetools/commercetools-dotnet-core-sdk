using commercetools.Sdk.Domain.ProductProjections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Linq.Filter;
using Xunit;

namespace commercetools.Sdk.Linq.Tests
{
    public class FacetTests : IClassFixture<LinqFixture>
    {
        private readonly LinqFixture linqFixture;

        public FacetTests(LinqFixture linqFixture)
        {
            this.linqFixture = linqFixture;
        }

        [Fact]
        public void TermFacetCategoryId()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Categories.Select(c => c.Id).FirstOrDefault();
            // This is another way of expressing the filter, but it is not implemented.
            Expression<Func<ProductProjection, IEnumerable<string>>> expressionEnumerable = p => p.Categories.Select(c => c.Id);
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id", result);
        }

        [Fact]
        public void TermFacetAttributeEnumKey()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault();
            Expression<Func<ProductProjection, IEnumerable<IEnumerable<string>>>> expressionEnumerable = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color.key", result);
        }

        [Fact]
        public void TermFacetAttributeText()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "size").Select(a => ((TextAttribute)a).Value).FirstOrDefault()).FirstOrDefault();
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.size", result);
        }

        [Fact]
        public void TermFacetAttributeLocalizedText()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((LocalizedTextAttribute)a).Value["en"]).FirstOrDefault()).FirstOrDefault();
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color.en", result);
        }

        [Fact]
        public void TermFacetAttributeLocalizedEnum()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((LocalizedEnumAttribute)a).Value.Label["en"]).FirstOrDefault()).FirstOrDefault();
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color.label.en", result);
        }

        [Fact]
        public void TermFacetAttributeMoneyCentAmount()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "customMoney").Select(a => ((MoneyAttribute)a).Value.CentAmount.ToString()).FirstOrDefault()).FirstOrDefault();
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.customMoney.centAmount", result);
        }
        [Fact]
        public void TermFacetAttributeMoneyCurrencyCode()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "customMoney").Select(a => ((MoneyAttribute)a).Value.CurrencyCode).FirstOrDefault()).FirstOrDefault();
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.customMoney.currencyCode", result);
        }

        [Fact]
        public void TermFacetAvailableQuantity()
        {
            Expression<Func<ProductProjection, long>> expression = p => p.Variants.Select(v => v.Availability.AvailableQuantity).FirstOrDefault();
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.availability.availableQuantity", result);
        }

        [Fact]
        public void TermFacetChannelAvailableQuantity()
        {
            Expression<Func<ProductProjection, long>> expression = p => p.Variants.Select(v => v.Availability.Channels["1a3c451e-792a-43b5-8def-88d0db22eca8"].AvailableQuantity).FirstOrDefault();
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.availability.channels.1a3c451e-792a-43b5-8def-88d0db22eca8.availableQuantity", result);
        }

        [Fact]
        public void TermFacetAverageRating()
        {
            Expression<Func<ProductProjection, double>> expression = p => p.ReviewRatingStatistics.AverageRating;
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("reviewRatingStatistics.averageRating", result);
        }

        [Fact]
        public void RangeFacetCentAmount()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 30));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (1 to 30)", result);
        }

        [Fact]
        public void RangeFacetCustomAttribute()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any( v => v.Attributes.Any(a => a.Name == "size" && ((NumberAttribute) a).Value.Range(36, 42)));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.size:range (36 to 42)", result);
        }

        [Fact]
        public void FilteredFacetCategoryId()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e");
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id:\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\"", result);
        }

        [Fact]
        public void FilteredFacetCategoryAllSubtrees()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(c => c.Id.Subtree("*"));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id: subtree(\"*\")", result);
        }

        [Fact]
        public void FilteredFacetCategoryInTwoSubtrees()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e") || c.Id.Subtree("2fd1d652-2533-40f1-97d7-713ac24668b1"));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            var result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id: subtree(\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\"), subtree(\"2fd1d652-2533-40f1-97d7-713ac24668b1\")", result);
        }

        [Fact]
        public void FilteredFacetBySpecificPrice()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount == 30);
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:30", result);
        }

        [Fact]
        public void FilteredFacetAttributeIn()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "color" && ((TextAttribute)a).Value.In("red", "green")));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color:\"red\",\"green\"", result);
        }

        [Fact]
        public void FilterFacetAttributeInArray()
        {
            var t = new string[]
            {
                "red", "green"
            };
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "color" && ((TextAttribute)a).Value.In(t)));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color:\"red\",\"green\"", result);
        }

        [Fact]
        public void FilterFacetAttributeInTraversable()
        {
            var t = new List<PlainEnumValue>()
            {
                new PlainEnumValue() { Key = "red"},
                new PlainEnumValue() { Key = "green"},
            };
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "color" && ((TextAttribute)a).Value.In(t.Select(value => value.Key).ToArray())));
            IFilterPredicateExpressionVisitor filterExpressionVisitor = this.linqFixture.GetService<IFilterPredicateExpressionVisitor>();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color:\"red\",\"green\"", result);
        }
    }
}
