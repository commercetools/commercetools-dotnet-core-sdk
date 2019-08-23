using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.Subscriptions;

namespace commercetools.Sdk.Serialization
{
    internal class SubscriptionDestinationDecoratorTypeRetriever : DecoratorTypeRetriever<Destination>
    {
        public SubscriptionDestinationDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}
