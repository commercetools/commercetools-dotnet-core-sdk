using System.Linq.Expressions;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public interface IFilterExpressionVisitor
    {
        string Render(Expression expression);
    }
}