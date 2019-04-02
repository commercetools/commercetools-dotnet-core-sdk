using commercetools.Sdk.Domain.Project;

namespace commercetools.Sdk.Serialization
{
    internal class ShippingRateInputTypeConverter : JsonConverterDecoratorTypeRetrieverBase<ShippingRateInputType>
    {
        public override string PropertyName => "type";

        public ShippingRateInputTypeConverter(IDecoratorTypeRetriever<ShippingRateInputType> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}
