using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class CollectionPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.NewArrayInit;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            NewArrayExpression arrayExpression = expression as NewArrayExpression;
            if (arrayExpression == null)
            {
                return null;
            }

            List<IPredicateVisitor> predicateVisitors = new List<IPredicateVisitor>();
            foreach (var innerExpression in arrayExpression.Expressions)
            {
                IPredicateVisitor innerPredicateVisitor = predicateVisitorFactory.Create(innerExpression);
                predicateVisitors.Add(innerPredicateVisitor);
            }

            return new CollectionPredicateVisitor(predicateVisitors);
        }
    }
}
