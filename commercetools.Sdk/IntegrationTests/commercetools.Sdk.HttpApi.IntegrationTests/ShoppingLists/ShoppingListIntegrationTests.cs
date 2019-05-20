using System;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.Domain.Zones;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.ShoppingLists
{
    [Collection("Integration Tests")]
    public class ShoppingListIntegrationTests : IClassFixture<ServiceProviderFixture>, IDisposable
    {
        private readonly ShoppingListFixture shoppingListFixture;

        public ShoppingListIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.shoppingListFixture = new ShoppingListFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.shoppingListFixture.Dispose();
        }

        [Fact]
        public void CreateShoppingList()
        {
            IClient commerceToolsClient = this.shoppingListFixture.GetService<IClient>();
            ShoppingListDraft shoppingListDraft = this.shoppingListFixture.GetShoppingListDraft(withLineItem:true);
            ShoppingList shoppingList = commerceToolsClient
                .ExecuteAsync(new CreateCommand<ShoppingList>(shoppingListDraft)).Result;
            this.shoppingListFixture.ShoppingListToDelete.Add(shoppingList);
            Assert.Equal(shoppingListDraft.Key, shoppingList.Key);
        }
    }
}
