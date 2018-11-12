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
                new MoneyConverter<Domain.Attribute, MoneyAttribute>(),
                new StringConverter<Domain.Attribute, TextAttribute>(),
                new LocalizedStringConverter<Domain.Attribute, LocalizedTextAttribute>(),
                new BooleanConverter<Domain.Attribute, BooleanAttribute>(),
                new NumberConverter<Domain.Attribute, NumberAttribute>(),
                new DateTimeConverter<Domain.Attribute, DateTimeAttribute>(),
                new TimeConverter<Domain.Attribute, TimeAttribute>(),
                new DateConverter<Domain.Attribute, DateAttribute>(),
                new EnumConverter<Domain.Attribute, EnumAttribute>(),
                new LocalizedEnumConverter<Domain.Attribute, LocalizedEnumAttribute>()
            };
            IEnumerable<ICustomConverter<CustomFields>> customFieldConverters = new List<ICustomConverter<CustomFields>>()
            {
                new MoneyConverter<CustomFields, Money>(),
                new StringConverter<CustomFields, string>(),
                new LocalizedStringConverter<CustomFields, LocalizedString>(),
                new BooleanConverter<CustomFields, bool>(),
                new NumberConverter<CustomFields, double>(),
                new DateTimeConverter<CustomFields, DateTime>(),
                new TimeConverter<CustomFields, TimeSpan>(),
                new DateConverter<CustomFields, DateTime>(),
                new EnumConverter<CustomFields, EnumValue>(),
                new LocalizedEnumConverter<CustomFields, LocalizedEnumValue>()
            };
            IEnumerable<ICustomConverter<Money>> customMoneyConverters = new List<ICustomConverter<Money>>()
            {
                new CentPrecisionMoneyConverter(),
                new HighPrecisionMoneyConverter()
            };
            MoneyConverter moneyConverter = new MoneyConverter(customMoneyConverters);
            AttributeConverter attributeConverter = new AttributeConverter(customAttributeConverters);
            FieldsConverter fieldCollectionConverter = new FieldsConverter(customFieldConverters);
            IEnumerable<JsonConverter> registeredConverters = new List<JsonConverter>() { moneyConverter, attributeConverter, fieldCollectionConverter };
            CustomContractResolver customContractResolver = new CustomContractResolver(registeredConverters);
            JsonSerializerSettingsFactory jsonSerializerSettingsFactory = new JsonSerializerSettingsFactory(customContractResolver);
            ISerializerService serializerService = new SerializerService(jsonSerializerSettingsFactory);
            return serializerService; 
        }
    }
}
