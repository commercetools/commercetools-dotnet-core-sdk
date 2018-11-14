using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class ConverterMethodsPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        private readonly List<string> allowedMethodNames = new List<string>() { "ToString" };

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (this.allowedMethodNames.Contains(methodCallExpression.Method.Name))
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
                return cartPredicateVisitorFactory.Create(methodCallExpression.Object);
            }
            throw new NotSupportedException();
        }
    }
}
