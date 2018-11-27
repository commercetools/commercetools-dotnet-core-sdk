using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class LocalizedTextAttributeMapper : LocalizedStringConverter<Domain.Attribute, LocalizedString>, ICustomJsonMapper<Domain.Attribute>
    {
    }
}