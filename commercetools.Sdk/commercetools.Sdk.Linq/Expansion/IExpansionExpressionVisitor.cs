using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface IExpansionExpressionVisitor
    {
        string GetPath(Expression expression);
    }
}