using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.ProductTypes.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.ProductTypes.ProductTypesFixture;
using ChangeEnumValueLabelUpdateAction = commercetools.Sdk.Domain.ProductTypes.UpdateActions.ChangeEnumValueLabelUpdateAction;
using ChangeLocalizedEnumValueLabelUpdateAction = commercetools.Sdk.Domain.ProductTypes.UpdateActions.ChangeLocalizedEnumValueLabelUpdateAction;
using ChangeLocalizedEnumValueOrderUpdateAction = commercetools.Sdk.Domain.ProductTypes.UpdateActions.ChangeLocalizedEnumValueOrderUpdateAction;
using LocalizedEnumValue = commercetools.Sdk.Domain.Products.Attributes.LocalizedEnumValue;

namespace commercetools.Sdk.IntegrationTests.ProductTypes
{
    [Collection("Integration Tests")]
    public class ProductTypesIntegrationTests
    {
        private readonly IClient client;

        public ProductTypesIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateProductType()
        {
            var key = $"CreateProductType-{TestingUtility.RandomString()}";
            await WithProductType(
                client, productTypeDraft => DefaultProductTypeDraftWithKey(productTypeDraft, key),
                productType => { Assert.Equal(key, productType.Key); });
        }

        [Fact]
        public async Task GetProductTypeById()
        {
            var key = $"GetProductTypeById-{TestingUtility.RandomString()}";
            await WithProductType(
                client, productTypeDraft => DefaultProductTypeDraftWithKey(productTypeDraft, key),
                async productType =>
                {
                    var retrievedProductType = await client
                        .ExecuteAsync(new GetByIdCommand<ProductType>(productType.Id));
                    Assert.Equal(key, retrievedProductType.Key);
                });
        }

        [Fact]
        public async Task GetProductTypeByKey()
        {
            var key = $"GetProductTypeByKey-{TestingUtility.RandomString()}";
            await WithProductType(
                client, productTypeDraft => DefaultProductTypeDraftWithKey(productTypeDraft, key),
                async productType =>
                {
                    var retrievedProductType = await client
                        .ExecuteAsync(new GetByKeyCommand<ProductType>(productType.Key));
                    Assert.Equal(key, retrievedProductType.Key);
                });
        }

        [Fact]
        public async Task QueryProductTypes()
        {
            var key = $"QueryProductTypes-{TestingUtility.RandomString()}";
            await WithProductType(
                client, productTypeDraft => DefaultProductTypeDraftWithKey(productTypeDraft, key),
                async productType =>
                {
                    var queryCommand = new QueryCommand<ProductType>();
                    queryCommand.Where(p => p.Key ==productType.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteProductTypeById()
        {
            var key = $"DeleteProductTypeById-{TestingUtility.RandomString()}";
            await WithProductType(
                client, productTypeDraft => DefaultProductTypeDraftWithKey(productTypeDraft, key),
                async productType =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<ProductType>(productType));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<ProductType>(productType))
                    );
                });
        }

        [Fact]
        public async Task DeleteProductTypeByKey()
        {
            var key = $"DeleteProductTypeByKey-{TestingUtility.RandomString()}";
            await WithProductType(
                client, productTypeDraft => DefaultProductTypeDraftWithKey(productTypeDraft, key),
                async productType =>
                {
                    await client.ExecuteAsync(new DeleteByKeyCommand<ProductType>(productType.Key, productType.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<ProductType>(productType))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateProductTypeSetKey()
        {
            var newKey = $"UpdateProductTypeSetKey-{TestingUtility.RandomString()}";
            await WithUpdateableProductType(client, async productType =>
            {
                var updateActions = new List<UpdateAction<ProductType>>();
                var setKeyAction = new SetKeyUpdateAction {Key = newKey};
                updateActions.Add(setKeyAction);

                var updatedProductType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                Assert.Equal(newKey, updatedProductType.Key);
                return updatedProductType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeChangeName()
        {
            var newName = $"UpdateProductTypeChangeName-{TestingUtility.RandomString()}";
            await WithUpdateableProductType(client, async productType =>
            {
                var updateActions = new List<UpdateAction<ProductType>>();
                var changeNameAction = new ChangeNameUpdateAction { Name = newName };
                updateActions.Add(changeNameAction);

                var updatedProductType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                Assert.Equal(newName, updatedProductType.Name);
                return updatedProductType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeChangeDescription()
        {
            var newDescription = $"UpdateProductTypeChangeDescription-{TestingUtility.RandomString()}";
            await WithUpdateableProductType(client, async productType =>
            {
                var updateActions = new List<UpdateAction<ProductType>>();
                var changeDescriptionAction = new ChangeDescriptionUpdateAction { Description = newDescription};
                updateActions.Add(changeDescriptionAction);

                var updatedProductType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                Assert.Equal(newDescription, updatedProductType.Description);
                return updatedProductType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeAddAttributeDefinition()
        {
            await WithUpdateableProductType(client, DefaultProductTypeDraftWithEmptyAttributes,
                async productType =>
            {
                Assert.Empty(productType.Attributes);
                var textAttribute = CreateTextAttributeDefinitionDraft();
                var updateActions = new List<UpdateAction<ProductType>>();
                var addAttributeAction = new AddAttributeDefinitionUpdateAction
                {
                    Attribute = textAttribute
                };
                updateActions.Add(addAttributeAction);

                var updatedProductType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                Assert.Single(updatedProductType.Attributes);
                Assert.Equal(textAttribute.Name,updatedProductType.Attributes[0].Name);
                return updatedProductType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeRemoveAttributeDefinition()
        {
            await WithUpdateableProductType(client, DefaultProductTypeDraftWithOnlyTextAttribute,
                async productType =>
                {
                    Assert.Single(productType.Attributes);
                    var textAttribute = productType.Attributes[0];
                    var updateActions = new List<UpdateAction<ProductType>>();
                    var removeAttributeAction = new RemoveAttributeDefinitionUpdateAction
                    {
                        Name = textAttribute.Name
                    };
                    updateActions.Add(removeAttributeAction);

                    var updatedProductType = await client
                        .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                    Assert.Empty(updatedProductType.Attributes);
                    return updatedProductType;
                });
        }

        [Fact]
        public async Task UpdateProductTypeChangeAttributeDefinitionName()
        {
            var newName = $"UpdateProductTypeChangeAttributeDefinitionName-{TestingUtility.RandomString()}";
            await WithUpdateableProductType(client, DefaultProductTypeDraftWithOnlyTextAttribute,
                async productType =>
                {
                    Assert.Single(productType.Attributes);
                    var textAttribute = productType.Attributes[0];
                    var updateActions = new List<UpdateAction<ProductType>>();
                    var changeAttributeNameAction = new ChangeAttributeDefinitionNameUpdateAction
                    {
                        AttributeName = textAttribute.Name,
                        NewAttributeName = newName
                    };
                    updateActions.Add(changeAttributeNameAction);

                    var updatedProductType = await client
                        .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                    Assert.Equal(newName,updatedProductType.Attributes[0].Name);
                    return updatedProductType;
                });
        }

        [Fact]
        public async Task UpdateProductTypeChangeAttributeDefinitionLabel()
        {
            var newLabel = $"UpdateProductTypeChangeAttributeDefinitionLabel-{TestingUtility.RandomString()}";
            await WithUpdateableProductType(client, DefaultProductTypeDraftWithOnlyTextAttribute,
                async productType =>
                {
                    Assert.Single(productType.Attributes);
                    var textAttribute = productType.Attributes[0];
                    var updateActions = new List<UpdateAction<ProductType>>();
                    var changeAttributeLabelAction = new ChangeAttributeDefinitionLabelUpdateAction
                    {
                        AttributeName = textAttribute.Name,
                        Label = new LocalizedString {{"en", newLabel}}
                    };
                    updateActions.Add(changeAttributeLabelAction);

                    var updatedProductType = await client
                        .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                    Assert.Equal(newLabel,updatedProductType.Attributes[0].Label["en"]);
                    return updatedProductType;
                });
        }

        [Fact]
        public async Task UpdateProductTypeSetAttributeDefinitionInputTip()
        {
            var newInputTip = $"InputTip-{TestingUtility.RandomInt()}";
            await WithUpdateableProductType(client, DefaultProductTypeDraftWithOnlyTextAttribute,
                async productType =>
                {
                    Assert.Single(productType.Attributes);
                    var textAttribute = productType.Attributes[0];
                    var updateActions = new List<UpdateAction<ProductType>>();
                    var setInputTipAction = new SetAttributeDefinitionInputTipUpdateAction
                    {
                        AttributeName = textAttribute.Name,
                        InputTip = new LocalizedString {{"en", newInputTip}}
                    };
                    updateActions.Add(setInputTipAction);

                    var updatedProductType = await client
                        .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                    Assert.Equal(newInputTip,updatedProductType.Attributes[0].InputTip["en"]);
                    return updatedProductType;
                });
        }

        [Fact]
        public async Task UpdateProductTypeAddPlainEnumValue()
        {
            var rand = TestingUtility.RandomInt();
            var newPlainEnumValue = new PlainEnumValue {Key = $"enum-key-{rand}", Label = $"enum-label-{rand}"};
            await WithUpdateableProductType(client, async productType =>
            {
                Assert.NotEmpty(productType.Attributes);

                var enumAttribute =
                    productType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType));

                Assert.NotNull(enumAttribute);
                var updateActions = new List<UpdateAction<ProductType>>();
                var addPlainEnumValueToAttributeAction = new AddPlainEnumValueToAttributeDefinitionUpdateAction
                {
                    AttributeName = enumAttribute.Name,
                    Value = newPlainEnumValue
                };
                updateActions.Add(addPlainEnumValueToAttributeAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                var updatedEnumAttribute =
                    updatedType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType))?.Type as EnumAttributeType;

                Assert.NotNull(updatedEnumAttribute);

                Assert.Contains(updatedEnumAttribute.Values,
                    value => value.Key == newPlainEnumValue.Key && value.Label == newPlainEnumValue.Label);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeAddLocalizableEnumValue()
        {
            var rand = TestingUtility.RandomInt();
            var newLocalizedEnumValue = new LocalizedEnumValue
                { Key = $"enum-key-{rand}", Label = new LocalizedString {{"en", $"enum-label-{rand}"}}};
            await WithUpdateableProductType(client, async productType =>
            {
                Assert.NotEmpty(productType.Attributes);

                var localizedEnumAttribute =
                    productType.Attributes.FirstOrDefault(attribute =>
                        attribute.Type.GetType() == typeof(LocalizableEnumAttributeType));

                Assert.NotNull(localizedEnumAttribute);
                var updateActions = new List<UpdateAction<ProductType>>();
                var addLocalizedEnumValueToAttributeAction = new AddLocalizableEnumValueToAttributeDefinitionUpdateAction
                {
                    AttributeName = localizedEnumAttribute.Name,
                    Value = newLocalizedEnumValue
                };
                updateActions.Add(addLocalizedEnumValueToAttributeAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                var updatedLocalizedEnumAttribute =
                    updatedType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(LocalizableEnumAttributeType))?.Type as LocalizableEnumAttributeType;

                Assert.NotNull(updatedLocalizedEnumAttribute);

                Assert.Contains(updatedLocalizedEnumAttribute.Values,
                    value => value.Key == newLocalizedEnumValue.Key && value.Label["en"] == newLocalizedEnumValue.Label["en"]);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateProductRemoveEnumValueFromAttribute()
        {
            await WithUpdateableProductType(client, async productType =>
            {
                Assert.NotEmpty(productType.Attributes);

                var enumAttributeDefinition =
                    productType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType));

                Assert.NotNull(enumAttributeDefinition);
                var enumAttribute = enumAttributeDefinition.Type as EnumAttributeType;
                Assert.NotNull(enumAttribute);

                Assert.NotEmpty(enumAttribute.Values);
                var keys = new List<string> {enumAttribute.Values[0].Key};

                var updateActions = new List<UpdateAction<ProductType>>();
                var removeEnumValueToAttributeAction = new RemoveEnumValuesFromAttributeDefinitionUpdateAction
                {
                    AttributeName = enumAttributeDefinition.Name,
                    Keys = keys
                };
                updateActions.Add(removeEnumValueToAttributeAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                var updatedEnumAttribute =
                    updatedType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType))?.Type as EnumAttributeType;

                Assert.NotNull(updatedEnumAttribute);

                Assert.Equal(enumAttribute.Values.Count - 1, updatedEnumAttribute.Values.Count);
                Assert.DoesNotContain(updatedEnumAttribute.Values,
                    value => value.Key == keys[0]);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeChangeOrderOfAttributeDefinition()
        {
            await WithUpdateableProductType(client, async productType =>
            {
                Assert.NotEmpty(productType.Attributes);
                var attributeNames = productType.Attributes.Select(attribute => attribute.Name).ToList();
                attributeNames.Reverse();

                var updateActions = new List<UpdateAction<ProductType>>();
                var changeOrderOfAttributeDefinitions = new ChangeAttributeDefinitionsOrderUpdateAction
                {
                    AttributeNames = attributeNames
                };
                updateActions.Add(changeOrderOfAttributeDefinitions);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                Assert.NotEmpty(updatedType.Attributes);
                Assert.Equal(productType.Attributes[0].Name, updatedType.Attributes.Last().Name);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeChangeOrderOfEnumValues()
        {
            await WithUpdateableProductType(client, async productType =>
            {
                Assert.NotEmpty(productType.Attributes);

                var enumAttributeDefinition =
                    productType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType));
                Assert.NotNull(enumAttributeDefinition);

                var enumAttribute = enumAttributeDefinition.Type as EnumAttributeType;

                Assert.NotNull(enumAttribute);

                var attributeValues = enumAttribute.Values;
                attributeValues.Reverse();

                var updateActions = new List<UpdateAction<ProductType>>();
                var changePlainEnumValueOrderAction = new ChangePlainEnumValueOrderUpdateAction
                {
                    AttributeName = enumAttributeDefinition.Name,
                    Values = attributeValues
                };
                updateActions.Add(changePlainEnumValueOrderAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                Assert.NotEmpty(updatedType.Attributes);

                var updatedEnumAttribute =
                    updatedType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType))?.Type as EnumAttributeType;

                Assert.NotNull(updatedEnumAttribute);
                Assert.Equal(attributeValues[0].Key, updatedEnumAttribute.Values[0].Key);
                Assert.Equal(attributeValues[0].Label, updatedEnumAttribute.Values[0].Label);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeChangeOrderOfLocalizedEnumValues()
        {
            await WithUpdateableProductType(client, async productType =>
            {
                Assert.NotEmpty(productType.Attributes);

                var localizedEnumAttributeDefinition =
                    productType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(LocalizableEnumAttributeType));

                Assert.NotNull(localizedEnumAttributeDefinition);

                var enumAttribute = localizedEnumAttributeDefinition.Type as LocalizableEnumAttributeType;

                Assert.NotNull(enumAttribute);

                var attributeValues = enumAttribute.Values;
                attributeValues.Reverse();

                var updateActions = new List<UpdateAction<ProductType>>();
                var changePlainEnumValueOrderAction = new ChangeLocalizedEnumValueOrderUpdateAction
                {
                    AttributeName = localizedEnumAttributeDefinition.Name,
                    Values = attributeValues
                };
                updateActions.Add(changePlainEnumValueOrderAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                Assert.NotEmpty(updatedType.Attributes);

                var updatedEnumAttribute =
                    updatedType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(LocalizableEnumAttributeType))?.Type as LocalizableEnumAttributeType;

                Assert.NotNull(updatedEnumAttribute);
                Assert.Equal(attributeValues[0].Key, updatedEnumAttribute.Values[0].Key);
                Assert.Equal(attributeValues[0].Label["en"], updatedEnumAttribute.Values[0].Label["en"]);
                return updatedType;
            });
        }


        [Fact]
        public async Task UpdateProductTypeChangeKeyOfEnumValue()
        {
            var newKey = $"enum-Key-{TestingUtility.RandomInt()}";
            await WithUpdateableProductType(client, async productType =>
            {
                Assert.NotEmpty(productType.Attributes);

                var enumAttributeDefinition =
                    productType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType));

                Assert.NotNull(enumAttributeDefinition);

                var enumAttribute = enumAttributeDefinition.Type as EnumAttributeType;

                Assert.NotNull(enumAttribute);
                Assert.NotEmpty(enumAttribute.Values);

                var oldKey = enumAttribute.Values[0].Key;
                var updateActions = new List<UpdateAction<ProductType>>();
                var changeEnumValueKeyAction = new ChangeEnumValueKeyUpdateAction
                {
                    Key = oldKey,
                    NewKey = newKey,
                    AttributeName = enumAttributeDefinition.Name
                };
                updateActions.Add(changeEnumValueKeyAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                var updatedEnumAttribute =
                    updatedType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType))?.Type as EnumAttributeType;

                Assert.NotNull(updatedEnumAttribute);

                Assert.Contains(updatedEnumAttribute.Values,
                    value => value.Key == newKey);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeChangeLabelOfEnumValue()
        {
            var newLabel = $"enum-Label-{TestingUtility.RandomInt()}";
            await WithUpdateableProductType(client, async productType =>
            {
                Assert.NotEmpty(productType.Attributes);

                var enumAttributeDefinition =
                    productType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType));

                Assert.NotNull(enumAttributeDefinition);

                var enumAttribute = enumAttributeDefinition.Type as EnumAttributeType;

                Assert.NotNull(enumAttribute);
                Assert.NotEmpty(enumAttribute.Values);

                var newEnumValue = new PlainEnumValue { Key = enumAttribute.Values[0].Key, Label = newLabel};
                var updateActions = new List<UpdateAction<ProductType>>();
                var changeEnumValueLabelAction = new ChangeEnumValueLabelUpdateAction
                {
                    AttributeName = enumAttributeDefinition.Name,
                    NewValue = newEnumValue
                };
                updateActions.Add(changeEnumValueLabelAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                var updatedEnumAttribute =
                    updatedType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(EnumAttributeType))?.Type as EnumAttributeType;

                Assert.NotNull(updatedEnumAttribute);

                Assert.Contains(updatedEnumAttribute.Values,
                    value => value.Key == newEnumValue.Key && value.Label == newEnumValue.Label);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeChangeLabelOfLocalizedEnumValue()
        {
            var newLabel = $"enum-Label-{TestingUtility.RandomInt()}";
            await WithUpdateableProductType(client, async productType =>
            {
                Assert.NotEmpty(productType.Attributes);

                var enumAttributeDefinition =
                    productType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(LocalizableEnumAttributeType));

                Assert.NotNull(enumAttributeDefinition);

                var enumAttribute = enumAttributeDefinition.Type as LocalizableEnumAttributeType;

                Assert.NotNull(enumAttribute);
                Assert.NotEmpty(enumAttribute.Values);

                var newEnumValue = new LocalizedEnumValue
                {
                    Key = enumAttribute.Values[0].Key, Label = new LocalizedString {{"en", newLabel}}
                };
                var updateActions = new List<UpdateAction<ProductType>>();
                var changeLocalizedEnumValueLabelAction = new ChangeLocalizedEnumValueLabelUpdateAction
                {
                    AttributeName = enumAttributeDefinition.Name,
                    NewValue = newEnumValue
                };
                updateActions.Add(changeLocalizedEnumValueLabelAction);

                var updatedType = await client
                    .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                var updatedEnumAttribute =
                    updatedType.Attributes.FirstOrDefault(attribute => attribute.Type.GetType() == typeof(LocalizableEnumAttributeType))?.Type as LocalizableEnumAttributeType;

                Assert.NotNull(updatedEnumAttribute);

                Assert.Contains(updatedEnumAttribute.Values,
                    value => value.Key == newEnumValue.Key && value.Label["en"] == newEnumValue.Label["en"]);
                return updatedType;
            });
        }

        [Fact]
        public async Task UpdateProductTypeChangeAttributeDefinitionIsSearchable()
        {
            await WithUpdateableProductType(client, DefaultProductTypeDraftWithOnlyTextAttribute,
                async productType =>
                {
                    Assert.Single(productType.Attributes);
                    var textAttribute = productType.Attributes[0];
                    var newIsSearchable = !textAttribute.IsSearchable;

                    var updateActions = new List<UpdateAction<ProductType>>();
                    var setInputTipAction = new ChangeAttributeDefinitionIsSearchableUpdateAction
                    {
                        AttributeName = textAttribute.Name,
                        IsSearchable = newIsSearchable
                    };
                    updateActions.Add(setInputTipAction);

                    var updatedProductType = await client
                        .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                    Assert.Equal(newIsSearchable, updatedProductType.Attributes[0].IsSearchable);
                    return updatedProductType;
                });
        }

        [Fact]
        public async Task UpdateProductTypeChangeAttributeDefinitionInputHint()
        {
            await WithUpdateableProductType(client, DefaultProductTypeDraftWithOnlyTextAttribute,
                async productType =>
                {
                    Assert.Single(productType.Attributes);
                    var textAttribute = productType.Attributes[0];

                    var newInputHint = textAttribute.InputHint == TextInputHint.MultiLine
                        ? TextInputHint.SingleLine
                        : TextInputHint.MultiLine;
                    var updateActions = new List<UpdateAction<ProductType>>();
                    var setInputTipAction = new ChangeAttributeDefinitionInputHintUpdateAction
                    {
                        AttributeName = textAttribute.Name,
                        NewValue = TextInputHint.MultiLine
                    };
                    updateActions.Add(setInputTipAction);

                    var updatedProductType = await client
                        .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                    Assert.Equal(newInputHint, updatedProductType.Attributes[0].InputHint);
                    return updatedProductType;
                });
        }

        [Fact]
        public async Task UpdateProductTypeChangeAttributeDefinitionConstraint()
        {
            await WithUpdateableProductType(client,

                productTypeDraft =>
                    DefaultProductTypeDraftWithOnlyTextAttributeWithConstraint(productTypeDraft, AttributeConstraint.Unique),
                async productType =>
                {
                    Assert.Single(productType.Attributes);
                    var textAttribute = productType.Attributes[0];
                    Assert.Equal(AttributeConstraint.Unique, textAttribute.AttributeConstraint);
                    var updateActions = new List<UpdateAction<ProductType>>();
                    var setInputTipAction = new ChangeAttributeDefinitionAttributeConstraintUpdateAction
                    {
                        AttributeName = textAttribute.Name,
                        NewValue = AttributeConstraint.None
                    };
                    updateActions.Add(setInputTipAction);

                    var updatedProductType = await client
                        .ExecuteAsync(new UpdateByIdCommand<ProductType>(productType, updateActions));

                    Assert.Equal(AttributeConstraint.None, updatedProductType.Attributes[0].AttributeConstraint);
                    return updatedProductType;
                });
        }

        #endregion
    }
}
