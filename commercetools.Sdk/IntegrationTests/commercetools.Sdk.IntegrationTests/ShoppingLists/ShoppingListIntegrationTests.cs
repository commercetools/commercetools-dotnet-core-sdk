using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.Domain.ShoppingLists.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.ShoppingLists.ShoppingListsFixture;
using static commercetools.Sdk.IntegrationTests.Customers.CustomersFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.ShoppingLists.UpdateActions.SetDescriptionUpdateAction;


namespace commercetools.Sdk.IntegrationTests.ShoppingLists
{
    [Collection("Integration Tests")]
    public class ShoppingListIntegrationTests
    {
        private readonly IClient client;

        public ShoppingListIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateShoppingList()
        {
            var key = $"CreateShoppingList-{TestingUtility.RandomString()}";
            await WithShoppingList(
                client, shoppingListDraft => DefaultShoppingListDraftWithKey(shoppingListDraft, key),
                shoppingList => { Assert.Equal(key, shoppingList.Key); });
        }

        [Fact]
        public async Task GetShoppingListById()
        {
            var key = $"GetShoppingListById-{TestingUtility.RandomString()}";
            await WithShoppingList(
                client, shoppingListDraft => DefaultShoppingListDraftWithKey(shoppingListDraft, key),
                async shoppingList =>
                {
                    var retrievedShoppingList = await client
                        .ExecuteAsync(new GetByIdCommand<ShoppingList>(shoppingList.Id));
                    Assert.Equal(key, retrievedShoppingList.Key);
                });
        }

        [Fact]
        public async Task GetShoppingListByKey()
        {
            var key = $"GetShoppingListByKey-{TestingUtility.RandomString()}";
            await WithShoppingList(
                client, shoppingListDraft => DefaultShoppingListDraftWithKey(shoppingListDraft, key),
                async shoppingList =>
                {
                    var retrievedShoppingList = await client
                        .ExecuteAsync(new GetByKeyCommand<ShoppingList>(shoppingList.Key));
                    Assert.Equal(key, retrievedShoppingList.Key);
                });
        }


        [Fact]
        public async Task QueryShoppingLists()
        {
            var key = $"QueryShoppingLists-{TestingUtility.RandomString()}";
            await WithShoppingList(
                client, shoppingListDraft => DefaultShoppingListDraftWithKey(shoppingListDraft, key),
                async shoppingList =>
                {
                    var queryCommand = new QueryCommand<ShoppingList>();
                    queryCommand.Where(p => p.Key == shoppingList.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteShoppingListById()
        {
            var key = $"DeleteShoppingListById-{TestingUtility.RandomString()}";
            await WithShoppingList(
                client, shoppingListDraft => DefaultShoppingListDraftWithKey(shoppingListDraft, key),
                async shoppingList =>
                {
                    await client.ExecuteAsync(new DeleteByIdCommand<ShoppingList>(shoppingList));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<ShoppingList>(shoppingList))
                    );
                });
        }

        [Fact]
        public async Task DeleteShoppingListByKey()
        {
            var key = $"DeleteShoppingListByKey-{TestingUtility.RandomString()}";
            await WithShoppingList(
                client, shoppingListDraft => DefaultShoppingListDraftWithKey(shoppingListDraft, key),
                async shoppingList =>
                {
                    await client.ExecuteAsync(new DeleteByKeyCommand<ShoppingList>(shoppingList.Key, shoppingList.Version));
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<ShoppingList>(shoppingList))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateShoppingListChangeKey()
        {
            var newKey = $"UpdateShoppingListSetKey-{TestingUtility.RandomString()}";
            await WithUpdateableShoppingList(client, async shoppingList =>
            {
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new SetKeyUpdateAction
                {
                    Key = newKey
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Equal(newKey, updatedShoppingList.Key);
                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingListByKeySetSlug()
        {
            await WithUpdateableShoppingList(client, async shoppingList =>
            {
                var newSlug = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new SetSlugUpdateAction
                {
                    Slug = newSlug
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByKeyCommand<ShoppingList>(shoppingList.Key, shoppingList.Version, updateActions));

                Assert.Equal(newSlug["en"], updatedShoppingList.Slug["en"]);
                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingListByKeyChangeName()
        {
            await WithUpdateableShoppingList(client, async shoppingList =>
            {
                var newName = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new ChangeNameUpdateAction
                {
                    Name = newName
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByKeyCommand<ShoppingList>(shoppingList.Key, shoppingList.Version, updateActions));

                Assert.Equal(newName["en"], updatedShoppingList.Name["en"]);
                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetDescription()
        {
            await WithUpdateableShoppingList(client, async shoppingList =>
            {
                var newDescription = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new SetDescriptionUpdateAction
                {
                    Description = newDescription
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Equal(newDescription["en"], updatedShoppingList.Description["en"]);
                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetCustomer()
        {
            await WithCustomer(client, async customer =>
            {
                await WithUpdateableShoppingList(client,
                    async shoppingList =>
                    {
                        var updateActions = new List<UpdateAction<ShoppingList>>();
                        var setCustomerAction = new SetCustomerUpdateAction
                        {
                            Customer = customer.ToKeyResourceIdentifier()
                        };
                        updateActions.Add(setCustomerAction);
                        var updateCommand = new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions);
                        updateCommand.Expand(r => r.Customer);

                        var updatedShoppingList = await client.ExecuteAsync(updateCommand);

                        Assert.NotNull(updatedShoppingList.Customer.Obj);
                        Assert.Equal(customer.Key, updatedShoppingList.Customer.Obj.Key);
                        return updatedShoppingList;
                    });
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetAnonymousId()
        {
            await WithUpdateableShoppingList(client, async shoppingList =>
            {
                var anonymousId = TestingUtility.RandomString();
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new SetAnonymousIdUpdateAction
                {
                    AnonymousId = anonymousId
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Equal(anonymousId, updatedShoppingList.AnonymousId);
                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableShoppingList(client,
                    async shoppingList =>
                    {
                        var updateActions = new List<UpdateAction<ShoppingList>>();
                        var setTypeAction = new SetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };
                        updateActions.Add(setTypeAction);

                        var updatedShoppingList = await client.ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                        Assert.Equal(type.Id, updatedShoppingList.Custom.Type.Id);
                        return updatedShoppingList;
                    });
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString();

            await WithType(client, async type =>
            {
                await WithUpdateableShoppingList(client,
                    shoppingListDraft => DefaultShoppingListDraftWithCustomType(shoppingListDraft, type, fields),
                    async shoppingList =>
                    {
                        var updateActions = new List<UpdateAction<ShoppingList>>();
                        var action = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };
                        updateActions.Add(action);

                        var updatedShoppingList = await client.ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                        Assert.Equal(newValue, updatedShoppingList.Custom.Fields["string-field"]);
                        return updatedShoppingList;
                    });
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetDeleteDaysAfterLastModification()
        {
            await WithUpdateableShoppingList(client, async shoppingList =>
            {
                var days = TestingUtility.RandomInt(1, 30);
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new SetDeleteDaysAfterLastModificationUpdateAction
                {
                    DeleteDaysAfterLastModification = days
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Equal(days, updatedShoppingList.DeleteDaysAfterLastModification);
                return updatedShoppingList;
            });
        }



        [Fact]
        public async Task UpdateShoppingListAddLineItem()
        {
            await WithProduct(client, async product =>
            {
                await WithUpdateableShoppingList(client, async shoppingList =>
                {
                    var variantId = product.MasterData.Staged.MasterVariant.Id;
                    var quantity = 2;
                    var updateActions = new List<UpdateAction<ShoppingList>>();
                    var action = new AddLineItemUpdateAction
                    {
                        ProductId = product.Id,
                        VariantId = variantId,
                        Quantity = quantity
                    };
                    updateActions.Add(action);

                    var updatedShoppingList = await client
                        .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                    Assert.Single(updatedShoppingList.LineItems);
                    var addedLineItem = updatedShoppingList.LineItems[0];
                    Assert.Equal(product.Id, addedLineItem.ProductId);
                    Assert.Equal(variantId, addedLineItem.VariantId);
                    Assert.Equal(quantity, addedLineItem.Quantity);
                    return updatedShoppingList;
                });

            });
        }

        [Fact]
        public async Task UpdateShoppingListRemoveLineItem()
        {
            await WithUpdateableShoppingListWithLineItem(client, 3, async shoppingList =>
            {
                Assert.Single(shoppingList.LineItems);
                var lineItem = shoppingList.LineItems[0];
                Assert.Equal(3, lineItem.Quantity);

                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new RemoveLineItemUpdateAction
                {
                    LineItemId = lineItem.Id,
                    Quantity = 2
                };
                updateActions.Add(action);

                var updatedShoppingListWithRemovedLineItem = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Single(updatedShoppingListWithRemovedLineItem.LineItems);
                lineItem = updatedShoppingListWithRemovedLineItem.LineItems[0];
                Assert.Equal(1, lineItem.Quantity);

                action.Quantity = 1;
                updateActions.Clear();
                updateActions.Add(action);
                var shoppingListWithoutLineItems = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(updatedShoppingListWithRemovedLineItem, updateActions));

                Assert.Empty(shoppingListWithoutLineItems.LineItems);
                return updatedShoppingListWithRemovedLineItem;
            });
        }

        [Fact]
        public async Task UpdateShoppingChangeLineItemQuantity()
        {
            await WithUpdateableShoppingListWithLineItem(client, 1, async shoppingList =>
            {
                Assert.Single(shoppingList.LineItems);
                var lineItem = shoppingList.LineItems[0];
                Assert.Equal(1, lineItem.Quantity);

                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new ChangeLineItemQuantityUpdateAction
                {
                    LineItemId = lineItem.Id,
                    Quantity = 3
                };
                updateActions.Add(action);

                var updatedShoppingListWithMoreQuantity = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Single(updatedShoppingListWithMoreQuantity.LineItems);
                lineItem = updatedShoppingListWithMoreQuantity.LineItems[0];
                Assert.Equal(3, lineItem.Quantity);

                action.Quantity = 0;
                updateActions.Clear();
                updateActions.Add(action);
                var shoppingListWithoutLineItems = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(updatedShoppingListWithMoreQuantity, updateActions));

                Assert.Empty(shoppingListWithoutLineItems.LineItems);
                return shoppingListWithoutLineItems;
            });
        }

        [Fact]
        public async Task UpdateShoppingChangeLineItemsOrder()
        {
            await WithUpdateableShoppingListWithLineItems(client, async shoppingList =>
            {
                Assert.True(shoppingList.LineItems.Count > 1);
                var newLineItemsOrder = shoppingList.LineItems.Select(lineItem => lineItem.Id).ToList();
                newLineItemsOrder.Reverse();
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new ChangeLineItemOrderUpdateAction
                {
                    LineItemOrder = newLineItemsOrder
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                var updatedLineItemsOrder = updatedShoppingList.LineItems.Select(lineItem => lineItem.Id).ToList();
                Assert.True(newLineItemsOrder.SequenceEqual(updatedLineItemsOrder));
                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetLineItemCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableShoppingListWithLineItem(client, 3, async shoppingList =>
                {
                    Assert.Single(shoppingList.LineItems);
                    var lineItem = shoppingList.LineItems[0];
                    Assert.Null(lineItem.Custom);

                    var updateActions = new List<UpdateAction<ShoppingList>>();
                    var action = new SetLineItemCustomTypeUpdateAction
                    {
                        LineItemId = lineItem.Id,
                        Type = type.ToKeyResourceIdentifier(),
                        Fields = fields
                    };
                    updateActions.Add(action);

                    var updatedShoppingList = await client
                        .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                    Assert.Single(updatedShoppingList.LineItems);
                    lineItem = updatedShoppingList.LineItems[0];
                    Assert.NotNull(lineItem.Custom);
                    Assert.Equal(type.Id, lineItem.Custom.Type.Id);
                    return updatedShoppingList;
                });
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetLineItemCustomField()
        {
            var newValue = TestingUtility.RandomString();

            await WithUpdateableShoppingListWithLineItemWithCustomFields(client, 3, async shoppingList =>
            {
                Assert.Single(shoppingList.LineItems);
                var lineItem = shoppingList.LineItems[0];
                Assert.NotNull(lineItem.Custom);

                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new SetLineItemCustomFieldUpdateAction
                {
                    LineItemId = lineItem.Id,
                    Name = "string-field",
                    Value = newValue
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Single(updatedShoppingList.LineItems);
                lineItem = updatedShoppingList.LineItems[0];
                Assert.NotNull(lineItem.Custom);
                Assert.Equal(newValue, lineItem.Custom.Fields["string-field"]);
                return updatedShoppingList;
            });
        }



        [Fact]
        public async Task UpdateShoppingListAddTextLineItem()
        {
            await WithUpdateableShoppingList(client, async shoppingList =>
            {
                Assert.Empty(shoppingList.TextLineItems);
                var name = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var quantity = 2;
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new AddTextLineItemUpdateAction
                {
                    Name = name,
                    Quantity = quantity
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Single(updatedShoppingList.TextLineItems);
                var addedTextLineItem = updatedShoppingList.TextLineItems[0];
                Assert.Equal(name["en"], addedTextLineItem.Name["en"]);
                Assert.Equal(quantity, addedTextLineItem.Quantity);
                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingListRemoveTextLineItem()
        {
            await WithUpdateableShoppingListWithTextLineItem(client, 3, async shoppingList =>
            {
                Assert.Single(shoppingList.TextLineItems);
                var textLineItem = shoppingList.TextLineItems[0];
                Assert.Equal(3, textLineItem.Quantity);

                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new RemoveTextLineItemUpdateAction
                {
                    TextLineItemId = textLineItem.Id,
                    Quantity = 2
                };
                updateActions.Add(action);

                var updatedShoppingListWithRemovedTextLineItem = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Single(updatedShoppingListWithRemovedTextLineItem.TextLineItems);
                textLineItem = updatedShoppingListWithRemovedTextLineItem.TextLineItems[0];
                Assert.Equal(1, textLineItem.Quantity);

                action.Quantity = 1;
                updateActions.Clear();
                updateActions.Add(action);
                var shoppingListWithoutLineItems = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(updatedShoppingListWithRemovedTextLineItem, updateActions));

                Assert.Empty(shoppingListWithoutLineItems.TextLineItems);
                return updatedShoppingListWithRemovedTextLineItem;
            });
        }

        [Fact]
        public async Task UpdateShoppingChangeTextLineItemQuantity()
        {
            await WithUpdateableShoppingListWithTextLineItem(client, 1, async shoppingList =>
            {
                Assert.Single(shoppingList.TextLineItems);
                var textLineItem = shoppingList.TextLineItems[0];
                Assert.Equal(1,textLineItem.Quantity);

                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new ChangeTextLineItemQuantityUpdateAction
                {
                    TextLineItemId = textLineItem.Id,
                    Quantity = 3
                };
                updateActions.Add(action);

                var updatedShoppingListWithMoreQuantity = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Single(updatedShoppingListWithMoreQuantity.TextLineItems);
                textLineItem = updatedShoppingListWithMoreQuantity.TextLineItems[0];
                Assert.Equal(3, textLineItem.Quantity);

                action.Quantity = 0;
                updateActions.Clear();
                updateActions.Add(action);
                var shoppingListWithoutTextLineItems = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(updatedShoppingListWithMoreQuantity, updateActions));

                Assert.Empty(shoppingListWithoutTextLineItems.TextLineItems);
                return shoppingListWithoutTextLineItems;
            });
        }

        [Fact]
        public async Task UpdateShoppingChangeTextLineItemName()
        {
            await WithUpdateableShoppingListWithTextLineItem(client, 1, async shoppingList =>
            {
                Assert.Single(shoppingList.TextLineItems);
                var textLineItem = shoppingList.TextLineItems[0];

                var newName = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new ChangeTextLineItemNameUpdateAction
                {
                    Name = newName,
                    TextLineItemId = textLineItem.Id
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Single(updatedShoppingList.TextLineItems);
                textLineItem = updatedShoppingList.TextLineItems[0];
                Assert.Equal(newName["en"], textLineItem.Name["en"]);

                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingChangeTextLineItemDescription()
        {
            await WithUpdateableShoppingListWithTextLineItem(client, 1, async shoppingList =>
            {
                Assert.Single(shoppingList.TextLineItems);
                var textLineItem = shoppingList.TextLineItems[0];

                var newDescription = new LocalizedString {{"en", TestingUtility.RandomString()}};
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new SetTextLineItemDescriptionUpdateAction
                {
                    Description = newDescription,
                    TextLineItemId = textLineItem.Id
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Single(updatedShoppingList.TextLineItems);
                textLineItem = updatedShoppingList.TextLineItems[0];
                Assert.Equal(newDescription["en"], textLineItem.Description["en"]);

                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingChangeTextLineItemsOrder()
        {
            await WithUpdateableShoppingListWithTextLineItems(client, async shoppingList =>
            {
                Assert.True(shoppingList.TextLineItems.Count > 1);
                var newLineItemsOrder = shoppingList.TextLineItems.Select(lineItem => lineItem.Id).ToList();
                newLineItemsOrder.Reverse();
                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new ChangeTextLineItemOrderUpdateAction
                {
                    TextLineItemOrder = newLineItemsOrder
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                var updatedLineItemsOrder = updatedShoppingList.TextLineItems.Select(lineItem => lineItem.Id).ToList();
                Assert.True(newLineItemsOrder.SequenceEqual(updatedLineItemsOrder));
                return updatedShoppingList;
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetTextLineItemCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableShoppingListWithTextLineItem(client, 3, async shoppingList =>
                {
                    Assert.Single(shoppingList.TextLineItems);
                    var textLineItem = shoppingList.TextLineItems[0];
                    Assert.Null(textLineItem.Custom);

                    var updateActions = new List<UpdateAction<ShoppingList>>();
                    var action = new SetTextLineItemCustomTypeUpdateAction
                    {
                        TextLineItemId = textLineItem.Id,
                        Type = type.ToKeyResourceIdentifier(),
                        Fields = fields
                    };
                    updateActions.Add(action);

                    var updatedShoppingList = await client
                        .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                    Assert.Single(updatedShoppingList.TextLineItems);
                    textLineItem = updatedShoppingList.TextLineItems[0];
                    Assert.NotNull(textLineItem.Custom);
                    Assert.Equal(type.Id, textLineItem.Custom.Type.Id);
                    return updatedShoppingList;
                });
            });
        }

        [Fact]
        public async Task UpdateShoppingListSetTextLineItemCustomField()
        {
            var newValue = TestingUtility.RandomString();

            await WithUpdateableShoppingListWithTextLineItemWithCustomFields(client, 3, async shoppingList =>
            {
                Assert.Single(shoppingList.TextLineItems);
                var textLineItem = shoppingList.TextLineItems[0];
                Assert.NotNull(textLineItem.Custom);

                var updateActions = new List<UpdateAction<ShoppingList>>();
                var action = new SetTextLineItemCustomFieldUpdateAction
                {
                    TextLineItemId = textLineItem.Id,
                    Name = "string-field",
                    Value = newValue
                };
                updateActions.Add(action);

                var updatedShoppingList = await client
                    .ExecuteAsync(new UpdateByIdCommand<ShoppingList>(shoppingList, updateActions));

                Assert.Single(updatedShoppingList.TextLineItems);
                textLineItem = updatedShoppingList.TextLineItems[0];
                Assert.NotNull(textLineItem.Custom);
                Assert.Equal(newValue, textLineItem.Custom.Fields["string-field"]);
                return updatedShoppingList;
            });
        }

        #endregion
    }
}
