using System.Linq.Expressions;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public interface ISortExpressionVisitor
    {
        string GetPath(Expression expression);
    }
}