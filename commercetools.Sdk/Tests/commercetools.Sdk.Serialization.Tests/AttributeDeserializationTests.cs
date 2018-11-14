using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class AttributeDeserializationTests
    {
        [Fact]
        public void DeserializeTextAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/Text.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<TextAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeLocalizedTextAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/LocalizedText.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<LocalizedTextAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeDateTimeAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/DateTime.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<DateTimeAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeTimeAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/Time.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<TimeAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeDateAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/Date.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<DateAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeNumberAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/Number.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<NumberAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeBooleanAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/Boolean.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<BooleanAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeEnumAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/Enum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<EnumAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeLocalizedEnumAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/LocalizedEnum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<LocalizedEnumAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeMoneyAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/Money.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<MoneyAttribute>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeSetTextAttribute()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/SetText.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<SetAttribute<string>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeAttributeNoValue()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/NoValue.json");
            Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<ProductVariant>(serialized));
        }

        [Fact]
        public void DeserializeAttributeInvalidStructure()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/InvalidEnum.json");
            Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<ProductVariant>(serialized));
        }

        [Fact]
        public void DeserializeAttributeList()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            string serialized = File.ReadAllText("Resources/Attributes/List.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<TextAttribute>(deserialized.Attributes[0]);
            Assert.IsType<EnumAttribute>(deserialized.Attributes[1]);
        }
    }
}
