using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class AttributePredicateVisitorConverter : ICartPredicateVisitorConverter
    {
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

        private bool IsMethodNameAllowed(MethodCallExpression expression)
        {
            return expression.Method.Name == "Any";
        }

        private bool IsValidMethodCaller(MethodCallExpression expression)
        {
            Expression callerExpression = expression.Arguments[0];
            if (callerExpression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "Attributes")
                {
                    return true;
                }
            }
            return false;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {

            }
            ICartPredicateVisitor left = this.GetLeft(expression);
            ICartPredicateVisitor right = this.GetRight(expression);
            string operatorSign = this.GetOperatorSign(expression);
            ComparisonPredicateVisitor comparisonPredicateVisitor = new ComparisonPredicateVisitor(left, operatorSign, right);
            return comparisonPredicateVisitor;
        }

        private ICartPredicateVisitor GetLeft(Expression expression)
        {
            string left = string.Empty;
            StringPredicateVisitor stringPredicateVisitor = new StringPredicateVisitor(left);
            return stringPredicateVisitor;
        }

        private ICartPredicateVisitor GetRight(Expression expression)
        {
            string right = string.Empty;
            StringPredicateVisitor stringPredicateVisitor = new StringPredicateVisitor(right);
            return stringPredicateVisitor;
        }

        private string GetOperatorSign(Expression expression)
        {
            return null;
        }
    }
}
