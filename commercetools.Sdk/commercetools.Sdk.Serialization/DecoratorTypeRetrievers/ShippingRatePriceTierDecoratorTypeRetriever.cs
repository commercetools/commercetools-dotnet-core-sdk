using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Serialization
{
    internal class ShippingRatePriceTierDecoratorTypeRetriever : DecoratorTypeRetriever<ShippingRatePriceTier>
    {
        public ShippingRatePriceTierDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}