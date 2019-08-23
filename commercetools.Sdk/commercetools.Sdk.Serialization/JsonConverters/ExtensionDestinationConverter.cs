using commercetools.Sdk.Domain.APIExtensions;

namespace commercetools.Sdk.Serialization
{
    internal class ExtensionDestinationConverter : JsonConverterDecoratorTypeRetrieverBase<Destination>
    {
        public override string PropertyName => "type";

        public ExtensionDestinationConverter(IDecoratorTypeRetriever<Destination> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}
