using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface ICartPredicateVisitorFactory
    {
        ICartPredicateVisitor Create(Expression expression);
    }
}