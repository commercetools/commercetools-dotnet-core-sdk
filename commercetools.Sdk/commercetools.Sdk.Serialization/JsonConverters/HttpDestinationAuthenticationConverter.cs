using commercetools.Sdk.Domain.APIExtensions;

namespace commercetools.Sdk.Serialization
{
    internal class HttpDestinationAuthenticationConverter : JsonConverterDecoratorTypeRetrieverBase<HttpDestinationAuthentication>
    {
        public override string PropertyName => "type";

        public HttpDestinationAuthenticationConverter(IDecoratorTypeRetriever<HttpDestinationAuthentication> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}
