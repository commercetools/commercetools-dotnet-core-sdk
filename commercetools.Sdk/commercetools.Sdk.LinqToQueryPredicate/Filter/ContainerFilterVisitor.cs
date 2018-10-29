using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class ContainerFilterVisitor : FilterVisitor
    {
        private FilterVisitor innerFilterVisitor; 

        public ContainerFilterVisitor(MethodCallExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("The expression does not have all expected properties set.");
            }
            FilterVisitorFactory filterVisitorFactory = new FilterVisitorFactory();
            innerFilterVisitor = filterVisitorFactory.Create(expression.Arguments[1]);
            this.Accessors = AccessorTraverser.GetAccessors(expression.Arguments[0]);
        }

        public override string Render()
        {
            string result = $"{AccessorTraverser.RenderAccessors(this.Accessors)}";
            string innerValue = this.innerFilterVisitor.Render();
            if (!string.IsNullOrEmpty(innerValue))
            {
                result += $".{innerValue}";
            }
            return result;
        }

        public override string RenderValue()
        {
            return $"{this.innerFilterVisitor.Render()}";
        }
    }
}
