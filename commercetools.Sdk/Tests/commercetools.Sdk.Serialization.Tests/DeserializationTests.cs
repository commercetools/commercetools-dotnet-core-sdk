using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class DeserializationTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public DeserializationTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

        [Fact]
        public void DeserializeLocalizedString()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;

            string serialized = @"
                {
                    ""name"": {
                        ""en"": ""name-en"",
                        ""en-US"": ""name-en-us""
                    }
                }
            ";

            var product = serializerService.Deserialize<ProductProjection>(serialized);

            Assert.Equal("name-en", product.Name["en"]);
            Assert.Equal("name-en-us", product.Name["en-US"]);
        }

        [Fact]
        public void DeserializeCustomField()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;

            string serialized = @"
                {
                    ""fields"": {
                        ""foo"": ""bar"",
                        ""Foos"": ""Bars""
                    }
                }
            ";

            var customFields = serializerService.Deserialize<CustomFields>(serialized);

            Assert.Equal("bar", customFields.Fields["foo"]);
            Assert.Equal("Bars", customFields.Fields["Foos"]);
        }

    }
}
