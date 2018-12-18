using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class AttributeTypeDecoratorTypeRetriever : DecoratorTypeRetriever<AttributeType>
    {
        public AttributeTypeDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}