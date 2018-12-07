using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Serialization
{
    public class LocalizedStringFieldMapper : LocalizedStringConverter<Fields, LocalizedString>, ICustomJsonMapper<Fields>
    {
        public LocalizedStringFieldMapper(ICultureValidator cultureValidator) : base(cultureValidator)
        {
        }
    }
}