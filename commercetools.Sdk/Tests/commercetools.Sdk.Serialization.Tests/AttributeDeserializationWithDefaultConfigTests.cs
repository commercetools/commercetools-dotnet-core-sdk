using System;
using System.IO;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Registration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class AttributeDeserializationWithDefaultConfigTests
    {
        [Fact]
        public void DeserializeDateAttribute()
        {
            var serializerService = BuildSerializerServiceWithConfig(SerializationConfiguration.DefaultConfig);
            string serialized = File.ReadAllText("Resources/Attributes/Date.json");
            ProductVariant deserialized = serializerService.Deserialize<ProductVariant>(serialized);
            Assert.IsAssignableFrom<Attribute<DateTime>>(deserialized.Attributes[0]);
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