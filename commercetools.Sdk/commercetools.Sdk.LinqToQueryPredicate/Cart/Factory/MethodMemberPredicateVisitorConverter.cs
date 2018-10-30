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
            string accessor = this.accessorTraverser.Render(expression);
            StringPredicateVisitor stringPredicateVisitor = new StringPredicateVisitor(accessor);
            return stringPredicateVisitor;
        }
    }
}
