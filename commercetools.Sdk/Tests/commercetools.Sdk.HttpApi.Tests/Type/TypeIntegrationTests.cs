using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class TypeIntegrationTests : IClassFixture<TypeFixture>
    {
        private readonly TypeFixture typeFixture;

        public TypeIntegrationTests(TypeFixture typeFixture)
        {
            this.typeFixture = typeFixture;
        }

        [Fact]
        public void CreateType()
        {
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            TypeDraft typeDraft = new TypeDraft();
            typeDraft.Key = "my-category";
            typeDraft.Name = new LocalizedString();
            typeDraft.Name.Add("en", "customized fields");
            typeDraft.Description = new LocalizedString();
            typeDraft.Description.Add("en", "customized fields definition");
            typeDraft.ResourceTypeIds = new List<string>() { "category" };
            typeDraft.FieldDefinitions = new List<FieldDefinition>();
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "string-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "string description");
            fieldDefinition.InputHint = TextInputHint.SingleLine;
            fieldDefinition.Type = new StringType();
            typeDraft.FieldDefinitions.Add(fieldDefinition);
            Type createdType = commerceToolsClient.ExecuteAsync(new CreateCommand<Type>(typeDraft)).Result;
        }
    }
}
