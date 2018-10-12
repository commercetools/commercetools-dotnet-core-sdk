using System.Linq.Expressions;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public interface IExpansionVisitor
    {
        string GetPath(Expression expression);
    }
}