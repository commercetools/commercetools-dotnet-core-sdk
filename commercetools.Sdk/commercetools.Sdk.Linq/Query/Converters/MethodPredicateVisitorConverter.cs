using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class MethodPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (Mapping.AllowedMethods.ContainsKey(methodCallExpression.Method.Name))
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

            string methodName = Mapping.AllowedMethods[methodCallExpression.Method.Name];
            IPredicateVisitor methodCaller = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);

            // c.Key.In("c14", "c15")
            if (methodCallExpression.Arguments.Count > 1)
            {
                IPredicateVisitor methodArguments = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);
                return new BinaryPredicateVisitor(methodCaller, methodName, methodArguments);
            }

            // c.Key.IsDefined()
            return new BinaryPredicateVisitor(methodCaller, methodName, new ConstantPredicateVisitor(string.Empty));
        }
    }
}
