using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    // TODO rename to skip
    public class ConverterMethodsPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        private readonly List<string> allowedMethodNames = new List<string>() { "ToString", "ToMoney" };

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (this.allowedMethodNames.Contains(methodCallExpression.Method.Name))
                {
                    return true;
                }
            }
            if (expression.NodeType == ExpressionType.Convert)
            {
                return true;
            }
            return false;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Object != null)
                { 
                    return cartPredicateVisitorFactory.Create(methodCallExpression.Object);
                }
                else
                {
                    return cartPredicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                }
            }
            if (expression.NodeType == ExpressionType.Convert)
            {
                UnaryExpression unaryExpression = expression as UnaryExpression;
                return cartPredicateVisitorFactory.Create(unaryExpression.Operand);
            }
            throw new NotSupportedException();
        }
    }
}
