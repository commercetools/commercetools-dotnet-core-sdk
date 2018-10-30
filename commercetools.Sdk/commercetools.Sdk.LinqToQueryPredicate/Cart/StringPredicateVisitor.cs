using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class StringPredicateVisitor : ICartPredicateVisitor
    {
        private string value;

        public StringPredicateVisitor(string value)
        {
            this.value = value;
        }

        public string Render()
        {
            return value.ToCamelCase();
        }
    }
}
