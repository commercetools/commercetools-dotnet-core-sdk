using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface IFilterExpressionVisitor
    {
        string Render(Expression expression);
    }
}