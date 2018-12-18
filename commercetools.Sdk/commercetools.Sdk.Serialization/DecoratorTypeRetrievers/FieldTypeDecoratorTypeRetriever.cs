using commercetools.Sdk.Domain;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    public class FieldTypeDecoratorTypeRetriever : DecoratorTypeRetriever<FieldType>
    {
        public FieldTypeDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}