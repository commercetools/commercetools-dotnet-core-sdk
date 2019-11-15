using Newtonsoft.Json.Linq;
using System.IO;
using commercetools.Sdk.Domain.CustomObjects;
using FluentAssertions.Json;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class CustomObjectsSerializationTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public CustomObjectsSerializationTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }


        [Fact]
        public void SerializeCustomObjectDraft()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            var customObjectDraft = new CustomObjectDraft<CustomFooBar>
            {
                Key = "key1",
                Container = "container1",
                Value =  new CustomFooBar
                {
                    Foo = "Bar"
                },
                Version = 1
            };
            string result = serializerService.Serialize(customObjectDraft);
            JToken resultFormatted = JValue.Parse(result);
            string serialized = File.ReadAllText("Resources/CustomObjects/FooBarCustomObject.json");
            JToken serializedFormatted = JValue.Parse(serialized);
            serializedFormatted.Should().BeEquivalentTo(resultFormatted);
        }

        [Fact]
        public void CustomObjectDeserialize()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/CustomObjects/FooBarCustomObject.json");
            var customFooBarObject = serializerService.Deserialize<CustomObject<CustomFooBar>>(serialized);
            Assert.NotNull(customFooBarObject);
            Assert.IsType<CustomFooBar>(customFooBarObject.Value);
            var fooBar = customFooBarObject.Value;
            Assert.Equal("Bar",fooBar.Foo);
        }

    }

    public class CustomFooBar
    {
        public string Foo { get; set; }
    }

}
