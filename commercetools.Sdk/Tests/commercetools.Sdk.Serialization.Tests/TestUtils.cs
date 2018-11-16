using commercetools.Sdk.Domain;
using commercetools.Sdk.Test.Helpers;
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
            IEnumerable<ICustomJsonMapper<Domain.Attribute>> attributeMappers = ReflectionHelper.GetInstancesForInterface<ICustomJsonMapper<Domain.Attribute>>();
            IEnumerable<ICustomJsonMapper<Fields>> customFieldMappers = ReflectionHelper.GetInstancesForInterface<ICustomJsonMapper<Fields>>();
            IEnumerable<ICustomJsonMapper<Money>> customMoneyConverters = new List<ICustomJsonMapper<Money>>()
            {
                new CentPrecisionMoneyConverter(),
                new HighPrecisionMoneyConverter()
            };
            IMapperTypeRetriever<Fields> fieldsMapperTypeRetriever = new SetFieldMapperTypeRetriever(customFieldMappers);
            IMapperTypeRetriever<Domain.Attribute> attributeMapperTypeRetriever = new SetAttributeMapperTypeRetriever(attributeMappers);
            MoneyConverter moneyConverter = new MoneyConverter(customMoneyConverters);
            AttributeConverter attributeConverter = new AttributeConverter(attributeMapperTypeRetriever);
            FieldsConverter fieldCollectionConverter = new FieldsConverter(fieldsMapperTypeRetriever);
            IEnumerable<JsonConverter> registeredConverters = new List<JsonConverter>() { moneyConverter, attributeConverter, fieldCollectionConverter };
            CustomContractResolver customContractResolver = new CustomContractResolver(registeredConverters);
            JsonSerializerSettingsFactory jsonSerializerSettingsFactory = new JsonSerializerSettingsFactory(customContractResolver);
            ISerializerService serializerService = new SerializerService(jsonSerializerSettingsFactory);
            return serializerService; 
        }
    }
}
