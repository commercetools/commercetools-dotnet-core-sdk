using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Types.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.IntegrationTests.Types
{
    [Collection("Integration Tests")]
    public class TypesIntegrationTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly IClient client;

        public TypesIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateType()
        {
            var key = $"CreateType-{TestingUtility.RandomString()}";
            await WithType(
                client, typeDraft => DefaultTypeDraftWithKey(typeDraft, key),
                type => { Assert.Equal(key, type.Key); });
        }

        [Fact]
        public async Task GetTypeById()
        {
            var key = $"GetTypeById-{TestingUtility.RandomString()}";
            await WithType(
                client, typeDraft => DefaultTypeDraftWithKey(typeDraft, key),
                async type =>
                {
                    var retrievedType = await client
                        .ExecuteAsync(new GetByIdCommand<Type>(type.Id));
                    Assert.Equal(key, retrievedType.Key);
                });
        }

        [Fact]
        public async Task GetTypeByKey()
        {
            var key = $"GetTypeByKey-{TestingUtility.RandomString()}";
            await WithType(
                client, typeDraft => DefaultTypeDraftWithKey(typeDraft, key),
                async type =>
                {
                    var retrievedType = await client
                        .ExecuteAsync(new GetByKeyCommand<Type>(type.Key));
                    Assert.Equal(key, retrievedType.Key);
                });
        }

        [Fact]
        public async Task QueryTypes()
        {
            var key = $"QueryTypes-{TestingUtility.RandomString()}";
            await WithType(
                client, typeDraft => DefaultTypeDraftWithKey(typeDraft, key),
                async type =>
                {
                    var queryCommand = new QueryCommand<Type>();
                    queryCommand.Where(p => p.Key == type.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteTypeById()
        {
            var key = $"DeleteTypeById-{TestingUtility.RandomString()}";
            await WithType(
                client, typeDraft => DefaultTypeDraftWithKey(typeDraft, key),
                async type =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<Type>(type));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Type>(type))
                    );
                });
        }

        [Fact]
        public async Task DeleteTypeByKey()
        {
            var key = $"DeleteTypeByKey-{TestingUtility.RandomString()}";
            await WithType(
                client, typeDraft => DefaultTypeDraftWithKey(typeDraft, key),
                async type =>
                {
                    await client.ExecuteAsync(new DeleteByKeyCommand<Type>(type.Key, type.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Type>(type))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateTypeChangeKey()
        {
            var newKey = $"UpdateTypeChangeKey-{TestingUtility.RandomString()}";
            await WithUpdateableType(client, async type =>
            {
                var updateActions = new List<UpdateAction<Type>>();
                var changeKeyAction = new ChangeKeyUpdateAction {Key = newKey};
                updateActions.Add(changeKeyAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.Equal(newKey, updatedType.Key);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeChangeName()
        {
            var newName = new LocalizedString {{"en", $"UpdateTypeChangeName-{TestingUtility.RandomString()}"}};
            await WithUpdateableType(client, async type =>
            {
                var updateActions = new List<UpdateAction<Type>>();
                var changeNameAction = new ChangeNameUpdateAction {Name = newName};
                updateActions.Add(changeNameAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.Equal(newName["en"], updatedType.Name["en"]);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeSetDescription()
        {
            var newDescription = new LocalizedString
                {{"en", $"UpdateTypeSetDescription-{TestingUtility.RandomString()}"}};
            await WithUpdateableType(client, async type =>
            {
                var updateActions = new List<UpdateAction<Type>>();
                var setDescriptionAction = new SetDescriptionUpdateAction {Description = newDescription};
                updateActions.Add(setDescriptionAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.Equal(newDescription["en"], updatedType.Description["en"]);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeAddFieldDefinition()
        {
            var stringField = CreateStringFieldDefinition();
            await WithUpdateableType(client, DefaultTypeDraftWithoutFields, async type =>
            {
                Assert.Empty(type.FieldDefinitions);

                var updateActions = new List<UpdateAction<Type>>();
                var addFieldDefinitionAction = new AddFieldDefinitionUpdateAction
                {
                    FieldDefinition = stringField
                };
                updateActions.Add(addFieldDefinitionAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.Single(updatedType.FieldDefinitions);
                Assert.IsType<StringFieldType>(updatedType.FieldDefinitions[0].Type);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeRemoveFieldDefinition()
        {
            await WithUpdateableType(client, DefaultTypeDraftWithOneStringField, async type =>
            {
                Assert.Single(type.FieldDefinitions);

                var updateActions = new List<UpdateAction<Type>>();
                var removeFieldDefinitionAction = new RemoveFieldDefinitionUpdateAction
                {
                    FieldName = type.FieldDefinitions[0].Name
                };
                updateActions.Add(removeFieldDefinitionAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.Empty(updatedType.FieldDefinitions);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeChangeFieldDefinitionLabel()
        {
            await WithUpdateableType(client, DefaultTypeDraftWithOneStringField, async type =>
            {
                Assert.Single(type.FieldDefinitions);
                var newLabel = new LocalizedString {{"en", $"UpdateTypeChangeLabel-{TestingUtility.RandomString()}"}};
                var updateActions = new List<UpdateAction<Type>>();
                var changeFieldDefinitionLabelAction = new ChangeFieldDefinitionLabelUpdateAction
                {
                    FieldName = type.FieldDefinitions[0].Name,
                    Label = newLabel
                };
                updateActions.Add(changeFieldDefinitionLabelAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.Equal(newLabel["en"], updatedType.FieldDefinitions[0].Label["en"]);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeAddEnumValueToFieldDefinition()
        {
            var rand = TestingUtility.RandomInt();
            var newEnumValue = new EnumValue() {Key = $"enum-key-{rand}", Label = $"enum-label-{rand}"};
            await WithUpdateableType(client, async type =>
            {
                Assert.NotEmpty(type.FieldDefinitions);
                var enumField =
                    type.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(EnumFieldType));

                Assert.NotNull(enumField);
                var updateActions = new List<UpdateAction<Type>>();
                var addEnumValueToFieldAction = new AddEnumToFieldDefinitionUpdateAction
                {
                    Value = newEnumValue,
                    FieldName = enumField.Name
                };
                updateActions.Add(addEnumValueToFieldAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                var updatedEnumField =
                    updatedType.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(EnumFieldType))?.Type as EnumFieldType;

                Assert.NotNull(updatedEnumField);

                Assert.Contains(updatedEnumField.Values,
                    value => value.Key == newEnumValue.Key && value.Label == newEnumValue.Label);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeAddLocalizedEnumValueToFieldDefinition()
        {
            var rand = TestingUtility.RandomInt();
            var newEnumValue = new LocalizedEnumValue
            {
                Key = $"enum-key-{rand}", Label = new LocalizedString() { { "en", $"enum-label-{rand}" } }
            };
            await WithUpdateableType(client, async type =>
            {
                Assert.NotEmpty(type.FieldDefinitions);
                var enumField =
                    type.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(LocalizedEnumFieldType));

                Assert.NotNull(enumField);
                var updateActions = new List<UpdateAction<Type>>();
                var addLocalizedEnumValueToFieldAction = new AddLocalizedEnumToFieldDefinitionUpdateAction
                {
                    Value = newEnumValue,
                    FieldName = enumField.Name
                };
                updateActions.Add(addLocalizedEnumValueToFieldAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                var updatedEnumField =
                    updatedType.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(LocalizedEnumFieldType))?.Type as LocalizedEnumFieldType;

                Assert.NotNull(updatedEnumField);

                Assert.Contains(updatedEnumField.Values,
                    value => value.Key == newEnumValue.Key && value.Label["en"] == newEnumValue.Label["en"]);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeChangeOrderOfFieldsDefinition()
        {
            await WithUpdateableType(client, async type =>
            {
                Assert.NotEmpty(type.FieldDefinitions);
                var fieldNames = type.FieldDefinitions.Select(field => field.Name).ToList();
                fieldNames.Reverse();

                var updateActions = new List<UpdateAction<Type>>();
                var changeOrderOfFieldDefinitions = new ChangeFieldDefinitionOrderUpdateAction
                {
                    FieldNames = fieldNames
                };
                updateActions.Add(changeOrderOfFieldDefinitions);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.NotEmpty(updatedType.FieldDefinitions);
                Assert.Equal(type.FieldDefinitions[0].Name, updatedType.FieldDefinitions.Last().Name);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeChangeOrderOfEnumValues()
        {
            await WithUpdateableType(client, async type =>
            {
                Assert.NotEmpty(type.FieldDefinitions);

                var enumFieldDefinition =
                    type.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(EnumFieldType));
                Assert.NotNull(enumFieldDefinition);

                var enumField = enumFieldDefinition.Type as EnumFieldType;

                Assert.NotNull(enumField);

                var fieldValues = enumField.Values;
                fieldValues.Reverse();
                var keys = fieldValues.Select(value => value.Key).ToList();

                var updateActions = new List<UpdateAction<Type>>();
                var changeOrderOfFieldDefinitions = new ChangeEnumValueOrderUpdateAction
                {
                    FieldName = enumFieldDefinition.Name,
                    Keys = keys
                };
                updateActions.Add(changeOrderOfFieldDefinitions);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.NotEmpty(updatedType.FieldDefinitions);

                var updatedEnumField =
                    updatedType.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(EnumFieldType))?.Type as EnumFieldType;

                Assert.NotNull(updatedEnumField);
                Assert.Equal(fieldValues[0].Key, updatedEnumField.Values[0].Key);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeChangeOrderOfLocalizedEnumValues()
        {
            await WithUpdateableType(client, async type =>
            {
                Assert.NotEmpty(type.FieldDefinitions);

                var enumFieldDefinition =
                    type.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(LocalizedEnumFieldType));
                Assert.NotNull(enumFieldDefinition);

                var enumField = enumFieldDefinition.Type as LocalizedEnumFieldType;

                Assert.NotNull(enumField);

                var fieldValues = enumField.Values;
                fieldValues.Reverse();
                var keys = fieldValues.Select(value => value.Key).ToList();

                var updateActions = new List<UpdateAction<Type>>();
                var changeOrderOfFieldDefinitions = new ChangeLocalizedEnumValueOrderUpdateAction
                {
                    FieldName = enumFieldDefinition.Name,
                    Keys = keys
                };
                updateActions.Add(changeOrderOfFieldDefinitions);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.NotEmpty(updatedType.FieldDefinitions);

                var updatedEnumField =
                    updatedType.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(LocalizedEnumFieldType))?.Type as LocalizedEnumFieldType;

                Assert.NotNull(updatedEnumField);
                Assert.Equal(fieldValues[0].Key, updatedEnumField.Values[0].Key);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeChangeInputHint()
        {
            await WithUpdateableType(client, DefaultTypeDraftWithOneStringField, async type =>
            {
                Assert.Single(type.FieldDefinitions);
                Assert.Equal(TextInputHint.SingleLine, type.FieldDefinitions[0].InputHint);
                var updateActions = new List<UpdateAction<Type>>();
                var changeInputHintAction = new ChangeInputHintUpdateAction
                {
                    FieldName = type.FieldDefinitions[0].Name,
                    InputHint = TextInputHint.MultiLine
                };
                updateActions.Add(changeInputHintAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                Assert.Equal(TextInputHint.MultiLine, updatedType.FieldDefinitions[0].InputHint);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeChangeEnumValueLabel()
        {
            var newEnumValueLabel = $"enum-label-{TestingUtility.RandomInt()}";
            await WithUpdateableType(client, async type =>
            {
                Assert.NotEmpty(type.FieldDefinitions);
                var enumFieldDefinition =
                    type.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(EnumFieldType));

                Assert.NotNull(enumFieldDefinition);

                var enumField = enumFieldDefinition.Type as EnumFieldType;

                Assert.NotNull(enumField);
                Assert.NotEmpty(enumField.Values);

                var updateActions = new List<UpdateAction<Type>>();
                var newEnumValue = new EnumValue {Key = enumField.Values[0].Key, Label = newEnumValueLabel};
                var changeEnumValueLabelAction = new ChangeEnumValueLabelUpdateAction
                {
                    FieldName = enumFieldDefinition.Name,
                    Value = newEnumValue
                };
                updateActions.Add(changeEnumValueLabelAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                var updatedEnumField =
                    updatedType.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(EnumFieldType))?.Type as EnumFieldType;

                Assert.NotNull(updatedEnumField);

                Assert.Contains(updatedEnumField.Values,
                    value => value.Key == newEnumValue.Key && value.Label == newEnumValue.Label);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateTypeChangeLocalizedEnumValueLabel()
        {
            var newEnumValueLabel = $"enum-label-{TestingUtility.RandomInt()}";
            await WithUpdateableType(client, async type =>
            {
                Assert.NotEmpty(type.FieldDefinitions);
                var enumFieldDefinition =
                    type.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(LocalizedEnumFieldType));

                Assert.NotNull(enumFieldDefinition);

                var enumField = enumFieldDefinition.Type as LocalizedEnumFieldType;

                Assert.NotNull(enumField);
                Assert.NotEmpty(enumField.Values);

                var updateActions = new List<UpdateAction<Type>>();
                var newEnumValue = new LocalizedEnumValue
                {
                    Key = enumField.Values[0].Key,
                    Label = new LocalizedString
                    {
                        {"en", newEnumValueLabel}
                    }
                };
                var changeLocalizedEnumValueLabelAction = new ChangeLocalizedEnumValueLabelUpdateAction
                {
                    FieldName = enumFieldDefinition.Name,
                    Value = newEnumValue
                };
                updateActions.Add(changeLocalizedEnumValueLabelAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<Type>(type, updateActions));

                var updatedEnumField =
                    updatedType.FieldDefinitions.FirstOrDefault(field => field.Type.GetType() == typeof(LocalizedEnumFieldType))?.Type as LocalizedEnumFieldType;

                Assert.NotNull(updatedEnumField);

                Assert.Contains(updatedEnumField.Values,
                    value => value.Key == newEnumValue.Key && value.Label["en"] == newEnumValue.Label["en"]);
                return updatedType;
            });
        }


        #endregion
    }
}
