using System.Linq.Expressions;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public interface IFacetExpressionVisitor
    {
        string Render(Expression expression);
    }
}