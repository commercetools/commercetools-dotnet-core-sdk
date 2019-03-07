using System;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Orders;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Orders
{
    [Collection("Integration Tests")]
    public class OrdersIntegrationTests : IClassFixture<OrdersFixture>
    {
        private readonly OrdersFixture ordersFixture;

        public OrdersIntegrationTests(OrdersFixture ordersFixture)
        {
            this.ordersFixture = ordersFixture;
        }

        [Fact]
        public void CreateOrderFromCart()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            OrderFromCartDraft orderFromCartDraft = this.ordersFixture.GetOrderFromCartDraft();
            Order order = commerceToolsClient
                .ExecuteAsync(new CreateCommand<Order>(orderFromCartDraft)).Result;
            this.ordersFixture.OrdersToDelete.Add(order);
            Assert.Equal(order.OrderNumber, orderFromCartDraft.OrderNumber);
        }
        
        [Fact]
        public void GetOrderById()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            string orderId = "88505634-a81a-4cbe-92a9-7796be230a1f";
            Order order = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<Order>(new Guid(orderId))).Result;
        }
        [Fact]
        public async void DeleteOrderById()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            string orderId = "88505634-a81a-4cbe-92a9-7796be230a1f";
            
            Order order = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<Order>(new Guid(orderId))).Result;
            
            Order deletedOrder = commerceToolsClient
                .ExecuteAsync(
                    new DeleteByIdCommand<Order>(new Guid(orderId), order.Version))
                .Result;
        }
    }
}