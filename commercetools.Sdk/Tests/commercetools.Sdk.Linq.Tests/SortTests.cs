using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using System;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Linq.Sort;
using Xunit;

namespace commercetools.Sdk.Linq.Tests
{
    public class SortTests : IClassFixture<LinqFixture>
    {
        private readonly LinqFixture linqFixture;

        public SortTests(LinqFixture linqFixture)
        {
            this.linqFixture = linqFixture;
        }

        [Fact]
        public void SortCategoryParentTypeId()
        {
            Expression<Func<Category, string>> expression = c => c.Parent.Id;
            ISortPredicateExpressionVisitor sortVisitor = this.linqFixture.GetService<ISortPredicateExpressionVisitor>();
            string result = sortVisitor.Render(expression);
            Assert.Equal("parent.id", result);
        }

        [Fact]
        public void SortCategorySlug()
        {
            Expression<Func<Category, string>> expression = c => c.Slug["en"];
            ISortPredicateExpressionVisitor sortVisitor = this.linqFixture.GetService<ISortPredicateExpressionVisitor>();
            string result = sortVisitor.Render(expression);
            Assert.Equal("slug.en", result);
        }

        [Fact]
        public void SortProductName()
        {
            Expression<Func<Product, string>> expression = p => p.MasterData.Current.Name["en"];
            ISortPredicateExpressionVisitor sortVisitor = this.linqFixture.GetService<ISortPredicateExpressionVisitor>();
            string result = sortVisitor.Render(expression);
            Assert.Equal("masterData.current.name.en", result);
        }

        [Fact]
        public void SortProductProjectionByName()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Name["en"];
            ISortPredicateExpressionVisitor sortVisitor = this.linqFixture.GetService<ISortPredicateExpressionVisitor>();
            string result = sortVisitor.Render(expression);
            Assert.Equal("name.en", result);
        }

        [Fact]
        public void SortProductProjectionByCreatedAt()
        {
            Expression<Func<ProductProjection, DateTime>> expression = p => p.CreatedAt;
            ISortPredicateExpressionVisitor sortVisitor = this.linqFixture.GetService<ISortPredicateExpressionVisitor>();
            string result = sortVisitor.Render(expression);
            Assert.Equal("createdAt", result);
        }

        [Fact]
        public void SortProductProjectionByReviewAverageRating()
        {
            Expression<Func<ProductProjection, double>> expression = p => p.ReviewRatingStatistics.AverageRating;
            ISortPredicateExpressionVisitor sortVisitor = this.linqFixture.GetService<ISortPredicateExpressionVisitor>();
            string result = sortVisitor.Render(expression);
            Assert.Equal("reviewRatingStatistics.averageRating", result);
        }


        [Fact]
        public void SortProductProjectionByAvailability()
        {
            Expression<Func<ProductProjection, IComparable>> expression = p => p.Variants.Select(v => v.Availability.AvailableQuantity).FirstOrDefault();
            var sort = new Sort<ProductProjection>(expression, SortDirection.Descending);
            string result = sort.ToString();
            Assert.Equal("variants.availability.availableQuantity desc", result);
        }

        [Fact]
        public void SortProductProjectionByChannelAvailability()
        {
            Expression<Func<ProductProjection, IComparable>> expression = p => p.Variants.Select(v => v.Availability.Channels["1a3c451e-792a-43b5-8def-88d0db22eca8"].AvailableQuantity).FirstOrDefault();
            var sort = new Sort<ProductProjection>(expression, SortDirection.DescendingWithMinValue);
            string result = sort.ToString();
            Assert.Equal("variants.availability.channels.1a3c451e-792a-43b5-8def-88d0db22eca8.availableQuantity desc.min", result);
        }

        [Fact]
        public void SortProductProjectionByChannelRestockableInDays()
        {
            Expression<Func<ProductProjection, IComparable>> expression = p => p.Variants.Select(v => v.Availability.Channels["1a3c451e-792a-43b5-8def-88d0db22eca8"].RestockableInDays).FirstOrDefault();
            var sort = new Sort<ProductProjection>(expression, SortDirection.AscendingWithMaxValue);
            string result = sort.ToString();
            Assert.Equal("variants.availability.channels.1a3c451e-792a-43b5-8def-88d0db22eca8.restockableInDays asc.max", result);
        }

        [Fact]
        public void SortProductProjectionByValueAttribute()
        {
            Expression<Func<ProductProjection, IComparable>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "size").Select(a => ((TextAttribute)a).Value).FirstOrDefault()).FirstOrDefault();
            var sort = new Sort<ProductProjection>(expression, SortDirection.Ascending);
            string result = sort.ToString();
            Assert.Equal("variants.attributes.size asc", result);
        }

        [Fact]
        public void SortProductProjectionByMoneyAttribute()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "customMoney").Select(a => ((MoneyAttribute)a).Value.CentAmount.ToString()).FirstOrDefault()).FirstOrDefault();
            ISortPredicateExpressionVisitor sortVisitor = this.linqFixture.GetService<ISortPredicateExpressionVisitor>();
            string result = sortVisitor.Render(expression);
            Assert.Equal("variants.attributes.customMoney.centAmount", result);
        }

        [Fact]
        public void SortProductProjectionAttributeEnumKey()
        {
            Expression<Func<ProductProjection, string>> expression = p => p.Variants.Select(v => v.Attributes.Where(a => a.Name == "color").Select(a => ((EnumAttribute)a).Value.Key).FirstOrDefault()).FirstOrDefault();
            ISortPredicateExpressionVisitor sortVisitor = this.linqFixture.GetService<ISortPredicateExpressionVisitor>();
            string result = sortVisitor.Render(expression);
            Assert.Equal("variants.attributes.color.key", result);
        }
    }
}
