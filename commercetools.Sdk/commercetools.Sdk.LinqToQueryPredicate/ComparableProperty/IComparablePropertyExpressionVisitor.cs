using System.Linq.Expressions;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public interface IComparablePropertyExpressionVisitor
    {
        string Render(Expression expression);
    }
}