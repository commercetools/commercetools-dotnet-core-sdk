using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class MethodFilterVisitor : FilterVisitor
    {
        private string method;

        public MethodFilterVisitor(MethodCallExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("The expression does not have all expected properties set.");
            }
            this.method = expression.Method.Name.ToCamelCase();
            if (expression.Arguments[0] is MethodCallExpression methodCallExpression)
            {
                FilterVisitorFactory filterVisitorFactory = new FilterVisitorFactory();
                this.Accessors = filterVisitorFactory.Create(methodCallExpression).Accessors;
            }
            else
            { 
                this.Accessors = AccessorTraverser.GetAccessors(expression.Arguments[0]);
            }
        }

        public override string Render()
        {
            return $"{AccessorTraverser.RenderAccessors(this.Accessors)}:{this.RenderValue()}";
        }

        public override string RenderValue()
        {
            return $"{this.method}";
        }
    }
}
