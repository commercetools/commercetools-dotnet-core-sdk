using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class ComparisonMethodsPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                return Mapping.ComparisonMethodOperators.ContainsKey(methodCallExpression.Method.Name);
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

            string operatorSign = Mapping.ComparisonMethodOperators[methodCallExpression.Method.Name];
            IPredicateVisitor left = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            IPredicateVisitor right = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);

            BinaryPredicateVisitor binary = new BinaryPredicateVisitor(left, operatorSign, right);
            return binary;
        }
    }
}
