using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Messages;

namespace commercetools.Sdk.Serialization
{
    internal class MessageDecoratorTypeRetriever : DecoratorTypeRetriever<Message>
    {
        public MessageDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}