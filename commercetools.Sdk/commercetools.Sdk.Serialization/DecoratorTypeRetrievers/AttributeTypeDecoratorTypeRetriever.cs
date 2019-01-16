using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class AttributeTypeDecoratorTypeRetriever : DecoratorTypeRetriever<AttributeType>
    {
        public AttributeTypeDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}