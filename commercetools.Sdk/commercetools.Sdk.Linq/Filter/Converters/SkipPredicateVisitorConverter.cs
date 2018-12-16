using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class SkipPredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method.Name == "Any" || methodCallExpression.Method.Name == "Where" ||
                       methodCallExpression.Method.Name == "Select";
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

            IPredicateVisitor current = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);
            IPredicateVisitor parent = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            return new AccessorPredicateVisitor(current, parent);
        }
    }
}
