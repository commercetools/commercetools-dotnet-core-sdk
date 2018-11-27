using commercetools.Sdk.Domain;
using System;

namespace commercetools.Sdk.Serialization
{
    public class DateFieldMapper : DateConverter<Fields, DateTime>, ICustomJsonMapper<Fields>
    {
    }
}