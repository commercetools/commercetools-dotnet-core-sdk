﻿using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Linq.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using commercetools.Sdk.Domain.Categories;
using Xunit;

namespace commercetools.Sdk.Linq.Tests
{
    public class ProductDiscountPredicateTests : IClassFixture<LinqFixture>
    {
        private readonly LinqFixture linqFixture;

        public ProductDiscountPredicateTests(LinqFixture linqFixture)
        {
            this.linqFixture = linqFixture;
        }

        [Fact]
        public void ProductIdAndVariantId()
        {
            // The first line would have written out only id and not product.id, hence we need an extension
            // Expression<Func<Product, bool>> expressionTest = p => p.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" && p.VariantId() == 1;
            Expression<Func<Product, bool>> expression = p => p.ProductId() == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" && p.VariantId() == 1;
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("product.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\" and variant.id = 1", result);
        }

        [Fact]
        public void CategoriesIdEqual()
        {
            Expression<Func<Product, bool>> expression = p => p.CategoryId() == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7";
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

        [Fact]
        public void CategoriesIdContains()
        {
            Expression<Func<Product, bool>> expression = p => p.CategoriesId().Contains("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7");
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id contains \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

        [Fact]
        public void CategoriesIdContainsAny()
        {
            Expression<Func<Product, bool>> expression = p => p.CategoriesId().ContainsAny("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", "abcd9a23-14e3-40d0-aee2-3e612fcbefgh");
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id contains any (\"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\", \"abcd9a23-14e3-40d0-aee2-3e612fcbefgh\")", result);
        }

        [Fact]
        public void CategoriesIdContainsAll()
        {
            Expression<Func<Product, bool>> expression = p => p.CategoriesId().ContainsAll("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", "abcd9a23-14e3-40d0-aee2-3e612fcbefgh");
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id contains all (\"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\", \"abcd9a23-14e3-40d0-aee2-3e612fcbefgh\")", result);
        }

        [Fact]
        public void CategoriesIdEqualsMultipleIds()
        {
            Expression<Func<Product, bool>> expression = p => p.CategoriesId().IsIn("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", "abcd9a23-14e3-40d0-aee2-3e612fcbefgh");
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id = (\"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\", \"abcd9a23-14e3-40d0-aee2-3e612fcbefgh\")", result);
        }

        [Fact]
        public void CategoriesIdEqualsSingleId()
        {
            Expression<Func<Product, bool>> expression = p => p.CategoriesId().IsIn("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7");
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

        [Fact]
        public void CategoriesNotEqualsMultipleIds()
        {
            var category = new Category
            {
                Id = "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"
            };
            Expression<Func<Product, bool>> expression = p => p.CategoriesId().IsNotIn(category.Id.valueOf(), "963cbb75-c604-4ad2-841c-890b792224ee");
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id != (\"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\", \"963cbb75-c604-4ad2-841c-890b792224ee\")", result);
        }

        [Fact]
        public void CategoriesNotEqualsSingleId()
        {
            var category = new Category
            {
                Id = "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"
            };
            Expression<Func<Product, bool>> expression = p => p.CategoriesId().IsNotIn(category.Id.valueOf());
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("categories.id != (\"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\")", result);
        }

        [Fact]
        public void CategoriesWithAncestorsIdContains()
        {
            Expression<Func<Product, bool>> expression = p => p.CategoriesWithAncestorsId().Contains("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7");
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("categoriesWithAncestors.id contains \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

        [Fact]
        public void PriceAndCustomerGroup()
        {
            Expression<Func<Product, bool>> expression = p => p.CentAmount() > 1200 && p.Currency() == "EUR" && p.Country() != "FR" && p.CustomerGroup().Id.IsNotDefined();
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("centAmount > 1200 and currency = \"EUR\" and country != \"FR\" and customerGroup.id is not defined", result);
        }

        [Fact]
        public void Attributes()
        {
            Expression<Func<Product, bool>> expression = p => p.Attributes().Any(a => a.Name == "size" && a.ToTextAttribute().Value == "L") && p.Attributes().Any(a => a.Name == "colors" && a.ToSetEnumAttribute().ContainsAll("black", "white"));
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.size = \"L\" and attributes.colors contains all (\"black\", \"white\")", result);
        }

        [Fact]
        public void AttributeNameWithDash()
        {
            Expression<Func<Product, bool>> expression = p => p.Attributes().Any(a => a.Name == "size-att" && a.ToTextAttribute().Value == "L") && p.Attributes().Any(a => a.Name == "colors-attr" && a.ToSetEnumAttribute().ContainsAll("black", "white"));
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.`size-att` = \"L\" and attributes.`colors-attr` contains all (\"black\", \"white\")", result);
        }

        [Fact]
        public void AttributeNameStartWithNumber()
        {
            Expression<Func<Product, bool>> expression = p => p.Attributes().Any(a => a.Name == "4size" && a.ToTextAttribute().Value == "L") && p.Attributes().Any(a => a.Name == "4colors" && a.ToSetEnumAttribute().ContainsAll("black", "white"));
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes.`4size` = \"L\" and attributes.`4colors` contains all (\"black\", \"white\")", result);
        }

        [Fact]
        public void SkuAndAttributes()
        {
            Expression<Func<Product, bool>> expression = p => p.Sku() == "AB-12" && p.Attributes().Any(a => a.Name == "available" && a.ToBooleanAttribute().Value == true) && p.Attributes().Any(a => a.Name == "weight" && a.ToNumberAttribute().Value < 100);
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("sku = \"AB-12\" and attributes.available = true and attributes.weight < 100", result);
        }
    }
}
