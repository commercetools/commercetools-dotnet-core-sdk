using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class AttributePredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (IsMethodNameAllowed(methodCallExpression) && IsValidMethodCaller(methodCallExpression))
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

            IPredicateVisitor inner = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);
            IPredicateVisitor attribute = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
            return new ContainerPredicateVisitor(inner, attribute);
        }

        private static bool IsMethodNameAllowed(MethodCallExpression expression)
        {
            return expression.Method.Name == "Any";
        }

        private static bool IsValidMethodCaller(MethodCallExpression expression)
        {
            Expression callerExpression = expression.Arguments[0];
            if (callerExpression is MemberExpression memberExpression)
            {
                if (memberExpression.Member.Name == "Attributes")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
