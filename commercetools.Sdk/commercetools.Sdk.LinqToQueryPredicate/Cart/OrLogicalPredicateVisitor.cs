using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class OrLogicalPredicateVisitor : LogicalPredicateVisitor
    {
        public OrLogicalPredicateVisitor(ICartPredicateVisitor left, ICartPredicateVisitor right) : base(left, LogicalOperators.OR, right)
        { 
        }
    }
}
