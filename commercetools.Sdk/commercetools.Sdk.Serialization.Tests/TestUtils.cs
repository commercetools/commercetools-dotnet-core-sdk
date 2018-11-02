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
            IEnumerable<ICustomConverter<Field>> customFieldConverters = new List<ICustomConverter<Field>>()
            {
                new MoneyConverter<Field, MoneyField>(),
                new StringConverter<Field, StringField>(),
                new LocalizedStringConverter<Field, LocalizedStringField>(),
                new BooleanConverter<Field, BooleanField>(),
                new NumberConverter<Field, NumberField>(),
                new DateTimeConverter<Field, DateTimeField>(),
                new TimeConverter<Field, TimeField>(),
                new DateConverter<Field, DateField>(),
                new EnumConverter<Field, EnumField>(),
                new LocalizedEnumConverter<Field, LocalizedEnumField>()
            };
            IEnumerable<ICustomConverter<Money>> customMoneyConverters = new List<ICustomConverter<Money>>()
            {
                new CentPrecisionMoneyConverter(),
                new HighPrecisionMoneyConverter()
            };
            MoneyConverter moneyConverter = new MoneyConverter(customMoneyConverters);
            AttributeConverter attributeConverter = new AttributeConverter(customAttributeConverters);
            FieldCollectionConverter fieldCollectionConverter = new FieldCollectionConverter();
            IEnumerable<JsonConverter> registeredConverters = new List<JsonConverter>() { moneyConverter, attributeConverter, fieldCollectionConverter };
            CustomContractResolver customContractResolver = new CustomContractResolver(registeredConverters);
            JsonSerializerSettingsFactory jsonSerializerSettingsFactory = new JsonSerializerSettingsFactory(customContractResolver);
            ISerializerService serializerService = new SerializerService(jsonSerializerSettingsFactory);
            return serializerService; 
        }
    }
}
