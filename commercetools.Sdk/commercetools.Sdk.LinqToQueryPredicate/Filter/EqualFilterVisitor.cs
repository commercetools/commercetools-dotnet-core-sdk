using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class EqualFilterVisitor : FilterVisitor
    {
        private string value;

        public EqualFilterVisitor(BinaryExpression expression)
        {
            this.value = GetValue(expression.Right);
            this.Accessors = AccessorTraverser.GetAccessors(expression.Left);
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
            return $"{AccessorTraverser.RenderAccessors(this.Accessors)}:{this.RenderValue()}";
        }

        public override string RenderValue()
        {
            return $"{this.value}";
        }
    }
}
