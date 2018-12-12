using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Serialization
{
    public class LocalizedEnumAttributeMapper : LocalizedEnumConverter<Attribute, LocalizedEnumValue>, ICustomJsonMapper<Attribute>
    {
        public LocalizedEnumAttributeMapper(ICultureValidator cultureValidator) : base(cultureValidator)
        {
        }
    }
}