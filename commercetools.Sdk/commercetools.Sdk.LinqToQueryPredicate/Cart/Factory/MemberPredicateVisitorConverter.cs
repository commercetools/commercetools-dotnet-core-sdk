using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class MemberPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        private readonly IAccessorTraverser accessorTraverser;

        public MemberPredicateVisitorConverter(IAccessorTraverser accessorTraverser)
        {
            this.accessorTraverser = accessorTraverser;
        }

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.MemberAccess;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            string accessor = this.accessorTraverser.Render(expression);
            StringPredicateVisitor stringPredicateVisitor = new StringPredicateVisitor(accessor);
            return stringPredicateVisitor;
        }
    }
}
