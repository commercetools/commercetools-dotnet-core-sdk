using commercetools.Sdk.Domain;
using System;

namespace commercetools.Sdk.Serialization
{
    public class TimeFieldMapper : TimeConverter<Fields, TimeSpan>, ICustomJsonMapper<Fields>
    {
    }
}