using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class MixedGroupFilterVisitor : GroupFilterVisitor<FilterVisitor>
    {
        public MixedGroupFilterVisitor(List<Expression> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException();
            }
            foreach(var expression in expressions)
            {
                FilterVisitorFactory filterVisitorFactory = new FilterVisitorFactory();
                this.innerFilters.Add(filterVisitorFactory.Create(expression));
            }
            this.Accessors = this.innerFilters[0].Accessors;
        }

        public override string Render()
        {
            return $"{AccessorTraverser.RenderAccessors(this.Accessors)}:{this.RenderValue()}";
        }

        public override string RenderValue()
        {
            // Replace double space with single space (happens when subtree is inside of the list)
            return $"{string.Join(", ", this.innerFilters.Select(x => x.RenderValue())).Replace("  ", " ")}";
        }
    }
}
