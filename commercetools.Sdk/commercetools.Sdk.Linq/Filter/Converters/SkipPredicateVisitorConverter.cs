using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class SkipPredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name.StartsWith(Mapping.CastMethodStart, StringComparison.InvariantCulture) || methodCallExpression.Method.Name == "FirstOrDefault")
                {
                    return true;
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;

            if (methodCallExpression?.Object != null)
            {
                return predicateVisitorFactory.Create(methodCallExpression.Object);
            }

            if (methodCallExpression?.Arguments[0] != null)
            {
                return predicateVisitorFactory.Create(methodCallExpression?.Arguments[0]);
            }

            return null;

        }
    }
}
