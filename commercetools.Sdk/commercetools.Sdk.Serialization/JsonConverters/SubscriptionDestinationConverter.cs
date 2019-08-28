using commercetools.Sdk.Domain.Subscriptions;

namespace commercetools.Sdk.Serialization
{
    internal class SubscriptionDestinationConverter : JsonConverterDecoratorTypeRetrieverBase<Destination>
    {
        public override string PropertyName => "type";

        public SubscriptionDestinationConverter(IDecoratorTypeRetriever<Destination> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}
