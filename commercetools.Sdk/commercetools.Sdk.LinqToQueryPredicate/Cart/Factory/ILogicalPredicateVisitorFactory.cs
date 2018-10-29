using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public interface ILogicalPredicateVisitorFactory
    {
        LogicalPredicateVisitor Create(BinaryExpression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory);
    }
}
