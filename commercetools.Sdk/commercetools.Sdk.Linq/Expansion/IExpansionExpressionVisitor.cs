using System.Linq.Expressions;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public interface IExpansionExpressionVisitor
    {
        string GetPath(Expression expression);
    }
}