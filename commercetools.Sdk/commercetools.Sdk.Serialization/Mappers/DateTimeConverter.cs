﻿using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class DateTimeConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 4;

        public Type Type => typeof(S);

        public virtual bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.Date)
            {
                return true;
            }
            return false;
        }
    }
}