using System;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Serialization
{
    public class DateAttributeMapper : DateConverter<Attribute, DateTime>, ICustomJsonMapper<Attribute>
    {
    }
}