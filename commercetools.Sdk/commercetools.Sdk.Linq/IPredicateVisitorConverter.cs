using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public interface IPredicateVisitorConverter
    {
        bool CanConvert(Expression expression);

        IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory);
    }
}