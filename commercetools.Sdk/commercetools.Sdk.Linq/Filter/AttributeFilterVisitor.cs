using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public class AttributeFilterVisitor : FilterVisitor
    {
        private string Name;
        private string Value;

        public AttributeFilterVisitor(MethodCallExpression expression)
        {
            this.Accessors = AccessorTraverser.GetAccessors(expression.Arguments[0]);
            var innerExpression = expression.Arguments[1];
            if (innerExpression is LambdaExpression lambdaExpression)
            {
                var attributeExpression = lambdaExpression.Body;
                if (attributeExpression.NodeType == ExpressionType.Equal)
                {
                    SetName(attributeExpression);
                }
                if (attributeExpression.NodeType == ExpressionType.And || attributeExpression.NodeType == ExpressionType.AndAlso)
                {
                    SetName(((BinaryExpression)attributeExpression).Left);
                    SetValue(((BinaryExpression)attributeExpression).Right);
                }
            }
        }

        private void SetName(Expression expression)
        {
            if (expression is BinaryExpression nameExpression)
            {
                if (nameExpression.Left is MemberExpression memberExpression && memberExpression.Member.Name == "Name")
                {
                    this.Name = nameExpression.Right.ToString().Replace("\"", "");
                    this.Accessors.Add(this.Name);
                }
            }
        }

        private void SetValue(Expression expression)
        {
            FilterVisitorFactory filterVisitorFactory = new FilterVisitorFactory();
            FilterVisitor filterVisitor = filterVisitorFactory.Create(expression);
            this.Value = filterVisitor.RenderValue();
            this.Accessors.AddRange(filterVisitor.Accessors);
        }

        public override string Render()
        {
            return $"{AccessorTraverser.RenderAccessors(this.Accessors)}:{this.RenderValue()}";
        }

        public override string RenderValue()
        {
            return $"{this.Value}";
        }
    }
}