namespace commercetools.Sdk.LinqToQueryPredicate.Tests
{
    using commercetools.Sdk.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;

    public class FacetTests
    {
        [Fact]
        public void TermFacetCategoryId()
        {
            // TODO Find a more intuitive way to define the expression (check the one below)
            Expression<Func<ProductProjection, string>> expression = p => p.Categories.Select(c => c.Id).FirstOrDefault();
            Expression<Func<ProductProjection, IEnumerable<string>>> expressionEnumerable = p => p.Categories.Select(c => c.Id);
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id", result);
        }

        [Fact]
        public void TermFacetAttributeEnumKey()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault();
            Expression<Func<ProductProjection, IEnumerable<IEnumerable<string>>>> expressionEnumerable = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color.key", result);
        }

        [Fact]
        public void TermFacetAttributeText()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "size").Select(a => ((TextAttribute)a).Value).FirstOrDefault()).FirstOrDefault();
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.size", result);
        }

        [Fact]
        public void TermFacetAttributeLocalizedText()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((LocalizedTextAttribute)a).Value["en"]).FirstOrDefault()).FirstOrDefault();
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color.en", result);
        }

        [Fact]
        public void TermFacetAttributeLocalizedEnum()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((LocalizedEnumAttribute)a).Value.Label["en"]).FirstOrDefault()).FirstOrDefault();
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color.label.en", result);
        }

        [Fact]
        public void TermFacetChannelAvailableQuantity()
        {
            Expression<Func<ProductProjection, int>> expression = p => p.Variants.Select(v => v.Availability.Channels["1a3c451e-792a-43b5-8def-88d0db22eca8"].AvailableQuantity).FirstOrDefault();
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.availability.channels.1a3c451e-792a-43b5-8def-88d0db22eca8.availableQuantity", result);
        }

        [Fact]
        public void TermFacetAverageRating()
        {
            Expression<Func<ProductProjection, double>> expression = p => p.ReviewRatingStatistics.AverageRating;
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("reviewRatingStatistics.averageRating", result);
        }

        [Fact]
        public void RangeFacetCentAmount()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Price.Value.CentAmount.Range(1, 30));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.price.centAmount:range (1 to 30)", result);
        }

        [Fact]
        public void FilterFacetCategoryId()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e");
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id:\"34940e9b-0752-4ffa-8e6e-4f2417995a3e\"", result);
        }

        [Fact]
        public void FilterFacetCategoryAllSubtrees()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(c => c.Id.Subtree("*"));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("categories.id: subtree(\"*\")", result);
        }

        [Fact]
        public void FilterFacetAttributeIn()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(v => v.Attributes.Any(a => a.Name == "color" && ((TextAttribute)a).Value.In("red", "green")));
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            string result = filterExpressionVisitor.Render(expression);
            Assert.Equal("variants.attributes.color:\"red\",\"green\"", result);
        }
    }
}
