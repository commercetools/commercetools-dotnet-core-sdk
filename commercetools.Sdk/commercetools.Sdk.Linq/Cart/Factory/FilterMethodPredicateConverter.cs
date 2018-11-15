using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class SelectMethodPredicateConverter : ICartPredicateVisitorConverter
    {
        private readonly List<string> allowedMethodNames = new List<string>() { "Select" };

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
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new ArgumentException();
            }
            if (methodCallExpression.Arguments.Count >= 2)
            {
                ICartPredicateVisitor inside = cartPredicateVisitorFactory.Create(methodCallExpression.Arguments[1]);
                ICartPredicateVisitor parent = cartPredicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                Accessor stringPredicateVisitor = new Accessor(inside, parent);
                return stringPredicateVisitor;
            }
            throw new NotSupportedException();
        }
    }
}
