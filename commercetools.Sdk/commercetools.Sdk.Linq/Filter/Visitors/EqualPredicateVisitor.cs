using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq.Filter.Visitors
{
    public class EqualPredicateVisitor : IPredicateVisitor
    {
        public EqualPredicateVisitor(IPredicateVisitor left, IPredicateVisitor right)
        {
            this.Left = left;
            this.Right = right;
        }

        public IPredicateVisitor Left { get; }

        public IPredicateVisitor Right { get; }

        public string Render()
        {
            return $"{this.Left?.Render()}:{this.Right?.Render()}";
        }
    }
}
