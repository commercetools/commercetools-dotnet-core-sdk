using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class ConstantPredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.Constant;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            StringPredicateVisitor stringPredicateVisitor = new StringPredicateVisitor(expression.ToString());
            return stringPredicateVisitor;
        }
    }
}
