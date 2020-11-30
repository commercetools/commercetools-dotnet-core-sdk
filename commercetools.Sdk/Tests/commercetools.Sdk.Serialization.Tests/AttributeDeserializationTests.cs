using commercetools.Sdk.Domain;
using System;
using System.IO;
using System.Linq;
using commercetools.Sdk.Domain.Products.Attributes;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Serialization.Tests
{
    public class AttributeDeserializationTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public AttributeDeserializationTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

        [Fact]
        public void DeserializeTextAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Text.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<string>>(deserialized.Attributes[0]);
        }
        
        [Fact]
        public void SerializeTextAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Text.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);

            string result = serializerService.Serialize(deserialized.Attributes[0]);
            JToken resultFormatted = JValue.Parse(result);

            string expectedSerialized = File.ReadAllText("Resources/Attributes/Text.json");
            JToken serializedFormatted = JObject.Parse(expectedSerialized).GetValue("attributes")[0];
            serializedFormatted.Should().BeEquivalentTo(resultFormatted);
        }

        [Fact]
        public void DeserializeLocalizedTextAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/LocalizedText.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<LocalizedString>>(deserialized.Attributes[0]);
            // Assert.IsAssignableFrom<JObject>(deserialized.Attributes[0].ToIAttribute().JsonValue);
        }

        [Fact]
        public void DeserializeDateAttribute()
        {
            var serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Date.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<DateTime>>(deserialized.Attributes[0]);
        }
        
        [Fact]
        public void DeserializeDateTimeAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/DateTime.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<DateTime>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeTimeAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Time.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<TimeSpan>>(deserialized.Attributes[0]);
        }
        
        [Fact]
        public void DeserializeNumberAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Number.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<double>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeBooleanAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Boolean.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<bool>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeEnumAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Enum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<PlainEnumValue>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeLocalizedEnumAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/LocalizedEnum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<LocalizedEnumValue>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeMoneyAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/Money.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<BaseMoney>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeSetTextAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/SetText.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<AttributeSet<string>>>(deserialized.Attributes[0]);
        }

        [Fact]
        public void DeserializeEmptySetAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/EmptySet.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            var emptySetAttribute = deserialized.Attributes[0] as Attribute<AttributeSet<object>>;
            Assert.NotNull(emptySetAttribute);
            Assert.Empty(emptySetAttribute.Value);
        }

        [Fact]
        public void DeserializeSetEnumAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/SetEnum.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.True(deserialized.Attributes[0].IsSetEnumAttribute());
            var attr = deserialized.Attributes[0].ToSetEnumAttribute();
            Assert.NotNull(attr);
        }
        
        [Fact]
        public void DeserializeSetNestedAttributeDefinition()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/SetNestedDefinition.json");
            var attr = serializerService.Deserialize<AttributeDefinition>(serialized);
            Assert.NotNull(attr);
            var setAttrType = attr.Type as SetAttributeType;
            Assert.NotNull(setAttrType);
            Assert.NotNull(setAttrType.ElementType);
            var nestedAttrType = setAttrType.ElementType as NestedAttributeType;
            Assert.NotNull(nestedAttrType);
            Assert.NotNull(nestedAttrType.TypeReference);

        }

        [Fact]
        public void DeserializeAttributeNoValue()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/NoValue.json");
            Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<ProductVariant>(serialized));
        }

        [Fact]
        public void DeserializeAttributeInvalidStructure()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/InvalidEnum.json");
            Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<ProductVariant>(serialized));
        }

        [Fact]
        public void DeserializeAttributeInvalidLocalizedString()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/InvalidLocalizedText.json");

            var deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<LocalizedString>>(deserialized.Attributes[0]);
            Assert.Equal("invalid-loc-string-de",deserialized.Attributes[0].ToLocalizedTextAttribute().Value["_"]);
        }

        [Fact]
        public void DeserializeAttributeList()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/List.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<string>>(deserialized.Attributes[0]);
            Assert.IsAssignableFrom<Attribute<PlainEnumValue>>(deserialized.Attributes[1]);
        }
        
        [Fact]
        public void DeserializeAttributeListAndGetAttributeValue()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Attributes/List.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            var attributes = deserialized.Attributes;
            Assert.NotEmpty(attributes);
            
            Assert.True(attributes[0].IsTextAttribute());
            var textAttribute = attributes[0].ToTextAttribute();
            Assert.NotNull(textAttribute);
            
            Assert.True(attributes[1].IsEnumAttribute());
            var enumAttribute = attributes[1].ToEnumAttribute();
            Assert.NotNull(enumAttribute);
            
            Assert.True(attributes[2].IsBooleanAttribute());
            var boolAttribute = attributes[2].ToBooleanAttribute();
            Assert.NotNull(boolAttribute);
            
            
        }
        
        [Fact]
        public void DeserializeProductWithNestedAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string productSerialized = File.ReadAllText("Resources/Types/ProductWithNestedAttribute.json");
            
            var product = serializerService.Deserialize<Product>(productSerialized);
            var attr = product.MasterData.Current.MasterVariant.Attributes[0];
            
            Assert.True(attr.IsNestedAttribute());
            var nestedAttr = attr.ToNestedAttribute();
            Assert.NotNull(nestedAttr);
            Assert.True(nestedAttr.Value.Count == 2);
            Assert.True(nestedAttr.Value[0].IsNumberAttribute());
            Assert.True(nestedAttr.Value[1].IsTextAttribute());
        }
        
        [Fact]
        public void DeserializeProductWithSetNestedAttribute()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string productSerialized = File.ReadAllText("Resources/Types/ProductWithSetNestedAttribute.json");
            
            var product = serializerService.Deserialize<Product>(productSerialized);
            var attr = product.MasterData.Current.MasterVariant.Attributes[0];
            
            Assert.True(attr.IsSetOfNestedAttribute());
            var setOfNestedAttr = attr.ToSetOfNestedAttribute();
            Assert.NotNull(setOfNestedAttr);
            Assert.Equal(2,setOfNestedAttr.Value.Count);
            var firstNestedAttr = setOfNestedAttr.Value.FirstOrDefault();
            Assert.NotNull(firstNestedAttr);
            Assert.Equal(2, firstNestedAttr.Value.Count);
            Assert.True(firstNestedAttr.Value[0].IsNumberAttribute());
            Assert.True(firstNestedAttr.Value[1].IsTextAttribute());
        }
        
        [Fact]
        public void DeserializeProductWithSetNestedAttribute2()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string productSerialized = File.ReadAllText("Resources/Types/ProductWithSetNestedAttribute2.json");
            
            var product = serializerService.Deserialize<Product>(productSerialized);
            var attr = product.MasterData.Current.MasterVariant.Attributes[0];
            
            
            Assert.True(attr.IsSetOfNestedAttribute());
            var setOfNestedAttr = attr.ToSetOfNestedAttribute();
            Assert.NotNull(setOfNestedAttr);
            Assert.Equal("supplier-info",setOfNestedAttr.Name);
            Assert.Single(setOfNestedAttr.Value);
            var firstNestedAttr = setOfNestedAttr.Value.FirstOrDefault();
            Assert.NotNull(firstNestedAttr);
            Assert.Equal(4, firstNestedAttr.Value.Count);
            Assert.True(firstNestedAttr.Value[0].IsTextAttribute());
            Assert.True(firstNestedAttr.Value[3].IsMoneyAttribute());
        }
        
        [Fact]
        public void DeserializeDateAsTextAttribute()
        {
            var config = new SerializationConfiguration
            {
                DeserializeDateAttributesAsString = true
            };
            var serializerService = this.serializationFixture.BuildSerializerServiceWithConfig(config);
            
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
            Assert.IsAssignableFrom<Attribute<string>>(deserialized.Attributes[0]);
        }
        
        [Fact]
        public void DeserializeDateTimeAsTextAttribute()
        {
            var config = new SerializationConfiguration
            {
                DeserializeDateTimeAttributesAsString = true
            };
            var serializerService = this.serializationFixture.BuildSerializerServiceWithConfig(config);

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
            Assert.IsAssignableFrom<Attribute<string>>(deserialized.Attributes[0]);
        }

    }
}
