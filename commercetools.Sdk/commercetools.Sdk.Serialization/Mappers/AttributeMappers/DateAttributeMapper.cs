using System;

namespace commercetools.Sdk.Serialization
{
    public class DateAttributeMapper : DateConverter<Domain.Attribute, DateTime>, ICustomJsonMapper<Domain.Attribute>
    {
    }
}