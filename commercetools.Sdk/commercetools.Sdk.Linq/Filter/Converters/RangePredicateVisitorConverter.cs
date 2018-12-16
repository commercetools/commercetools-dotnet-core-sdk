using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class RangePredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method.Name == "Range";
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

            IPredicateVisitor from = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);
            IPredicateVisitor to = predicateVisitorFactory.Create(methodCallExpression.Arguments[2]);
            RangePredicateVisitor range = new RangePredicateVisitor(from, to);
            RangeCollectionPredicateVisitor ranges = new RangeCollectionPredicateVisitor(new List<RangePredicateVisitor>() { range });
            IPredicateVisitor parent = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            return new EqualPredicateVisitor(parent, ranges);
        }
    }
}
