using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Carts;

namespace commercetools.Sdk.Linq
{
    public class MoneyParsePredicateConverter : ICartPredicateVisitorConverter
    {
        private readonly string methodName = "Parse";

        // TODO Find a better way how to match Type
        private readonly string methodCallerType = "commercetools.Sdk.Domain.Money";

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (IsMatchingMethodName(methodCallExpression) && IsMatchingType(methodCallExpression))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsMatchingMethodName(MethodCallExpression methodCallExpression)
        {
            return this.methodName == methodCallExpression.Method.Name;
        }

        private bool IsMatchingType(MethodCallExpression methodCallExpression)
        {
            return methodCallExpression.Type.ToString() == methodCallerType;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new ArgumentException();
            }
            Expression argument = methodCallExpression.Arguments[0];
            if (argument != null && argument.NodeType == ExpressionType.Constant)
            {
                return new ConstantPredicateVisitor($"\"{((ConstantExpression)argument).Value.ToString()}\"");
            }
            throw new NotSupportedException();
        }
    }
}