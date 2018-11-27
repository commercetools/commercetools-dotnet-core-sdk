using commercetools.Sdk.Domain;
using System;

namespace commercetools.Sdk.Serialization
{
    public class DateTimeFieldMapper : DateTimeConverter<Fields, DateTime>, ICustomJsonMapper<Fields>
    {
    }
}