using commercetools.Sdk.Domain;
using System;
using System.IO;
using System.Linq;
using commercetools.Sdk.Domain.Products.Attributes;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class AttributeDeserializationWithCustomConfigTests : IClassFixture<SerializationFixtureWithConfig>
    {
        private readonly SerializationFixtureWithConfig serializationFixture;

        public AttributeDeserializationWithCustomConfigTests(SerializationFixtureWithConfig serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

        [Fact]
        public void DeserializeDateAsTextAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            var serialized = @"
                {
                    ""id"": 1,
                    ""key"": ""newKey"",
                    ""attributes"": [
                                        {
                                            ""name"": ""text-attribute"",
                                            ""value"": ""2021-10-12""
                                        }
                                    ]
                }
            ";
            var deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<string>>(deserialized.Attributes[0]);
            Assert.IsType<JValue>(deserialized.Attributes[0].ToIAttribute().JsonValue);
        }
        
        [Fact]
        public void DeserializeDateTimeAsTextAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            var serialized = @"
                {
                    ""id"": 1,
                    ""key"": ""newKey"",
                    ""attributes"": [
                                        {
                                            ""name"": ""text-attribute"",
                                            ""value"": ""2021-10-12 05:50:06""
                                        }
                                    ]
                }
            ";
            var deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsType<Attribute<string>>(deserialized.Attributes[0]);
            Assert.IsType<JValue>(deserialized.Attributes[0].ToIAttribute().JsonValue);
        }
    }
}