using commercetools.Sdk.Domain;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization.DecoratorTypeRetrievers
{
    public class FieldTypeDecoratorTypeRetriever : DecoratorTypeRetriever<FieldType, FieldTypeAttribute>
    {
        public FieldTypeDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}