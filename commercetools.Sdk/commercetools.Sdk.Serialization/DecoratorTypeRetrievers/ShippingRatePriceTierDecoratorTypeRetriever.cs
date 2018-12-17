using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Serialization
{
    public class ShippingRatePriceTierDecoratorTypeRetriever : DecoratorTypeRetriever<ShippingRatePriceTier>
    {
        public ShippingRatePriceTierDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}