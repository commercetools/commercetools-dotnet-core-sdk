using commercetools.Sdk.Domain;
using System;

namespace commercetools.Sdk.Serialization
{
    internal class DateTimeFieldMapper : DateTimeConverter<Fields, DateTime>, ICustomJsonMapper<Fields>
    {
    }
}