using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShoppingLists;
using Type = commercetools.Sdk.Domain.Types.Type;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
namespace commercetools.Sdk.IntegrationTests.ShoppingLists
{
    public static class ShoppingListsFixture
    {
        #region DraftBuilds
        public static ShoppingListDraft DefaultShoppingListDraft(ShoppingListDraft shoppingListDraft)
        {
            var random = TestingUtility.RandomInt();
            shoppingListDraft.Key = $"Key_{random}";
            shoppingListDraft.Name = new LocalizedString() {{"en", $"ShoppingList_{random}"}};

            return shoppingListDraft;
        }
        public static ShoppingListDraft DefaultShoppingListDraftWithKey(ShoppingListDraft draft, string key)
        {
            var shoppingListDraft = DefaultShoppingListDraft(draft);
            shoppingListDraft.Key = key;
            return shoppingListDraft;
        }

        public static ShoppingListDraft DefaultShoppingListDraftWithLineItems(ShoppingListDraft draft, Product product)
        {
            var shoppingListDraft = DefaultShoppingListDraft(draft);
            var lineItemDraft1 = new LineItemDraft{ ProductId = product.Id, Quantity = 1};
            var lineItemDraft2 = new LineItemDraft{ ProductId = product.Id, Quantity = 2};
            var lineItemDraft3 = new LineItemDraft{ ProductId = product.Id, Quantity = 3};
            shoppingListDraft.LineItems = new List<LineItemDraft>
            {
                lineItemDraft1, lineItemDraft2, lineItemDraft3
            };
            return shoppingListDraft;
        }

        public static ShoppingListDraft DefaultShoppingListDraftWithTextLineItems(ShoppingListDraft draft)
        {
            var shoppingListDraft = DefaultShoppingListDraft(draft);
            var textLineItemDraft1 = new TextLineItemDraft{ Name = new LocalizedString {{"en", $"TextLineItem1"}}, Quantity = 1};
            var textLineItemDraft2 = new TextLineItemDraft{ Name = new LocalizedString {{"en", $"TextLineItem2"}}, Quantity = 2};
            var textLineItemDraft3 = new TextLineItemDraft{ Name = new LocalizedString {{"en", $"TextLineItem3"}}, Quantity = 3};

            shoppingListDraft.TextLineItems = new List<TextLineItemDraft>
            {
                textLineItemDraft1,
                textLineItemDraft2,
                textLineItemDraft3
            };
            return shoppingListDraft;
        }

        public static ShoppingListDraft DefaultShoppingListDraftWithCustomType(ShoppingListDraft draft, Type type, Fields fields)
        {
            var customFieldsDraft = new CustomFieldsDraft
            {
                Type = type.ToKeyResourceIdentifier(),
                Fields = fields
            };

            var shoppingListDraft = DefaultShoppingListDraft(draft);
            shoppingListDraft.Custom = customFieldsDraft;

            return shoppingListDraft;
        }
        #endregion

        #region WithShoppingList

        public static async Task WithShoppingList( IClient client, Action<ShoppingList> func)
        {
            await With(client, new ShoppingListDraft(), DefaultShoppingListDraft, func);
        }
        public static async Task WithShoppingList( IClient client, Func<ShoppingListDraft, ShoppingListDraft> draftAction, Action<ShoppingList> func)
        {
            await With(client, new ShoppingListDraft(), draftAction, func);
        }

        public static async Task WithShoppingList( IClient client, Func<ShoppingList, Task> func)
        {
            await WithAsync(client, new ShoppingListDraft(), DefaultShoppingListDraft, func);
        }
        public static async Task WithShoppingList( IClient client, Func<ShoppingListDraft, ShoppingListDraft> draftAction, Func<ShoppingList, Task> func)
        {
            await WithAsync(client, new ShoppingListDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableShoppingList

        public static async Task WithUpdateableShoppingList(IClient client, Func<ShoppingList, ShoppingList> func)
        {
            await WithUpdateable(client, new ShoppingListDraft(), DefaultShoppingListDraft, func);
        }

        public static async Task WithUpdateableShoppingList(IClient client, Func<ShoppingListDraft, ShoppingListDraft> draftAction, Func<ShoppingList, ShoppingList> func)
        {
            await WithUpdateable(client, new ShoppingListDraft(), draftAction, func);
        }

        public static async Task WithUpdateableShoppingList(IClient client, Func<ShoppingList, Task<ShoppingList>> func)
        {
            await WithUpdateableAsync(client, new ShoppingListDraft(), DefaultShoppingListDraft, func);
        }

        public static async Task WithUpdateableShoppingListWithLineItems(IClient client, Func<ShoppingList, Task<ShoppingList>> func)
        {
            await WithProduct(client, async product =>
            {
                var shoppingListDraft = DefaultShoppingListDraftWithLineItems(new ShoppingListDraft(), product);
                await WithUpdateableAsync(client, shoppingListDraft, DefaultShoppingListDraft, func);
            });
        }

        public static async Task WithUpdateableShoppingListWithTextLineItems(IClient client, Func<ShoppingList, Task<ShoppingList>> func)
        {
            var shoppingListDraft = DefaultShoppingListDraftWithTextLineItems(new ShoppingListDraft());
            await WithUpdateableAsync(client, shoppingListDraft, DefaultShoppingListDraft, func);
        }

        public static async Task WithUpdateableShoppingListWithLineItem(IClient client, long quantity, Func<ShoppingList, Task<ShoppingList>> func)
        {
            await WithProduct(client, async product =>
            {
                var lineItemDraft = new LineItemDraft
                {
                    Quantity = quantity,
                    ProductId = product.Id
                };
                var shoppingListDraft = new ShoppingListDraft();
                shoppingListDraft.LineItems = new List<LineItemDraft> { lineItemDraft };
                await WithUpdateableAsync(client, shoppingListDraft, DefaultShoppingListDraft, func);
            });
        }

        public static async Task WithUpdateableShoppingListWithTextLineItem(IClient client, long quantity, Func<ShoppingList, Task<ShoppingList>> func)
        {
            var textLineItemDraft = new TextLineItemDraft {
                Quantity = quantity,
                Name = new LocalizedString {{"en", TestingUtility.RandomString()}}
            };
            var shoppingListDraft = new ShoppingListDraft();
            shoppingListDraft.TextLineItems = new List<TextLineItemDraft> { textLineItemDraft };
            await WithUpdateableAsync(client, shoppingListDraft, DefaultShoppingListDraft, func);
        }

        public static async Task WithUpdateableShoppingListWithLineItemWithCustomFields(IClient client, long quantity, Func<ShoppingList, Task<ShoppingList>> func)
        {
            await WithType(client, async type =>
            {
                await WithProduct(client, async product =>
                {
                    var fields = CreateNewFields();
                    var customFieldsDraft = new CustomFieldsDraft
                    {
                        Type = type.ToKeyResourceIdentifier(),
                        Fields = fields
                    };
                    var lineItemDraft = new LineItemDraft
                    {
                        Quantity = quantity,
                        ProductId = product.Id,
                        Custom = customFieldsDraft
                    };
                    var shoppingListDraft = new ShoppingListDraft();
                    shoppingListDraft.LineItems = new List<LineItemDraft> { lineItemDraft };
                    await WithUpdateableAsync(client, shoppingListDraft, DefaultShoppingListDraft, func);
                });
            });
        }

        public static async Task WithUpdateableShoppingListWithTextLineItemWithCustomFields(IClient client, long quantity, Func<ShoppingList, Task<ShoppingList>> func)
        {
            await WithType(client, async type =>
            {
                var fields = CreateNewFields();
                var customFieldsDraft = new CustomFieldsDraft
                {
                    Type = type.ToKeyResourceIdentifier(),
                    Fields = fields
                };
                var textLineItemDraft = new TextLineItemDraft
                {
                    Quantity = quantity,
                    Name = new LocalizedString {{"en", TestingUtility.RandomString()}},
                    Custom = customFieldsDraft
                };
                var shoppingListDraft = new ShoppingListDraft();
                shoppingListDraft.TextLineItems = new List<TextLineItemDraft> { textLineItemDraft };
                await WithUpdateableAsync(client, shoppingListDraft, DefaultShoppingListDraft, func);
            });
        }

        public static async Task WithUpdateableShoppingList(IClient client, Func<ShoppingListDraft, ShoppingListDraft> draftAction, Func<ShoppingList, Task<ShoppingList>> func)
        {
            await WithUpdateableAsync(client, new ShoppingListDraft(), draftAction, func);
        }

        #endregion
    }
}
