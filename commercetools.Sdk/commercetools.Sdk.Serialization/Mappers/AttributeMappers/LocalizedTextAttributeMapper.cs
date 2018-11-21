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
    public class LocalizedTextAttributeMapper : LocalizedStringConverter<Domain.Attribute, LocalizedString>, ICustomJsonMapper<Domain.Attribute>
    {
    }
}
