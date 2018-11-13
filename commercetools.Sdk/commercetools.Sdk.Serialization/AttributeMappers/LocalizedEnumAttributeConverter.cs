using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class LocalizedEnumAttributeConverter : LocalizedEnumConverter<Domain.Attribute, LocalizedEnumAttribute>,  ICustomJsonMapper<Domain.Attribute>
    {
    }
}
