using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public interface ISimplePredicateVisitorFactory
    {
        SimplePredicateVisitor Create(BinaryExpression expression);
    }
}
