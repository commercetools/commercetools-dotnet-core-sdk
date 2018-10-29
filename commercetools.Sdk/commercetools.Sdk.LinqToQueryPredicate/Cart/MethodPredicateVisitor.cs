using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class MethodPredicateVisitor : ICartPredicateVisitor
    {
        private string method;
        private ICartPredicateVisitor innerPredicate;

        public MethodPredicateVisitor(string method, ICartPredicateVisitor innerPredicate)
        {
            this.method = method;
            this.innerPredicate = innerPredicate;
        }

        public string Render()
        {
            return $"{this.method}({this.innerPredicate.Render()})";
        }
    }
}
