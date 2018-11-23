using commercetools.Sdk.Domain;
using System.Collections.Generic;
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

        [Fact]
        public void SerializeStringFieldType()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            TypeDraft typeDraft = new TypeDraft();
            typeDraft.Key = "new-key";
            typeDraft.ResourceTypeIds = new List<ResourceTypeId>();
            typeDraft.ResourceTypeIds.Add(ResourceTypeId.Category);
            typeDraft.ResourceTypeIds.Add(ResourceTypeId.CustomLineItem);
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "string-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "string description");
            fieldDefinition.InputHint = TextInputHint.SingleLine;
            fieldDefinition.Type = new StringType();
            typeDraft.FieldDefinitions = new List<FieldDefinition>();
            typeDraft.FieldDefinitions.Add(fieldDefinition);
            string serialized = serializerService.Serialize(typeDraft);            
        }
    }
}
