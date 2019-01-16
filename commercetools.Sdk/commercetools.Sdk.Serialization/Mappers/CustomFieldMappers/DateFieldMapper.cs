using commercetools.Sdk.Domain;
using System;

namespace commercetools.Sdk.Serialization
{
    internal class DateFieldMapper : DateConverter<Fields, DateTime>, ICustomJsonMapper<Fields>
    {
    }
}