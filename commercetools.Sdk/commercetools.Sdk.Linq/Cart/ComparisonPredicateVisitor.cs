using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class ComparisonPredicateVisitor : ICartPredicateVisitor
    {
        private ICartPredicateVisitor left;
        private ICartPredicateVisitor right;
        private string operatorSign; 

        public ComparisonPredicateVisitor(ICartPredicateVisitor left, string operatorSign, ICartPredicateVisitor right)
        {
            this.left = left;
            this.right = right;
            this.operatorSign = operatorSign;
        }

        public string Render()
        {
            return $"{left.Render()} {operatorSign} {right.Render()}";
        }
    }
}
