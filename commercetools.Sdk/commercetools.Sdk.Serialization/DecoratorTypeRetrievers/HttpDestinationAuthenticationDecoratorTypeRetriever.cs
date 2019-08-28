using commercetools.Sdk.Domain.APIExtensions;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    internal class HttpDestinationAuthenticationDecoratorTypeRetriever : DecoratorTypeRetriever<HttpDestinationAuthentication>
    {
        public HttpDestinationAuthenticationDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}
