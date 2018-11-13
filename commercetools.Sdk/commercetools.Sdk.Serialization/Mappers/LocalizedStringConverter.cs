using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class LocalizedStringConverter<T, S> : ICustomJsonMapper<T>
    {
        public int Priority => 4;

        public Type Type => typeof(S);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                // Checks only the first key value pair; the assumption is all of them are of the same type
                var firstValue = property.First as JProperty;
                var name = firstValue?.Name;
                if (name != null && name.IsValidLanguageTag())
                { 
                    return true;
                }
            }
            return false;
        }
    }
}
