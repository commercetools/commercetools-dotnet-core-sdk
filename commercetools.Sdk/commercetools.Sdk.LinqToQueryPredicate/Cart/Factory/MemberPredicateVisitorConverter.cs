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
            List<string> accessors = this.accessorTraverser.GetAccessorsForExpression(expression);
            AccessorPredicateVisitor stringPredicateVisitor = new AccessorPredicateVisitor(accessors);
            return stringPredicateVisitor;
        }
    }
}
