using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Serialization
{
    public class LocalizedTextAttributeMapper : LocalizedStringConverter<Domain.Attribute, LocalizedString>, ICustomJsonMapper<Domain.Attribute>
    {
        public LocalizedTextAttributeMapper(ICultureValidator cultureValidator) : base(cultureValidator)
        {
        }
    }
}