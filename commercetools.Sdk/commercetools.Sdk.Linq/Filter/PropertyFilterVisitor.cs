using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class PropertyFilterVisitor : FilterVisitor
    {
        public PropertyFilterVisitor(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException();
            }
            this.Accessors = AccessorTraverser.GetAccessors(expression);
        }

        public override string Render()
        {
            return $"{AccessorTraverser.RenderAccessors(this.Accessors)}";
        }

        public override string RenderValue()
        {
            return string.Empty;
        }
    }
}
