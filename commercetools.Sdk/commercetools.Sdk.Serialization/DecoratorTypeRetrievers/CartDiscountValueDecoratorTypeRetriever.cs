using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;

namespace commercetools.Sdk.Serialization
{
    internal class CartDiscountValueDecoratorTypeRetriever : DecoratorTypeRetriever<CartDiscountValue>
    {
        public CartDiscountValueDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}