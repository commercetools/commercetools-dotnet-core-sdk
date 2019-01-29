using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Linq.Discount;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
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
            Expression<Func<Product, bool>> expression = p => p.ProductId() == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" && p.VariantId() == 1;
            IDiscountPredicateExpressionVisitor predicateExpressionVisitor = this.linqFixture.GetService<IDiscountPredicateExpressionVisitor>();
            var result = predicateExpressionVisitor.Render(expression);
            Assert.Equal("product.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\" and variantId = 1", result);
        }
    }
}
