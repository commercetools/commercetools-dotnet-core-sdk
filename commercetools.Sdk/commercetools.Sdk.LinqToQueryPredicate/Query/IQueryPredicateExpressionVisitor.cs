using System.Linq.Expressions;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public interface IQueryPredicateExpressionVisitor
    {
        string ProcessExpression(Expression expression);
    }
}