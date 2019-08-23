using commercetools.Sdk.Domain.Subscriptions;

namespace commercetools.Sdk.Serialization
{
    internal class DestinationConverter : JsonConverterDecoratorTypeRetrieverBase<Destination>
    {
        public override string PropertyName => "type";

        public DestinationConverter(IDecoratorTypeRetriever<Destination> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}
