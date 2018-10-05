using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class AttributeDeserializationTests
    {
        [Fact]
        public void DeserializeTextAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/Text.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<TextAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeLocalizedTextAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/LocalizedText.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<LocalizedTextAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeDateTimeAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/DateTime.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<DateTimeAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeTimeAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/Time.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<TimeAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeDateAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/Date.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<DateAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeNumberAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/Number.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<NumberAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeBooleanAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/Boolean.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<BooleanAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeEnumAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/Enum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<EnumAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeLocalizedEnumAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/LocalizedEnum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<LocalizedEnumAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeMoneyAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/Money.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<MoneyAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeSetTextAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/SetText.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<SetTextAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeAttributeNoValue()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/NoValue.json");
            Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<ProductVariant>(serialized));
        }

        [Fact]
        public void DeserializeAttributeInvalidStructure()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/InvalidEnum.json");
            Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<ProductVariant>(serialized));
        }

        [Fact]
        public void DeserializeAttributeList()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/List.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<TextAttribute>(deserialized.Attributes[0]);
            Assert.IsType<EnumAttribute>(deserialized.Attributes[1]);
        }
    }
}
