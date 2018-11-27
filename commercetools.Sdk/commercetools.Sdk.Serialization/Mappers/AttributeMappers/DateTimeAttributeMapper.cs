using System;

namespace commercetools.Sdk.Serialization
{
    public class DateTimeAttributeMapper : DateTimeConverter<Domain.Attribute, DateTime>, ICustomJsonMapper<Domain.Attribute>
    {
    }
}