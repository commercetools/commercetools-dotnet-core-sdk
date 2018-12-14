using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface IPredicateExpressionVisitor
    {
        string Render(Expression expression);
    }
}