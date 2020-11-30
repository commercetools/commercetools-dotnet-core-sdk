using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Registration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class AttributeDeserializationWithCustomConfigTests
    {
        [Fact]
        public void DeserializeDateAsTextAttribute()
        {
            var config = new SerializationConfiguration
            {
                DeserializeDateAttributesAsString = true
            };
            var serializerService = BuildSerializerServiceWithConfig(config);
            
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
            var serializerService = BuildSerializerServiceWithConfig(config);

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
        
        public ISerializerService BuildSerializerServiceWithConfig(
            SerializationConfiguration config = null)
        {
            var services = new ServiceCollection();
            services.UseRegistration();
            services.UseDomain();
            services.UseSerialization(config ?? SerializationConfiguration.DefaultConfig);
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<ISerializerService>();
        }
    }
}