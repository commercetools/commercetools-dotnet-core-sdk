using commercetools.Sdk.Domain;
using System.IO;
using Xunit;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.Serialization.Tests
{
    public class FieldTypeDeserializationTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public FieldTypeDeserializationTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

        [Fact]
        public void DeserializeStringFieldType()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/FieldTypes/String.json");
            Type deserialized = serializerService.Deserialize<Type>(serialized);
            Assert.IsType<StringType>(deserialized.FieldDefinitions[0].Type);
        }
    }
}
