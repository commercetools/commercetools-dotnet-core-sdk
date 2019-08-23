using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.Subscriptions;

namespace commercetools.Sdk.Serialization
{
    internal class DestinationDecoratorTypeRetriever : DecoratorTypeRetriever<Destination>
    {
        public DestinationDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}
