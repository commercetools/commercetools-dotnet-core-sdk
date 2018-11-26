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
            TypeDraft typeDraft = this.typeFixture.CreateTypeDraft();
            Type createdType = commerceToolsClient.ExecuteAsync(new CreateCommand<Type>(typeDraft)).Result;
            this.typeFixture.TypesToDelete.Add(createdType);
            Assert.Equal(typeDraft.Key, createdType.Key);
        }

        [Fact]
        public void GetTypeById()
        {
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            Type type = this.typeFixture.CreateType();
            Type retrievedType = commerceToolsClient.ExecuteAsync(new GetByIdCommand<Type>(new Guid(type.Id))).Result;
            Assert.Equal(type.Id, retrievedType.Id);
        }

        [Fact]
        public void GetTypeByKey()
        {
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            Type type = this.typeFixture.CreateType();
            Type retrievedType = commerceToolsClient.ExecuteAsync(new GetByKeyCommand<Type>(type.Key)).Result;
            Assert.Equal(type.Key, retrievedType.Key);
        }

        [Fact]
        public void DeleteTypeById()
        {
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            Type type = this.typeFixture.CreateType();
            Type retrievedType = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Type>(new Guid(type.Id), type.Version)).Result;
            Assert.Equal(type.Key, retrievedType.Key);
        }

        [Fact]
        public void DeleteTypeByKey()
        {
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            Type type = this.typeFixture.CreateType();
            Type retrievedType = commerceToolsClient.ExecuteAsync(new DeleteByKeyCommand<Type>(type.Key, type.Version)).Result;
            Assert.Equal(type.Key, retrievedType.Key);
        }

        [Fact]
        public void UpdateTypeByKeyChangeName()
        {
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            Type type = this.typeFixture.CreateType();
            List<UpdateAction<Type>> updateActions = new List<UpdateAction<Type>>();
            string newName = this.typeFixture.RandomString(7);
            ChangeNameUpdateAction changeNameUpdateAction = new ChangeNameUpdateAction() { Name = new LocalizedString() { { "en", newName } } };
            updateActions.Add(changeNameUpdateAction);
            Type retrievedType = commerceToolsClient.ExecuteAsync(new UpdateByKeyCommand<Type>(type.Key, type.Version, updateActions)).Result;
            this.typeFixture.TypesToDelete.Remove(type);
            this.typeFixture.TypesToDelete.Add(retrievedType);
            Assert.Equal(newName, retrievedType.Name["en"]);
        }

        [Fact]
        public void UpdateTypeByIdChangeKey()
        {
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            Type type = this.typeFixture.CreateType();
            List<UpdateAction<Type>> updateActions = new List<UpdateAction<Type>>();
            string newKey = this.typeFixture.RandomString(7);
            ChangeKeyUpdateAction changeKeyUpdateAction = new ChangeKeyUpdateAction() { Key = newKey };
            updateActions.Add(changeKeyUpdateAction);
            Type retrievedType = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<Type>(new Guid(type.Id), type.Version, updateActions)).Result;
            this.typeFixture.TypesToDelete.Remove(type);
            this.typeFixture.TypesToDelete.Add(retrievedType);
            Assert.Equal(newKey, retrievedType.Key);
        }

        [Fact]
        public void UpdateTypeByIdAddEnumToFieldDefinition()
        { 
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            Type type = this.typeFixture.CreateType();
            List<UpdateAction<Type>> updateActions = new List<UpdateAction<Type>>();
            string newKey = this.typeFixture.RandomString(7);
            AddEnumToFieldDefinitionUpdateAction addEnumToFieldDefinitionUpdateAction = new AddEnumToFieldDefinitionUpdateAction() { FieldName = "enum-field", Value = new EnumValue() { Key = "new-enum-key", Label = "new-enum-label" } };
            updateActions.Add(addEnumToFieldDefinitionUpdateAction);
            Type retrievedType = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<Type>(new Guid(type.Id), type.Version, updateActions)).Result;
            this.typeFixture.TypesToDelete.Remove(type);
            this.typeFixture.TypesToDelete.Add(retrievedType);
            Assert.Equal(type.GetFieldDefinition("enum-field").Type.ToEnumType().Values.Count + 1, retrievedType.GetFieldDefinition("enum-field").Type.ToEnumType().Values.Count);
        }

        [Fact]
        public void UpdateTypeByIdAddFieldDefinition()
        {
            IClient commerceToolsClient = this.typeFixture.GetService<IClient>();
            Type type = this.typeFixture.CreateType();
            List<UpdateAction<Type>> updateActions = new List<UpdateAction<Type>>();
            string newKey = this.typeFixture.RandomString(7);
            AddFieldDefinitionUpdateAction addFieldDefinitionUpdateAction = new AddFieldDefinitionUpdateAction() { FieldDefinition = this.typeFixture.CreateStringField() };
            updateActions.Add(addFieldDefinitionUpdateAction);
            Type retrievedType = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<Type>(new Guid(type.Id), type.Version, updateActions)).Result;
            this.typeFixture.TypesToDelete.Remove(type);
            this.typeFixture.TypesToDelete.Add(retrievedType);
            Assert.Equal(type.FieldDefinitions.Count + 1, retrievedType.FieldDefinitions.Count);
        }
    }
}
