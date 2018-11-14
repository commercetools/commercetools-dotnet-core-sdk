using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class SelectMethodPredicateConverter : ICartPredicateVisitorConverter
    {
        private readonly IAccessorTraverser accessorTraverser;
        private readonly List<string> allowedMethodNames = new List<string>() { "Select" };

        public SelectMethodPredicateConverter(IAccessorTraverser accessorTraverser)
        {
            this.accessorTraverser = accessorTraverser;
        }

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
                List<string> accessors = this.accessorTraverser.GetAccessorsForExpression(methodCallExpression.Arguments[1]);
                accessors.AddRange(this.accessorTraverser.GetAccessorsForExpression(methodCallExpression.Arguments[0]));
                AccessorPredicateVisitor stringPredicateVisitor = new AccessorPredicateVisitor(accessors);
                return stringPredicateVisitor;
            }
            throw new NotSupportedException();
        }
    }
}
