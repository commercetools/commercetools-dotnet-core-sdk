using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.Serialization
{
    public class LocalizedTextAttributeConverter : ICustomConverter<Domain.Attribute>
    {
        public int Priority => 4;

        public Type Type => typeof(LocalizedTextAttribute);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                // Checks only the first key value pair so that we can exclude this type from deserialization
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
