using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.Serialization
{
    public class LocalizedEnumAttributeConverter : ICustomConverter<Domain.Attribute>
    {
        public int Priority => 2;

        public Type Type => typeof(LocalizedEnumAttribute);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                var keyProperty = property["key"];
                if (keyProperty != null)
                {
                    var labelProperty = property["label"];
                    if (labelProperty != null && labelProperty.HasValues)
                    {
                        // Checks only the first key value pair so that we can exclude this type from deserialization
                        var firstValue = labelProperty.First as JProperty;
                        var name = firstValue?.Name;
                        if (name != null && name.IsValidLanguageTag())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
