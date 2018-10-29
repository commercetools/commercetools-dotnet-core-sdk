using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class SimplePredicateVisitor : ICartPredicateVisitor
    {
        private string left;
        private string right;
        private string operatorSign; 

        public SimplePredicateVisitor(string left, string right, string operatorSign)
        {
            this.left = left;
            this.right = right;
            this.operatorSign = operatorSign;
        }

        public string Render()
        {
            return $"{left} {right} {operatorSign}";
        }
    }
}
