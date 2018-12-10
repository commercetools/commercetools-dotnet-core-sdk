using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization
{
    public class ShippingRateInputDecoratorTypeRetriever : DecoratorTypeRetriever<IShippingRateInput>
    {
        public ShippingRateInputDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}