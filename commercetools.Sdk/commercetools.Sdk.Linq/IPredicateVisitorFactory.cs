using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface IPredicateVisitorFactory
    {
        IPredicateVisitor Create(Expression expression);
    }
}