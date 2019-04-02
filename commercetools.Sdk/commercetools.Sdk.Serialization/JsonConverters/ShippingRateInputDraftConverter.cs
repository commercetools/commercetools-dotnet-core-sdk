using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Project;

namespace commercetools.Sdk.Serialization
{
    internal class ShippingRateInputDraftConverter : JsonConverterDecoratorTypeRetrieverBase<IShippingRateInputDraft>
    {
        public override string PropertyName => "type";

        public ShippingRateInputDraftConverter(IDecoratorTypeRetriever<IShippingRateInputDraft> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}
