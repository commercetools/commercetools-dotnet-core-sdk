using commercetools.Sdk.Domain;
using System;

namespace commercetools.Sdk.Serialization
{
    internal class TimeFieldMapper : TimeConverter<Fields, TimeSpan>, ICustomJsonMapper<Fields>
    {
    }
}