using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class MoneyAttributeMapper : MoneyConverter<Domain.Attribute, Money>, ICustomJsonMapper<Domain.Attribute>
    {
    }
}
