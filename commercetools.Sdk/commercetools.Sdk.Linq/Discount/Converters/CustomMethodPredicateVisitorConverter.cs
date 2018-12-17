using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class CustomMethodPredicateVisitorConverter : IDiscountPredicateVisitorConverter
    {
        public int Priority => 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (DiscountMapping.CustomMethods.Contains(methodCallExpression.Method.Name))
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

            if (methodCallExpression.Arguments[1] == null)
            {
                throw new NotSupportedException();
            }

            IPredicateVisitor innerPredicateVisitor = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);
            MethodPredicateVisitor methodPredicateVisitor = new MethodPredicateVisitor(methodCallExpression.Method.Name, innerPredicateVisitor);
            return methodPredicateVisitor;
        }
    }
}