using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.OrderEdits;

namespace commercetools.Sdk.Serialization
{
    internal class OrderEditResultDecoratorTypeRetriever : DecoratorTypeRetriever<OrderEditResult>
    {
        public OrderEditResultDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}