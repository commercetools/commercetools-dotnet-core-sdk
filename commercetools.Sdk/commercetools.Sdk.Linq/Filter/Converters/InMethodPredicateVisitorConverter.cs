using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class InMethodPredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "In")
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

            IPredicateVisitor parent = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            List<IPredicateVisitor> parameters = new List<IPredicateVisitor>();
            if (methodCallExpression.Arguments[1].NodeType != ExpressionType.NewArrayInit)
            {
                return null;
            }

            foreach (Expression part in ((NewArrayExpression)methodCallExpression.Arguments[1]).Expressions)
            {
                parameters.Add(predicateVisitorFactory.Create(part));
            }

            CollectionPredicateVisitor collection = new CollectionPredicateVisitor(parameters);
            EqualPredicateVisitor equal = new EqualPredicateVisitor(parent, collection);
            return equal;
        }
    }
}
