using commercetools.Sdk.Domain;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization
{
    public class AttributeTypeDecoratorTypeRetriever : DecoratorTypeRetriever<AttributeType>
    {
        public AttributeTypeDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}