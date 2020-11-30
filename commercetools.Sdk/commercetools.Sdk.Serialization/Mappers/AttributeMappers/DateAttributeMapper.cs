using System;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Serialization
{
    internal class DateAttributeMapper : DateConverter<Attribute, DateTime>, ICustomJsonMapper<Attribute>
    {
    }
}