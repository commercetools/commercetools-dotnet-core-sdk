using commercetools.Sdk.Domain.Subscriptions;

namespace commercetools.Sdk.Serialization
{
    internal class SubscriptionFormatConverter : JsonConverterDecoratorTypeRetrieverBase<Format>
    {
        public override string PropertyName => "type";

        public SubscriptionFormatConverter(IDecoratorTypeRetriever<Format> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}
