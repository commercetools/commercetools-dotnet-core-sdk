using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace commercetools.Sdk.Linq.Tests
{
    public class SortTests
    {
        [Fact]
        public void SortCategoryParentTypeId()
        {
            Expression<Func<Category, string>> expression = c => c.Parent.Id;
            ISortExpressionVisitor sortVisitor = new SortExpressionVisitor();
            string result = sortVisitor.Render(expression);
            Assert.Equal("parent.id", result);
        }

        [Fact]
        public void SortCategorySlug()
        {
            Expression<Func<Category, string>> expression = c => c.Slug["en"];
            ISortExpressionVisitor sortVisitor = new SortExpressionVisitor();
            string result = sortVisitor.Render(expression);
            Assert.Equal("slug.en", result);
        }

        [Fact]
        public void SortProductName()
        {
            Expression<Func<Product, string>> expression = p => p.MasterData.Current.Name["en"];
            ISortExpressionVisitor sortVisitor = new SortExpressionVisitor();
            string result = sortVisitor.Render(expression);
            Assert.Equal("masterData.current.name.en", result);
        }
    }
}
