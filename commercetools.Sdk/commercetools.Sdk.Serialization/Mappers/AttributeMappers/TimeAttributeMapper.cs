using System;

namespace commercetools.Sdk.Serialization
{
    public class TimeAttributeMapper : TimeConverter<Domain.Attribute, TimeSpan>, ICustomJsonMapper<Domain.Attribute>
    {
    }
}