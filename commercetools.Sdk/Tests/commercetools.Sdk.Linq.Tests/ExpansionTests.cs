using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShoppingLists;
using System;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.Predicates;
using Xunit;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;
using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.Linq.Tests
{
    public class ExpansionTests
    {
        [Fact]
        public void ExpandCategoryParent()
        {
            Expression<Func<Category, Reference>> expression = c => c.Parent;
            ExpansionExpressionVisitor expansionVisitor = new ExpansionExpressionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("parent", result);
        }

        [Fact]
        public void ExpandCategoryParentOfParent()
        {
            Expression<Func<Category, Reference>> expression = c => c.Parent.Obj.Parent;
            ExpansionExpressionVisitor expansionVisitor = new ExpansionExpressionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("parent.parent", result);
        }

        [Fact]
        public void ExpandCategoryAllAncestors()
        {
            Expression<Func<Category, Reference>> expression = c => c.Ancestors.ExpandAll();
            ExpansionExpressionVisitor expansionVisitor = new ExpansionExpressionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("ancestors[*]", result);
        }

        [Fact]
        public void ExpandCategoryFirstAncestor()
        {
            Expression<Func<Category, Reference>> expression = c => c.Ancestors[0];
            ExpansionExpressionVisitor expansionVisitor = new ExpansionExpressionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("ancestors[0]", result);
        }

        [Fact]
        public void ExpandProductPricesAllCustomerGroups()
        {
            Expression<Func<Product, Reference>> expression = p => p.MasterData.Current.MasterVariant.Prices.ExpandAll().CustomerGroup;
            ExpansionExpressionVisitor expansionVisitor = new ExpansionExpressionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("masterData.current.masterVariant.prices[*].customerGroup", result);
        }

        [Fact]
        public void ExpandProductAllCategories()
        {
            Expression<Func<Product, Reference>> expression = p => p.MasterData.Current.Categories.ExpandAll();
            ExpansionExpressionVisitor expansionVisitor = new ExpansionExpressionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("masterData.current.categories[*]", result);
        }

        [Fact]
        public void ExpandProductAttributeAllValues()
        {
            Expression<Func<Product, Attribute>> expression = p => p.MasterData.Current.MasterVariant.Attributes.ExpandValues();
            ExpansionExpressionVisitor expansionVisitor = new ExpansionExpressionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("masterData.current.masterVariant.attributes[*].value", result);
        }

        [Fact]
        public void ExpandShoppingListLineItemsAllProductSlugs()
        {
            Expression<Func<ShoppingList, LineItem>> expression = l => l.LineItems.ExpandProductSlugs();
            ExpansionExpressionVisitor expansionVisitor = new ExpansionExpressionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("lineItems[*].productSlug", result);
        }

        [Fact]
        public void ExpandShoppingListLineItemsAllVariants()
        {
            Expression<Func<ShoppingList, LineItem>> expression = l => l.LineItems.ExpandVariants();
            ExpansionExpressionVisitor expansionVisitor = new ExpansionExpressionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("lineItems[*].variant", result);
        }
    }
}