using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public abstract class FilterVisitor
    {
        public List<string> Accessors { get; protected set; }
        public abstract string Render();
            
    }
}
