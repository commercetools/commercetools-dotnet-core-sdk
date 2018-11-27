using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface ICartPredicateVisitorConverter
    {
        bool CanConvert(Expression expression);

        ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory);
    }
}