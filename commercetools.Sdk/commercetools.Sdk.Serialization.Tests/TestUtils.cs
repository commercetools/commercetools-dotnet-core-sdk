using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Serialization.Tests
{
    public class TestUtils
    {
        public static ISerializerService GetSerializerService()
        {
            IEnumerable<ICustomConverter<Domain.Attribute>> customAttributeConverters = new List<ICustomConverter<Domain.Attribute>>()
            {
                new MoneyAttributeConverter(),
                new TextAttributeConverter(),
                new LocalizedTextAttributeConverter(),
                new BooleanAttributeConverter(),
                new NumberAttributeConverter(),
                new DateTimeAttributeConverter(),
                new TimeAttributeConverter(),
                new DateAttributeConverter(),
                new EnumAttributeConverter(),
                new LocalizedEnumAttributeConverter()
            };
            IEnumerable<ICustomConverter<Money>> customMoneyConverters = new List<ICustomConverter<Money>>()
            {
                new CentPrecisionMoneyConverter(),
                new HighPrecisionMoneyConverter()
            };
            MoneyConverter moneyConverter = new MoneyConverter(customMoneyConverters);
            AttributeConverter attributeConverter = new AttributeConverter(customAttributeConverters, moneyConverter);
            IEnumerable<JsonConverter> registeredConverters = new List<JsonConverter>() { moneyConverter, attributeConverter };
            CustomContractResolver customContractResolver = new CustomContractResolver(registeredConverters);
            JsonSerializerSettingsFactory jsonSerializerSettingsFactory = new JsonSerializerSettingsFactory(customContractResolver);
            ISerializerService serializerService = new SerializerService(jsonSerializerSettingsFactory);
            return serializerService; 
        }
    }
}
