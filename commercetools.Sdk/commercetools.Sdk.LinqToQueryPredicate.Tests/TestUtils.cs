using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq.Tests
{
    public static class TestUtils
    {
        public static ICartPredicateExpressionVisitor CreateCartPredicateExpressionVisitor()
        {
            ICartPredicateVisitorFactory cartPredicateVisitorFactory = new CartPredicateVisitorFactory(null);
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = new CartPredicateExpressionVisitor(cartPredicateVisitorFactory);
            return cartPredicateExpressionVisitor;
        }
    }
}
