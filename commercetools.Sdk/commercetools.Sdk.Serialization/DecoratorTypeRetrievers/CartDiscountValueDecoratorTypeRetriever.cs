using commercetools.Sdk.Domain;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization
{
    public class CartDiscountValueDecoratorTypeRetriever : DecoratorTypeRetriever<CartDiscountValue>
    {
        public CartDiscountValueDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}