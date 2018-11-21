using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Attributes;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class EnumAttributeMapper: EnumConverter<Domain.Attribute, PlainEnumValue>,  ICustomJsonMapper<Domain.Attribute>
    {
    }
}
