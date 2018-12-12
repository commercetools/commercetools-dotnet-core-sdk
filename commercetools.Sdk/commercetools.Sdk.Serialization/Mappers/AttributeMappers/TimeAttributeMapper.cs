using System;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Serialization
{
    public class TimeAttributeMapper : TimeConverter<Attribute, TimeSpan>, ICustomJsonMapper<Attribute>
    {
    }
}