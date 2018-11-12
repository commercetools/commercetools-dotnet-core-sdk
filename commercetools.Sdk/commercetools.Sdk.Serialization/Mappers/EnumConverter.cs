using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class EnumConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 3;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                var keyProperty = property["key"];
                var labelProperty = property["label"];
                if (keyProperty != null && labelProperty != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
