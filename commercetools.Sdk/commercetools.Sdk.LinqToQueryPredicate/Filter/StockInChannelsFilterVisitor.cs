using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class StockInChannelsFilterVisitor : FilterVisitor
    {
        private List<string> channels = new List<string>();

        public StockInChannelsFilterVisitor(MethodCallExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException();
            }
            this.Accessors = AccessorTraverser.GetAccessors(expression.Arguments[0]);
            this.Accessors.Add(expression.Method.Name);
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
