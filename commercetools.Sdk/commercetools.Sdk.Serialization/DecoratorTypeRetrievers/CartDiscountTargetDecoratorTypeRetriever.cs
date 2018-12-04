using commercetools.Sdk.Domain;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization
{
    public class CartDiscountTargetDecoratorTypeRetriever : DecoratorTypeRetriever<CartDiscountTarget>
    {
        public CartDiscountTargetDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}