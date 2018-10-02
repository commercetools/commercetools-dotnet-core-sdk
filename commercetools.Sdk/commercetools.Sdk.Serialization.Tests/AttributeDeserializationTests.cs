using commercetools.Sdk.Domain;
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
            string serialized = File.ReadAllText("Resources/text.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<TextAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeDateTimeAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/datetime.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<DateTimeAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeTimeAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/time.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<TimeAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeDateAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/date.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<DateAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeNumberAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/number.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<NumberAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeBooleanAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/boolean.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<BooleanAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeEnumAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/enum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<EnumAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeMoneyAttribute()
        {
            ISerializerService serializerService = new SerializerService(JsonSerializerSettingsFactory.Create);
            string serialized = File.ReadAllText("Resources/money.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<MoneyAttribute>(deserialized.Attributes[0]);
        }
    }
}
