using System;
using Newtonsoft.Json.Linq;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Serialization
{
    internal class DateAttributeMapper : DateConverter<Attribute, DateTime>, ICustomJsonMapper<Attribute>
    {
        private readonly ISerializationConfiguration _configuration;

        public DateAttributeMapper(ISerializationConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public override bool CanConvert(JToken property)
        {
            return !_configuration.DeserializeDateAttributesAsString
                   && base.CanConvert(property);
        }
    }
}