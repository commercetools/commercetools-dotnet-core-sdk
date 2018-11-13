using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class NotLogicalPredicateVisitor : ICartPredicateVisitor
    {
        private readonly ICartPredicateVisitor innerPredicate;

        public NotLogicalPredicateVisitor(ICartPredicateVisitor innerPredicate)
        {
            this.innerPredicate = innerPredicate;
        }

        public string Render()
        {
            return $"{LogicalOperators.NOT}({this.innerPredicate.Render()})";
        }
    }
}
