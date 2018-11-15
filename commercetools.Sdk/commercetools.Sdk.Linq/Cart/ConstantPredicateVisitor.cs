using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class ConstantPredicateVisitor : ICartPredicateVisitor
    {
        private string constant; 

        public ConstantPredicateVisitor(string constant)
        {
            this.constant = constant;
        }

        public void AppendAccessor(Accessor accessor)
        {

        }

        public string Render()
        {
            return this.constant.ToCamelCase();
        }
    }
}
