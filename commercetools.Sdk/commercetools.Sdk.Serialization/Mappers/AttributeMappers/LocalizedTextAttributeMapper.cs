using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Serialization
{
    internal class LocalizedTextAttributeMapper : LocalizedStringConverter<Attribute, LocalizedString>, ICustomJsonMapper<Attribute>
    {
        public LocalizedTextAttributeMapper(ICultureValidator cultureValidator) : base(cultureValidator)
        {
        }
    }
}
