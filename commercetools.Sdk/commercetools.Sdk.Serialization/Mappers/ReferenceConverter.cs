﻿using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal abstract class ReferenceConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 3;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues && !property.IsArrayToken())
            {
                var typeIdProperty = property["typeId"];
                var idProperty = property["id"];
                if (typeIdProperty != null && idProperty != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}