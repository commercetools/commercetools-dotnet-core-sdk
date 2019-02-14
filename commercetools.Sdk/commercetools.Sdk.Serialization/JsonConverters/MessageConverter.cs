using System;
using commercetools.Sdk.Domain.Messages;

namespace commercetools.Sdk.Serialization
{
    internal class MessageConverter : JsonConverterDecoratorTypeRetrieverBase<Message>
    {
        public override string PropertyName => "type";

        public override Type DefaultType => typeof(GeneralMessage);

        public MessageConverter(IDecoratorTypeRetriever<Message> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}
