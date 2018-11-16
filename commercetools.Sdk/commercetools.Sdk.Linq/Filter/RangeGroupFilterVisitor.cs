using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class RangeGroupFilterVisitor : GroupFilterVisitor<RangeFilterVisitor>
    {
        public RangeGroupFilterVisitor(List<MethodCallExpression> expressions)
        {
            if (expressions == null && expressions.Count == 0)
            {
                throw new ArgumentNullException("The expression list is null or empty");
            }
            // all accessors are the same
            this.Accessors = AccessorTraverser.GetAccessors(expressions[0].Arguments[0]);
            foreach (var expression in expressions)
            {
                RangeFilterVisitor rangeFilterVisitor = new RangeFilterVisitor(expression);
                this.innerFilters.Add(rangeFilterVisitor);
            }
        }

        public override string Render()
        {
            return $"{AccessorTraverser.RenderAccessors(this.Accessors)}:{this.RenderValue()}";
        }

        public override string RenderValue()
        {
            return $"range {string.Join(", ", this.innerFilters.Select(x => x.Render()))}";
        }
    }
}
