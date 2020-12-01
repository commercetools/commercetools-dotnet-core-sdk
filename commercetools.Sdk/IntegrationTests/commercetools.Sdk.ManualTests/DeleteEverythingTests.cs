using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.APIExtensions;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.CustomObjects;
using commercetools.Sdk.Domain.DiscountCodes;
using commercetools.Sdk.Domain.InventoryEntries;
using commercetools.Sdk.Domain.OrderEdits;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.ProductDiscounts;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.Stores;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.Domain.Types;
using commercetools.Sdk.Domain.Zones;
using Xunit;
using static commercetools.Sdk.ManualTests.GenericFixture;

namespace commercetools.Sdk.ManualTests
{
    public class DeleteEverythingTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly IClient client;

        public DeleteEverythingTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }
        
        [Fact]
        public async Task CleanUp()
        {
            await DeleteResources<Extension>(client);
            await DeleteResources<OrderEdit>(client);
            await DeleteResources<Order>(client);
            await DeleteResources<Cart>(client);
            await DeleteResources<ShoppingList>(client);
            await DeleteResources<Review>(client);
            await DeleteResources<ProductDiscount>(client);
            await DeleteProducts(client);
            await DeleteResources<Category>(client);
            await DeleteResources<CartDiscount>(client);
            await DeleteResources<InventoryEntry>(client);
            await DeleteResources<ProductType>(client);
            await DeleteResources<TaxCategory>(client);
            await DeleteResources<DiscountCode>(client);
            await DeleteResources<CustomObjectBase>(client);
            await DeleteResources<Customer>(client);
            await DeleteResources<CustomerGroup>(client);
            await DeleteResources<ShippingMethod>(client);
            await DeleteResources<State>(client);
            await DeleteResources<Store>(client);
            await DeleteResources<Channel>(client);
            await DeleteResources<Type>(client);
            await DeleteResources<Zone>(client);
        }
    }
}