using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.Projects;

namespace commercetools.Sdk.Serialization
{
    internal class ShippingRateInputTypeDecoratorTypeRetriever : DecoratorTypeRetriever<ShippingRateInputType>
    {
        public ShippingRateInputTypeDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}