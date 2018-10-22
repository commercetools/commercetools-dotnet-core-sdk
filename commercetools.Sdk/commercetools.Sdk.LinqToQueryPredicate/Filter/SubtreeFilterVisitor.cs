using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class SubtreeFilterVisitor : FilterVisitor
    {
        public string Value { get; private set; }

        public SubtreeFilterVisitor(MethodCallExpression expression)
        {
            this.Value = GetValue(expression.Arguments[1]);
            this.Accessors = AccessorTraverser.GetAccessors(expression.Arguments[0]);
        }

        private string GetValue(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("The expression does not have all expected properties set.");
            }
            if (expression.NodeType == ExpressionType.Constant)
            {
                return expression.ToString();
            }
            throw new NotSupportedException("The expression type is not supported.");
        }

        public override string Render()
        {
            return "subtree(" + this.Value + ")";
        }
    }
}
