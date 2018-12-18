using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Serialization
{
    public class ShippingRateInputDecoratorTypeRetriever : DecoratorTypeRetriever<IShippingRateInput>
    {
        public ShippingRateInputDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}