using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface IQueryPredicateExpressionVisitor
    {
        string ProcessExpression(Expression expression);
    }
}