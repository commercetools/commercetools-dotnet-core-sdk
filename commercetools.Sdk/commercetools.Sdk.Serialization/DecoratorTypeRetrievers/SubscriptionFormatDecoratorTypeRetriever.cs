using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.Subscriptions;

namespace commercetools.Sdk.Serialization
{
    internal class SubscriptionFormatDecoratorTypeRetriever : DecoratorTypeRetriever<Format>
    {
        public SubscriptionFormatDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}
