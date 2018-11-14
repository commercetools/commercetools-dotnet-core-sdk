using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class MethodMemberPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        private readonly IAccessorTraverser accessorTraverser;

        public MethodMemberPredicateVisitorConverter(IAccessorTraverser accessorTraverser)
        {
            this.accessorTraverser = accessorTraverser;
        }

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (Mappings.MethodAccessors.ContainsKey(methodCallExpression.Method.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            List<string> accessors = this.accessorTraverser.GetAccessorsForExpression(expression);
            AccessorPredicateVisitor stringPredicateVisitor = new AccessorPredicateVisitor(accessors);
            return stringPredicateVisitor;
        }
    }
}
