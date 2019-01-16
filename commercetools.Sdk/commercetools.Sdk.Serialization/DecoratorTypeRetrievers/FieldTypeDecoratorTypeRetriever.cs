using commercetools.Sdk.Domain;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    internal class FieldTypeDecoratorTypeRetriever : DecoratorTypeRetriever<FieldType>
    {
        public FieldTypeDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}