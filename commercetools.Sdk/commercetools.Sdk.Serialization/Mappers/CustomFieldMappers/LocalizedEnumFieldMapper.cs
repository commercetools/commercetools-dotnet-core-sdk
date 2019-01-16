using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Validation;

namespace commercetools.Sdk.Serialization
{
    internal class LocalizedEnumFieldMapper : LocalizedEnumConverter<Fields, LocalizedEnumValue>, ICustomJsonMapper<Fields>
    {
        public LocalizedEnumFieldMapper(ICultureValidator cultureValidator) : base(cultureValidator)
        {
        }
    }
}