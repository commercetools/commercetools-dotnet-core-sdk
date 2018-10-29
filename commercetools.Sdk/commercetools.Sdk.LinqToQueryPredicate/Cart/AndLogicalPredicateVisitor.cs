using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class AndLogicalPredicateVisitor : LogicalPredicateVisitor
    {
        public AndLogicalPredicateVisitor(ICartPredicateVisitor left, ICartPredicateVisitor right) : base(left, LogicalOperators.AND, right)
        { 
        }
    }
}
