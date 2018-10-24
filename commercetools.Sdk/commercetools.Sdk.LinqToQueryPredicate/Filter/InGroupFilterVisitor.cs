using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class InGroupFilterVisitor : FilterVisitor
    {
        private List<string> channels = new List<string>();
        private List<string> allowedMethodNames = new List<string>() { "IsOnStockInChannels" };

        public InGroupFilterVisitor(MethodCallExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException();
            }
            this.Accessors = AccessorTraverser.GetAccessors(expression.Arguments[0]);
            if (allowedMethodNames.Contains(expression.Method.Name))
            { 
                this.Accessors.Add(expression.Method.Name.ToCamelCase());
            }
            if (expression.Arguments[1].NodeType == ExpressionType.NewArrayInit)
            {
                foreach (Expression part in ((NewArrayExpression)expression.Arguments[1]).Expressions)
                {
                    if (part.NodeType == ExpressionType.Constant)
                    {
                        channels.Add(part.ToString());
                    }
                }
            }
        }

        public override string Render()
        {
            return $"{AccessorTraverser.RenderAccessors(this.Accessors)}:{this.RenderValue()}";
        }

        public override string RenderValue()
        {
            return string.Join(",", channels);
        }
    }
}
