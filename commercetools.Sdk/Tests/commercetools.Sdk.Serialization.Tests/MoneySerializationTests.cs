using System.IO;
using commercetools.Sdk.Domain;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class MoneySerializationTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public MoneySerializationTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

        [Fact]
        public void MoneyDeserialization()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = @"{
                ""currencyCode"": ""EUR"",
                ""centAmount"": 123456,
            }";
            var deserialized = serializerService.Deserialize<BaseMoney>(serialized);
            Assert.IsType<Money>(deserialized);
        }

        [Fact]
        public void MoneyWithTypeDeserialization()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = @"{
                ""type"": ""centPrecision"",
                ""currencyCode"": ""EUR"",
                ""centAmount"": 123456,
                ""fractionDigits"": 2
            }";
            var deserialized = serializerService.Deserialize<BaseMoney>(serialized);
            Assert.IsType<Money>(deserialized);
            Assert.Equal("EUR", deserialized.CurrencyCode);
            Assert.Equal(MoneyType.CentPrecision, deserialized.Type);
            Assert.Equal(1234.56M, deserialized.AmountToDecimal());
            Assert.Equal(2, deserialized.FractionDigits);
        }

        [Fact]
        public void HighPrecisionMoneyDeserialization()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = @"{
                ""type"": ""highPrecision"",
                ""currencyCode"": ""EUR"",
                ""centAmount"": 123456,
                ""preciseAmount"": 12345678,
                ""fractionDigits"": 4
            }";
            var deserialized = serializerService.Deserialize<BaseMoney>(serialized);
            Assert.IsType<HighPrecisionMoney>(deserialized);
            Assert.Equal("EUR", deserialized.CurrencyCode);
            Assert.Equal(MoneyType.HighPrecision, deserialized.Type);
            Assert.Equal(123456, deserialized.CentAmount);
            Assert.Equal(1234.5678M, deserialized.AmountToDecimal());
            Assert.Equal(4, deserialized.FractionDigits);
        }

        [Fact]
        public void HighPrecisionMoneySerialization()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            var m = HighPrecisionMoney.FromDecimal("EUR", 1234.56789M, 4);

            var serialize = serializerService.Serialize(m);
            JToken resultFormatted = JValue.Parse(serialize);
            JToken serializedFormatted = JValue.Parse(@"{""type"":""highPrecision"",""preciseAmount"":12345679,""currencyCode"":""EUR"",""centAmount"":123457,""fractionDigits"":4}");
            serializedFormatted.Should().BeEquivalentTo(resultFormatted);
        }

        [Fact]
        public void CentPrecisionMoneySerialization()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            var m = Money.FromDecimal("EUR", 1234.5678M);

            var serialize = serializerService.Serialize(m);
            JToken resultFormatted = JValue.Parse(serialize);
            JToken serializedFormatted = JValue.Parse(@"{""type"":""centPrecision"",""currencyCode"":""EUR"",""centAmount"":123457}");
            serializedFormatted.Should().BeEquivalentTo(resultFormatted);
        }

        [Fact]
        public void DeserializeInvalidMoney()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = @"{
                ""type"": ""unknown"",
                ""centAmount"": 123456
            }";

            var exception = Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<BaseMoney>(serialized));
            Assert.Equal("Unknown money type: {\"type\":\"unknown\",\"centAmount\":123456}", exception.Message);
        }
    }
}
