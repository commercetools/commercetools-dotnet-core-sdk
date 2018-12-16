using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class ParsePredicateCoParsePredicateVisitorConverternverter : IFilterPredicateVisitorConverter
    {
        public int Priority => 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (IsMatchingMethodName(methodCallExpression))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsMatchingMethodName(MethodCallExpression methodCallExpression)
        {
            return Mapping.ParseMethods.Contains(methodCallExpression.Method.Name);
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return null;
            }

            return predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
        }
    }
}