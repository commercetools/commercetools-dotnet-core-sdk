using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface ISortExpressionVisitor
    {
        string Render(Expression expression);
    }
}