using System.Collections.Generic;

namespace commercetools.Sdk.Linq.Tests
{
    public static class TestUtils
    {
        public static ICartPredicateExpressionVisitor CreateCartPredicateExpressionVisitor()
        {
            ICartPredicateVisitorConverter memberPredicateVisitorConverter = new MemberPredicateVisitorConverter();
            ICartPredicateVisitorConverter binaryLogicalPredicateVisitorConverter = new BinaryLogicalPredicateVisitorConverter();
            ICartPredicateVisitorConverter comparisonPredicateVisitorConverter = new ComparisonPredicateVisitorConverter();
            ICartPredicateVisitorConverter constantPredicateVisitorConverter = new ConstantPredicateVisitorConverter();
            ICartPredicateVisitorConverter customMethodPredicateVisitorConverter = new CustomMethodPredicateVisitorConverter();
            ICartPredicateVisitorConverter methodMemberPredicateVisitorConverter = new MethodMemberPredicateVisitorConverter();
            ICartPredicateVisitorConverter attributePredicateVisitorConverter = new AttributePredicateVisitorConverter();
            ICartPredicateVisitorConverter methodPredicateVisitorConverter = new MethodPredicateVisitorConverter();
            ICartPredicateVisitorConverter moneyParsePredicateVisitorConverter = new MoneyParsePredicateConverter();
            ICartPredicateVisitorConverter converterMethodsPredicateVisitorConverter = new ConverterMethodsPredicateVisitorConverter();
            ICartPredicateVisitorConverter customFieldsConverter = new CustomFieldsConverter();
            ICartPredicateVisitorConverter selectMethodPredicateVisitorConverter = new SelectMethodPredicateConverter();
            ICartPredicateVisitorConverter notLogicalPredicateVisitorConverter = new NotLogicalPredicateVisitorConverter();
            ICartPredicateVisitorFactory cartPredicateVisitorFactory = new CartPredicateVisitorFactory(new List<ICartPredicateVisitorConverter>() { binaryLogicalPredicateVisitorConverter, comparisonPredicateVisitorConverter, constantPredicateVisitorConverter, memberPredicateVisitorConverter, customMethodPredicateVisitorConverter, methodMemberPredicateVisitorConverter, attributePredicateVisitorConverter, methodPredicateVisitorConverter, moneyParsePredicateVisitorConverter, selectMethodPredicateVisitorConverter, converterMethodsPredicateVisitorConverter, customFieldsConverter, notLogicalPredicateVisitorConverter });
            ICartPredicateExpressionVisitor cartPredicateExpressionVisitor = new CartPredicateExpressionVisitor(cartPredicateVisitorFactory);
            return cartPredicateExpressionVisitor;
        }
    }
}