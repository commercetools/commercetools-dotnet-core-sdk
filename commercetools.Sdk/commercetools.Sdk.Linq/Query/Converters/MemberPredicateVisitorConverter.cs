using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class MemberPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.MemberAccess;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MemberExpression memberExpression = expression as MemberExpression;
            if (memberExpression == null)
            {
                return null;
            }

            string currentName = memberExpression.Member.Name;
            ConstantPredicateVisitor constant = new ConstantPredicateVisitor(currentName);
            IPredicateVisitor inner = predicateVisitorFactory.Create(memberExpression.Expression);
            return new ContainerPredicateVisitor(constant, inner);
        }
    }
}
