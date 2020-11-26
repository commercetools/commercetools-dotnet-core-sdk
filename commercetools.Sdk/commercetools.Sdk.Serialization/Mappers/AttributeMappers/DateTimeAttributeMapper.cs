using System;
using Newtonsoft.Json.Linq;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Serialization
{
    internal class DateTimeAttributeMapper : DateTimeConverter<Attribute, DateTime>, ICustomJsonMapper<Attribute>
    {
        private readonly ISerializationConfiguration _configuration;

        public DateTimeAttributeMapper(ISerializationConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public override bool CanConvert(JToken property)
        {
            return !_configuration.DeserializeDateTimeAttributesAsString
                   && base.CanConvert(property);
        }
    }
}