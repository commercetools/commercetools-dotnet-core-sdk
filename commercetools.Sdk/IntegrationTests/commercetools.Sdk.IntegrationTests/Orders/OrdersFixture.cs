using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Orders;
using static commercetools.Sdk.IntegrationTests.GenericFixture;
using static commercetools.Sdk.IntegrationTests.Carts.CartsFixture;

namespace commercetools.Sdk.IntegrationTests.Orders
{
    public class OrdersFixture
    {
        #region DraftBuilds

        public static OrderFromCartDraft DefaultOrderFromCartDraft(OrderFromCartDraft orderFromCartDraft, Cart cart)
        {
            var rand = TestingUtility.RandomInt();
            orderFromCartDraft.Id = cart.Id;
            orderFromCartDraft.Version = cart.Version;
            orderFromCartDraft.OrderNumber = $"Order_{rand}";
            return orderFromCartDraft;
        }
        public static OrderFromCartDraft DefaultOrderFromCartDraftWithNumber(OrderFromCartDraft draft, Cart cart, string orderNumber)
        {
            var orderFromCartDraft = DefaultOrderFromCartDraft(draft, cart);
            orderFromCartDraft.OrderNumber = orderNumber;
            return orderFromCartDraft;
        }
        
        public static OrderFromCartDraft DefaultOrderFromCartDraftWithOrderState(OrderFromCartDraft draft, Cart cart, OrderState orderState)
        {
            var orderFromCartDraft = DefaultOrderFromCartDraft(draft, cart);
            orderFromCartDraft.OrderState = orderState;
            return orderFromCartDraft;
        }
        
        public static OrderFromCartDraft DefaultOrderFromCartDraftWithShipmentState(OrderFromCartDraft draft, Cart cart, ShipmentState shipmentState)
        {
            var orderFromCartDraft = DefaultOrderFromCartDraft(draft, cart);
            orderFromCartDraft.ShipmentState = shipmentState;
            return orderFromCartDraft;
        }
        
        public static OrderFromCartDraft DefaultOrderFromCartDraftWithPaymentState(OrderFromCartDraft draft, Cart cart, PaymentState paymentState)
        {
            var orderFromCartDraft = DefaultOrderFromCartDraft(draft, cart);
            orderFromCartDraft.PaymentState = paymentState;
            return orderFromCartDraft;
        }
        #endregion
      
        #region WithOrder

        public static async Task WithOrder( IClient client, Func<OrderFromCartDraft, OrderFromCartDraft> draftAction, Action<Order> func)
        {
            await With(client, new OrderFromCartDraft(), draftAction, func);
        }
        public static async Task WithOrder( IClient client, Func<OrderFromCartDraft, OrderFromCartDraft> draftAction, Func<Order, Task> func)
        {
            await WithAsync(client, new OrderFromCartDraft(), draftAction, func);
        }
        public static async Task WithListOfOrders( IClient client, List<Func<OrderFromCartDraft, OrderFromCartDraft>> draftsActions, Func<List<Order>, Task> func)
        {
            await WithListAsync(client, new OrderFromCartDraft(), draftsActions, func);
        }
        #endregion

        #region WithUpdateableOrder
       
        public static async Task WithUpdateableOrder(IClient client, Func<OrderFromCartDraft, OrderFromCartDraft> draftAction, Func<Order, Task<Order>> func)
        {
            await WithUpdateableAsync(client, new OrderFromCartDraft(), draftAction, func);
        }
       
        #endregion
    }
}