using commercetools.Sdk.Domain.APIExtensions;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    internal class ExtensionDestinationDecoratorTypeRetriever : DecoratorTypeRetriever<Destination>
    {
        public ExtensionDestinationDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}
