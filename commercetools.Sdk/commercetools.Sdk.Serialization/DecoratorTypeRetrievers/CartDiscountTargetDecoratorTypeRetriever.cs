using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;

namespace commercetools.Sdk.Serialization
{
    internal class CartDiscountTargetDecoratorTypeRetriever : DecoratorTypeRetriever<CartDiscountTarget>
    {
        public CartDiscountTargetDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}