using commercetools.Sdk.Domain;
using commercetools.Sdk.Reflection;
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
            IEnumerable<ICustomJsonMapper<Domain.Attribute>> attributeMappers = TypeRetriever.GetInstancesForInterface<ICustomJsonMapper<Domain.Attribute>>();

            //IEnumerable<ICustomJsonMapper<Domain.Attribute>> customAttributeConverters = new List<ICustomJsonMapper<Domain.Attribute>>()
            //{
            //    new MoneyConverter<Domain.Attribute, MoneyAttribute>(),
            //    new StringConverter<Domain.Attribute, TextAttribute>(),
            //    new LocalizedStringConverter<Domain.Attribute, LocalizedTextAttribute>(),
            //    new BooleanConverter<Domain.Attribute, BooleanAttribute>(),
            //    new NumberConverter<Domain.Attribute, NumberAttribute>(),
            //    new DateTimeConverter<Domain.Attribute, DateTimeAttribute>(),
            //    new TimeConverter<Domain.Attribute, TimeAttribute>(),
            //    new DateConverter<Domain.Attribute, DateAttribute>(),
            //    new EnumConverter<Domain.Attribute, EnumAttribute>(),
            //    new LocalizedEnumConverter<Domain.Attribute, LocalizedEnumAttribute>()
            //};
            IEnumerable<ICustomJsonMapper<Fields>> customFieldConverters = new List<ICustomJsonMapper<Fields>>()
            {
                new MoneyConverter<Fields, Money>(),
                new StringConverter<Fields, string>(),
                new LocalizedStringConverter<Fields, LocalizedString>(),
                new BooleanConverter<Fields, bool>(),
                new NumberConverter<Fields, double>(),
                new DateTimeConverter<Fields, DateTime>(),
                new TimeConverter<Fields, TimeSpan>(),
                new DateConverter<Fields, DateTime>(),
                new EnumConverter<Fields, EnumValue>(),
                new LocalizedEnumConverter<Fields, LocalizedEnumValue>()
            };
            IEnumerable<ICustomJsonMapper<Money>> customMoneyConverters = new List<ICustomJsonMapper<Money>>()
            {
                new CentPrecisionMoneyConverter(),
                new HighPrecisionMoneyConverter()
            };
            IMapperTypeRetriever<Fields> fieldsMapperTypeRetriever = new MapperTypeRetriever<Fields>(customFieldConverters);
            MoneyConverter moneyConverter = new MoneyConverter(customMoneyConverters);
            AttributeConverter attributeConverter = new AttributeConverter(attributeMappers);
            FieldsConverter fieldCollectionConverter = new FieldsConverter(fieldsMapperTypeRetriever);
            IEnumerable<JsonConverter> registeredConverters = new List<JsonConverter>() { moneyConverter, attributeConverter, fieldCollectionConverter };
            CustomContractResolver customContractResolver = new CustomContractResolver(registeredConverters);
            JsonSerializerSettingsFactory jsonSerializerSettingsFactory = new JsonSerializerSettingsFactory(customContractResolver);
            ISerializerService serializerService = new SerializerService(jsonSerializerSettingsFactory);
            return serializerService; 
        }
    }
}
