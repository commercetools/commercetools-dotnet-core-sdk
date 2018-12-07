using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Serialization
{
    public class LocalizedEnumAttributeMapper : LocalizedEnumConverter<Domain.Attribute, Domain.Attributes.LocalizedEnumValue>, ICustomJsonMapper<Domain.Attribute>
    {
        public LocalizedEnumAttributeMapper(ICultureValidator cultureValidator) : base(cultureValidator)
        {
        }
    }
}