using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class MethodMemberPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (DiscountMapping.MethodAccessors.ContainsKey(methodCallExpression.Method.Name))
                {
                    return true;
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                string currentName = methodCallExpression.Method.Name;
                string currentAccessor = ParseMethodAccessorName(currentName);
                if (string.IsNullOrEmpty(currentAccessor))
                {
                    return predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                }

                AccessorPredicateVisitor parentAccessor = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]) as AccessorPredicateVisitor;
                ConstantPredicateVisitor constantPredicateVisitor = new ConstantPredicateVisitor(currentAccessor);
                return new AccessorPredicateVisitor(constantPredicateVisitor, parentAccessor);
            }

            throw new NotSupportedException();
        }

        private static string ParseMethodAccessorName(string name)
        {
            if (DiscountMapping.MethodAccessors.ContainsKey(name))
            {
                return DiscountMapping.MethodAccessors[name];
            }

            return name;
        }
    }
}