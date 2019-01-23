using commercetools.Sdk.Domain.Messages;

namespace commercetools.Sdk.Serialization
{
    internal class MessageConverter : JsonConverterDecoratorTypeRetrieverBase<Message>
    {
        public override string PropertyName => "type";

        public MessageConverter(IDecoratorTypeRetriever<Message> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}