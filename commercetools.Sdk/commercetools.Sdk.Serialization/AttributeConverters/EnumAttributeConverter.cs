using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.Serialization
{
    public class EnumAttributeConverter : ICustomConverter<Domain.Attribute>
    {
        public int Priority => 3;

        public Type Type => typeof(EnumAttribute);

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
