using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace commercetools.Sdk.LinqToQueryPredicate.Tests
{
    public class ExpansionTests
    {
        [Fact]
        public void ExpandCategoryParent()
        {
            Expression<Func<Category, Reference>> expression = c => c.Parent;
            ExpansionVisitor expansionVisitor = new ExpansionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("parent", result);
        }

        [Fact]
        public void ExpandCategoryParentOfParent()
        {
            Expression<Func<Category, Reference>> expression = c => c.Parent.Obj.Parent;
            ExpansionVisitor expansionVisitor = new ExpansionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("parent.parent", result);
        }

        [Fact]
        public void ExpandCategoryAllAncestors()
        {
            Expression<Func<Category, Reference>> expression = c => c.Ancestors.ExpandAll();
            ExpansionVisitor expansionVisitor = new ExpansionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("ancestors[*]", result);
        }

        [Fact]
        public void ExpandCategoryFirstAncestor()
        {
            Expression<Func<Category, Reference>> expression = c => c.Ancestors[0];
            ExpansionVisitor expansionVisitor = new ExpansionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("ancestors[0]", result);
        }

        [Fact]
        public void ExpandProductPricesAllCustomerGroups()
        {
            Expression<Func<Product, Reference>> expression = p => p.MasterData.Current.MasterVariant.Prices.ExpandAll().CustomerGroup;
            ExpansionVisitor expansionVisitor = new ExpansionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("masterData.current.masterVariant.prices[*].customerGroup", result);
        }

        [Fact]
        public void ExpandProductAllCategories()
        {
            Expression<Func<Product, Reference>> expression = p => p.MasterData.Current.Categories.ExpandAll();
            ExpansionVisitor expansionVisitor = new ExpansionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("masterData.current.categories[*]", result);
        }

        [Fact]
        public void ExpandProductAttributeAllValues()
        {
            Expression<Func<Product, Domain.Attribute>> expression = p => p.MasterData.Current.MasterVariant.Attributes.ExpandValues();
            ExpansionVisitor expansionVisitor = new ExpansionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("masterData.current.masterVariant.attributes[*].value", result);
        }

        [Fact]
        public void ExpandShoppingListLineItemsAllProductSlugs()
        {
            Expression<Func<ShoppingList, LineItem>> expression = l => l.LineItems.ExpandProductSlugs();
            ExpansionVisitor expansionVisitor = new ExpansionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("lineItems[*].productSlug", result);
        }

        [Fact]
        public void ExpandShoppingListLineItemsAllVariants()
        {
            Expression<Func<ShoppingList, LineItem>> expression = l => l.LineItems.ExpandVariants();
            ExpansionVisitor expansionVisitor = new ExpansionVisitor();
            string result = expansionVisitor.GetPath(expression);
            Assert.Equal("lineItems[*].variant", result);
        }
    }      
}
