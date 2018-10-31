using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq.Tests
{
    public static class TestUtils
    {
        public static ICartPredicateExpressionVisitor CreateCartPredicateExpressionVisitor()
        {
            IAccessorTraverser accessorTraverser = new AccessorTraverser();
            ICartPredicateVisitorConverter memberPredicateVisitorConverter = new MemberPredicateVisitorConverter(accessorTraverser);
            ICartPredicateVisitorConverter binaryLogicalPredicateVisitorConverter = new BinaryLogicalPredicateVisitorConverter();
            ICartPredicateVisitorConverter comparisonPredicateVisitorConverter = new ComparisonPredicateVisitorConverter();
            ICartPredicateVisitorConverter constantPredicateVisitorConverter = new ConstantPredicateVisitorConverter();
            ICartPredicateVisitorConverter customMethodPredicateVisitorConverter = new CustomMethodPredicateVisitorConverter();
            ICartPredicateVisitorConverter methodMemberPredicateVisitorConverter = new MethodMemberPredicateVisitorConverter(accessorTraverser);
            ICartPredicateVisitorConverter attributePredicateVisitorConverter = new AttributePredicateVisitorConverter(accessorTraverser);
            ICartPredicateVisitorConverter methodPredicateVisitorConverter = new MethodPredicateVisitorConverter();
            ICartPredicateVisitorConverter moneyParsePredicateVisitorConverter = new MoneyParsePredicateConverter();
            ICartPredicateVisitorConverter selectMethodPredicateVisitorConverter = new SelectMethodPredicateConverter(accessorTraverser);
            ICartPredicateVisitorFactory cartPredicateVisitorFactory = new CartPredicateVisitorFactory(new List<ICartPredicateVisitorConverter>() { binaryLogicalPredicateVisitorConverter, comparisonPredicateVisitorConverter, constantPredicateVisitorConverter, memberPredicateVisitorConverter, customMethodPredicateVisitorConverter, methodMemberPredicateVisitorConverter, attributePredicateVisitorConverter, methodPredicateVisitorConverter, moneyParsePredicateVisitorConverter, selectMethodPredicateVisitorConverter });
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = new CartPredicateExpressionVisitor(cartPredicateVisitorFactory);
            return cartPredicateExpressionVisitor;
        }
    }
}
