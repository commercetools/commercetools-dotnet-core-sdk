using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Discount.Visitors;

namespace commercetools.Sdk.Linq.Discount.Converters
{
    public class SelectMethodPredicateConverter : IDiscountPredicateVisitorConverter
    {
        private readonly List<string> allowedMethodNames = new List<string>() { "Select" };

        public int Priority => 4;

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

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return null;
            }

            if (methodCallExpression.Arguments.Count >= 2)
            {
                IPredicateVisitor inside = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);
                IPredicateVisitor parent = predicateVisitorFactory.Create(methodCallExpression.Arguments[0]);
                AccessorPredicateVisitor stringPredicateVisitor = new AccessorPredicateVisitor(inside, parent);
                return stringPredicateVisitor;
            }

            throw new NotSupportedException();
        }
    }
}