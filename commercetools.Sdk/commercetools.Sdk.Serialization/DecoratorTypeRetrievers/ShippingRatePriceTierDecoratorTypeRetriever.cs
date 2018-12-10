using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization
{
    public class ShippingRatePriceTierDecoratorTypeRetriever : DecoratorTypeRetriever<ShippingRatePriceTier>
    {
        public ShippingRatePriceTierDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}