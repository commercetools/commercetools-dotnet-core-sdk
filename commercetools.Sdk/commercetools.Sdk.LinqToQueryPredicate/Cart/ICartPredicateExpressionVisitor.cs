using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface ICartPredicateExpressionVisitor
    {
        string Render(Expression expression);
    }
}