using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class TypeIntegrationTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public TypeIntegrationTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void CreateType()
        {
            IClient commerceToolsClient = this.clientFixture.GetService<IClient>();
            TypeDraft typeDraft = new TypeDraft();
            typeDraft.Key = "my-category";
            typeDraft.Name.Add("en", "customized fields");
            typeDraft.Description.Add("en", "customized fields definition");
            typeDraft.ResourceTypeIds = new List<string>() { "category" };
            typeDraft.FieldDefinitions = new List<FieldDefinition>();
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "string-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label.Add("en", "string description");
            fieldDefinition.InputHint = TextInputHint.SingleLine;
            fieldDefinition.Type = new StringType();
            Type createdCategory = commerceToolsClient.ExecuteAsync(new CreateCommand<Type>(typeDraft)).Result;

        }

    }
}
