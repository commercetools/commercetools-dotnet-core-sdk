using System;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class DateTimeCustomMethodPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (DiscountMapping.DateTimeCustomMethods.Contains(methodCallExpression.Method.Name))
                {
                    return true;
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return null;
            }

            if (methodCallExpression.Arguments[0] == null)
            {
                throw new NotSupportedException();
            }

            IPredicateVisitor innerPredicateVisitor = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);

            var methodName = methodCallExpression.Method.Name;
            var dynamicInvoke = Expression.Lambda(methodCallExpression.Arguments[0], null).Compile().DynamicInvoke(null);
            if (dynamicInvoke is DateTime dateTime)
            {
                return methodName == "AsDate" ?
                    new ConstantPredicateVisitor(dateTime.ToUtcIso8601(true).WrapInQuotes()) :
                    new ConstantPredicateVisitor(dateTime.ToUtcIso8601().WrapInQuotes());
            }

            if (dynamicInvoke is TimeSpan timeSpan && methodName == "AsTime")
            {
                return new ConstantPredicateVisitor(timeSpan.ToIso8601().WrapInQuotes());
            }

            return new ConstantPredicateVisitor(innerPredicateVisitor.Render().WrapInQuotes());
        }
    }
}