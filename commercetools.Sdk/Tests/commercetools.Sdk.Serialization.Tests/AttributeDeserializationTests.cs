using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Attributes;
using commercetools.Sdk.Test.Helpers;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class AttributeDeserializationTests
    {
        [Fact]
        public void DeserializeTextAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Text.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<string>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeLocalizedTextAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/LocalizedText.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<LocalizedString>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeDateTimeAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/DateTime.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<DateTime>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeTimeAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Time.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<TimeSpan>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeDateAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Date.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<DateTime>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeNumberAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Number.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<double>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeBooleanAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Boolean.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<bool>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeEnumAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Enum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<PlainEnumValue>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeLocalizedEnumAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/LocalizedEnum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<Domain.Attributes.LocalizedEnumValue>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeMoneyAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Money.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<Money>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeSetTextAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/SetText.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<Domain.Attributes.Set<string>>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeSetEnumAttribute()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/SetEnum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<Domain.Attributes.Set<PlainEnumValue>>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeAttributeNoValue()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/NoValue.json");
            Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<ProductVariant>(serialized));
        }

        [Fact]
        public void DeserializeAttributeInvalidStructure()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/InvalidEnum.json");
            Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<ProductVariant>(serialized));
        }

        [Fact]
        public void DeserializeAttributeList()
        {
            ISerializerService serializerService = SerializationHelper.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/List.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<string>>(deserialized.Attributes[0]);
            Assert.IsType<Attribute<PlainEnumValue>>(deserialized.Attributes[1]);
        }
    }
}
