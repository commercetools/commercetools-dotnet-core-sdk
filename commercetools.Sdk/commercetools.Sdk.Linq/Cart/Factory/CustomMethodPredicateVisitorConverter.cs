using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class CustomMethodPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        // TODO Try to inject these method names
        private readonly IEnumerable<string> allowedMethods = new List<string>()
        {
            "LineItemCount",
            "CustomLineItemCount",
            "LineItemTotal",
            "CustomLineItemTotal",
            "LineItemNetTotal",
            "CustomLineItemNetTotal",
            "LineItemGrossTotal",
            "CustomLineItemGrossTotal",
            "LineItemExists",
            "ForAllLineItems"
        };

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (this.allowedMethods.Contains(methodCallExpression.Method.Name))
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
            if (methodCallExpression.Arguments[1] == null)
            {
                throw new NotSupportedException();
            }
            ICartPredicateVisitor innerPredicateVisitor = cartPredicateVisitorFactory.Create(methodCallExpression.Arguments[1]);
            MethodPredicateVisitor methodPredicateVisitor = new MethodPredicateVisitor(methodCallExpression.Method.Name, innerPredicateVisitor);
            return methodPredicateVisitor;
        }
    }
}
