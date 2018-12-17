using System.Linq.Expressions;
using commercetools.Sdk.Linq.Filter.Visitors;

namespace commercetools.Sdk.Linq.Filter.Converters
{
    public class MemberPredicateVisitorConverter : IFilterPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

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
            if (FilterMapping.MembersToSkip.Contains(currentName))
            {
                return predicateVisitorFactory.Create(memberExpression.Expression);
            }

            ConstantPredicateVisitor constant = new ConstantPredicateVisitor(currentName);
            IPredicateVisitor parent = predicateVisitorFactory.Create(memberExpression.Expression);
            return new Visitors.AccessorPredicateVisitor(constant, parent);
        }
    }
}
