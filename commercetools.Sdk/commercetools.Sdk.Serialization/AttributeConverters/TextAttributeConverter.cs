using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;

namespace commercetools.Sdk.Serialization
{
    public class TextAttributeConverter : ICustomConverter<Domain.Attribute>
    {
        public int Priority => 4;

        public Type Type => typeof(TextAttribute);

        public bool CanConvert(JToken property)
        {
            if (property?.Type == JTokenType.String)
            {
                return true;
            }
            return false;
        }
    }
}
