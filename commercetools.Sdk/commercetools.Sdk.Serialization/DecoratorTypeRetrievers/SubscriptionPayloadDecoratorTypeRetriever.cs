using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.Subscriptions;

namespace commercetools.Sdk.Serialization
{
    internal class SubscriptionPayloadDecoratorTypeRetriever : DecoratorTypeRetriever<Payload>
    {
        public SubscriptionPayloadDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}
