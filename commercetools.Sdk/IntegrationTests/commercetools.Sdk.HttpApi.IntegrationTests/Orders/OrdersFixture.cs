using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.HttpApi.IntegrationTests.Carts;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Orders
{
    public class OrdersFixture : ClientFixture, IDisposable
    {
        private readonly CartFixture cartFixture;
        public List<Order> OrdersToDelete { get; }

        public OrdersFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.OrdersToDelete = new List<Order>();
            this.cartFixture = new CartFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.OrdersToDelete.Reverse();
            foreach (Order order in this.OrdersToDelete)
            {
                Order deletedType = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<Order>(new Guid(order.Id),
                        order.Version)).Result;
            }
            cartFixture.Dispose();
        }

        public OrderFromCartDraft GetOrderFromCartDraft()
        {
            //Create A Cart
            Cart cart = this.cartFixture.CreateCart();
            this.cartFixture.CartToDelete.Add(cart);

            //Add at least one product to this cart

            //Then Create Order from this Cart
            OrderFromCartDraft orderFromCartDraft = new OrderFromCartDraft();
            orderFromCartDraft.Id = cart.Id;
            orderFromCartDraft.Version = cart.Version;
            orderFromCartDraft.OrderNumber = TestingUtility.RandomString(10);
            return orderFromCartDraft;
        }

        public Order CreateOrderFromCart()
        {
            return this.CreateOrderFromCart(this.GetOrderFromCartDraft());
        }

        public Order CreateOrderFromCart(OrderFromCartDraft orderFromCartDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Order order = commerceToolsClient.ExecuteAsync(new CreateCommand<Order>(orderFromCartDraft)).Result;
            return order;
        }

    }
}
