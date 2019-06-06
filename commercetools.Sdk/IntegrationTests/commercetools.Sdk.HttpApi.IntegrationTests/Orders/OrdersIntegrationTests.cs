using System;
using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Orders.UpdateActions;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Orders
{
    [Collection("Integration Tests")]
    public class OrdersIntegrationTests : IClassFixture<ServiceProviderFixture>, IDisposable
    {
        private readonly OrdersFixture ordersFixture;

        public OrdersIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.ordersFixture = new OrdersFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.ordersFixture.Dispose();
        }

        [Fact]
        public void CreateOrderFromCart()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            OrderFromCartDraft orderFromCartDraft = this.ordersFixture.GetOrderFromCartDraft();
            Order order = commerceToolsClient
                .ExecuteAsync(new CreateCommand<Order>(orderFromCartDraft)).Result;
            this.ordersFixture.AddCartToDeletedCarts(order.Cart.Id);
            this.ordersFixture.OrdersToDelete.Add(order);
            Assert.Equal(order.OrderNumber, orderFromCartDraft.OrderNumber);
        }

        [Fact]
        public void CreateOrderByImportBySku()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var orderImportDraft = this.ordersFixture.GetOrderImportDraft(bySku: true);
            var importOrderCommand = new ImportOrderCommand(orderImportDraft);
            Order order = commerceToolsClient
                .ExecuteAsync(importOrderCommand).Result;

            this.ordersFixture.OrdersToDelete.Add(order);

            Assert.Null(order.Cart);
            Assert.Equal(orderImportDraft.OrderNumber, order.OrderNumber);
            Assert.Equal(orderImportDraft.TotalPrice, order.TotalPrice);
            Assert.Single(order.LineItems);

            var lineItem = order.LineItems[0];
            Assert.Equal(orderImportDraft.LineItems[0].Name["en"], lineItem.Name["en"]);
            Assert.Equal(orderImportDraft.LineItems[0].Quantity, lineItem.Quantity);
            Assert.Equal(orderImportDraft.LineItems[0].Variant.Sku, lineItem.Variant.Sku);
        }

        [Fact]
        public void CreateOrderByImportByProductId()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var orderImportDraft = this.ordersFixture.GetOrderImportDraft(bySku: false);
            var importOrderCommand = new ImportOrderCommand(orderImportDraft);
            Order order = commerceToolsClient
                .ExecuteAsync(importOrderCommand).Result;

            this.ordersFixture.OrdersToDelete.Add(order);

            Assert.Null(order.Cart);
            Assert.Equal(orderImportDraft.OrderNumber, order.OrderNumber);
            Assert.Equal(orderImportDraft.TotalPrice, order.TotalPrice);
            Assert.Single(order.LineItems);

            var lineItem = order.LineItems[0];
            Assert.Equal(orderImportDraft.LineItems[0].Name["en"], lineItem.Name["en"]);
            Assert.Equal(orderImportDraft.LineItems[0].Quantity, lineItem.Quantity);
            Assert.Equal(orderImportDraft.LineItems[0].Variant.ProductId, lineItem.ProductId);
            Assert.Equal(orderImportDraft.LineItems[0].Variant.Id, lineItem.Variant.Id);
        }

        [Fact]
        public void GetOrderById()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            Order order = this.ordersFixture.CreateOrderFromCart();
            this.ordersFixture.OrdersToDelete.Add(order);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<Order>(order.Id)).Result;
            Assert.Equal(order.Id, retrievedOrder.Id);
        }

        [Fact]
        public void GetOrderByOrderNumber()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            Order order = this.ordersFixture.CreateOrderFromCart();
            this.ordersFixture.OrdersToDelete.Add(order);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new GetOrderByOrderNumberCommand(order.OrderNumber)).Result;
            Assert.Equal(order.OrderNumber, retrievedOrder.OrderNumber);
        }

        [Fact]
        public void QueryOrders()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            Order order = this.ordersFixture.CreateOrderFromCart();
            this.ordersFixture.OrdersToDelete.Add(order);

            var queryCommand = new QueryCommand<Order>();
            queryCommand.Where(o => o.OrderNumber == order.OrderNumber.valueOf());
            var returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;

            Assert.Contains(returnedSet.Results, o => o.OrderNumber == order.OrderNumber);
        }

        [Fact]
        public void QueryOrdersAndExpandCart()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            Order order = this.ordersFixture.CreateOrderFromCart();
            this.ordersFixture.OrdersToDelete.Add(order);

            var queryCommand = new QueryCommand<Order>();
            queryCommand.Where(o => o.OrderNumber == order.OrderNumber.valueOf());
            queryCommand.Expand(o => o.Cart);
            var returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;

            Assert.Contains(returnedSet.Results, o => o.OrderNumber == order.OrderNumber && o.Cart.Obj != null);
        }

        [Fact]
        public void QueryAndSortOrders()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            for (var i = 0; i < 3; i++)
            {
                Order order = this.ordersFixture.CreateOrderFromCart();
                this.ordersFixture.OrdersToDelete.Add(order);
            }

            var queryCommand = new QueryCommand<Order>();
            queryCommand.Sort(o => o.OrderNumber);
            var returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;

            var sortedList = returnedSet.Results.OrderBy(o => o.OrderNumber);
            Assert.True(sortedList.SequenceEqual(returnedSet.Results));
        }

        [Fact]
        public void QueryAndLimitOrders()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            for (var i = 0; i < 3; i++)
            {
                Order order = this.ordersFixture.CreateOrderFromCart();
                this.ordersFixture.OrdersToDelete.Add(order);
            }

            var queryCommand = new QueryCommand<Order>();
            queryCommand.SetLimit(2);
            queryCommand.SetWithTotal(true);
            var returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;

            Assert.Equal(2, returnedSet.Results.Count);
            Assert.True(returnedSet.Total > 0);
        }

        [Fact]
        public async void DeleteOrderById()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(
                    new DeleteByIdCommand<Order>(order.Id, order.Version))
                .Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<Order>(order.Id)));
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public async void DeleteOrderByOrderNumber()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(
                    new DeleteByOrderNumberCommand(order.OrderNumber, order.Version))
                .Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<Order>(order.Id)));
            Assert.Equal(404, exception.StatusCode);
        }

        #region UpdateActions

        [Fact]
        public void UpdateOrderByOrderNumberChangeOrderState()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            Assert.Equal(OrderState.Open, order.OrderState);

            var newOrderState = OrderState.Complete;
            var updateActions = new List<UpdateAction<Order>>();
            var changeOrderStateAction = new ChangeOrderStateUpdateAction
            {
                OrderState = newOrderState
            };
            updateActions.Add(changeOrderStateAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByOrderNumberCommand(order.OrderNumber, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Equal(newOrderState, retrievedOrder.OrderState);
        }

        [Fact]
        public void UpdateOrderByOrderNumberChangeShipmentState()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            Assert.Equal(ShipmentState.Shipped, order.ShipmentState);

            var newShipmentState = ShipmentState.Delayed;
            var updateActions = new List<UpdateAction<Order>>();
            var changeShipmentStateAction = new ChangeShipmentStateUpdateAction
            {
                ShipmentState = newShipmentState
            };
            updateActions.Add(changeShipmentStateAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByOrderNumberCommand(order.OrderNumber, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Equal(newShipmentState, retrievedOrder.ShipmentState);
        }

        [Fact]
        public void UpdateOrderByIdChangePaymentState()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            var newPaymentState = PaymentState.Pending;
            var updateActions = new List<UpdateAction<Order>>();
            var changePaymentStateAction = new ChangePaymentStateUpdateAction
            {
                PaymentState = newPaymentState
            };
            updateActions.Add(changePaymentStateAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Equal(newPaymentState, retrievedOrder.PaymentState);
        }

        [Fact]
        public void UpdateOrderUpdateSyncInfo()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();
            Assert.Empty(order.SyncInfo);

            var orderExportChannel = this.ordersFixture.CreateNewChannel(ChannelRole.OrderImport);
            var externalId = TestingUtility.RandomString(10);

            var updateSyncInfoAction = new UpdateSyncInfoUpdateAction
            {
                Channel = new ResourceIdentifier<Channel>
                {
                    Key = orderExportChannel.Key
                },
                ExternalId = externalId
            };
            var updateActions = new List<UpdateAction<Order>>{ updateSyncInfoAction };

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Single(retrievedOrder.SyncInfo);
            var syncInfo = retrievedOrder.SyncInfo[0];
            Assert.Equal(orderExportChannel.Id, syncInfo.Channel.Id);
            Assert.Equal(externalId, syncInfo.ExternalId);
        }

        [Fact]
        public void UpdateOrderByIdAddReturnInfo()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();
            Assert.Empty(order.ReturnInfo);

            var lineItemId = order.LineItems[0].Id;
            var lineItemReturnItemDraft = this.ordersFixture.GetLineItemReturnItemDraft(lineItemId);
            var returnTrackingId = TestingUtility.RandomString(10);

            var updateActions = new List<UpdateAction<Order>>();
            var addReturnInfoUpdateAction = new AddReturnInfoUpdateAction
            {
                Items = new List<ReturnItemDraft> {lineItemReturnItemDraft},
                ReturnTrackingId = returnTrackingId
            };
            updateActions.Add(addReturnInfoUpdateAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Single(retrievedOrder.ReturnInfo);
            var returnInfo = retrievedOrder.ReturnInfo[0];

            Assert.Single(returnInfo.Items);
            Assert.Equal(returnTrackingId, returnInfo.ReturnTrackingId);
            Assert.Equal(lineItemReturnItemDraft.ShipmentState, returnInfo.Items[0].ShipmentState);
            Assert.IsType<LineItemReturnItem>(returnInfo.Items[0]);

            var item = returnInfo.Items[0] as LineItemReturnItem;
            Assert.NotNull(item);
            Assert.Equal(lineItemId, item.LineItemId);
        }

        [Fact]
        public void UpdateOrderSetReturnShipmentState()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withReturnInfo: true);
            Assert.Single(order.ReturnInfo);
            Assert.Single(order.ReturnInfo[0].Items);

            var returnItemId = order.ReturnInfo[0].Items[0].Id;
            var returnShipmentState = ReturnShipmentState.BackInStock;

            Assert.NotEqual(returnShipmentState, order.ReturnInfo[0].Items[0].ShipmentState);

            var updateActions = new List<UpdateAction<Order>>();
            var setReturnShipmentStateAction = new SetShipmentReturnStateUpdateAction
            {
                ReturnItemId = returnItemId,
                ShipmentState = returnShipmentState
            };
            updateActions.Add(setReturnShipmentStateAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Single(retrievedOrder.ReturnInfo);
            Assert.Single(retrievedOrder.ReturnInfo[0].Items);
            Assert.Equal(returnShipmentState, retrievedOrder.ReturnInfo[0].Items[0].ShipmentState);
        }

        [Fact]
        public void UpdateOrderSetReturnPaymentState()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withReturnInfo: true);
            Assert.Single(order.ReturnInfo);
            Assert.Single(order.ReturnInfo[0].Items);

            var returnItemId = order.ReturnInfo[0].Items[0].Id;
            var returnPaymentState = ReturnPaymentState.NotRefunded;

            Assert.NotEqual(returnPaymentState, order.ReturnInfo[0].Items[0].PaymentState);

            var updateActions = new List<UpdateAction<Order>>();
            var setReturnPaymentStateAction = new SetReturnPaymentStateUpdateAction
            {
                ReturnItemId = returnItemId,
                PaymentState = returnPaymentState
            };
            updateActions.Add(setReturnPaymentStateAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Single(retrievedOrder.ReturnInfo);
            Assert.Single(retrievedOrder.ReturnInfo[0].Items);
            Assert.Equal(returnPaymentState, retrievedOrder.ReturnInfo[0].Items[0].PaymentState);
        }

        [Fact]
        public void UpdateOrderChangeStateOfLineItem()
        {
            var states = this.ordersFixture.GetStandardStates();
            Assert.Equal(2, states.Count);
            var initialState = states.FirstOrDefault(state => state.Initial);
            var nextState = states.FirstOrDefault(state => !state.Initial);
            Assert.NotNull(initialState);
            Assert.NotNull(nextState);

            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            Assert.Single(order.LineItems);
            var lineItem = order.LineItems[0];
            var quantity = 1;

            var updateActions = new List<UpdateAction<Order>>();
            var changeStateOfLineItemAction = new TransitionLineItemStateUpdateAction
            {
                Quantity = quantity,
                LineItemId = lineItem.Id,
                FromState = new Reference<State> { Id = initialState.Id},
                ToState = new Reference<State> { Id = nextState.Id},
                ActualTransitionDate = DateTime.Now.AddSeconds(-500)
            };
            updateActions.Add(changeStateOfLineItemAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Single(retrievedOrder.LineItems);
            var updatedLineItem = retrievedOrder.LineItems[0];
            var lineItemNextState = updatedLineItem.State.FirstOrDefault(s => s.State.Id == nextState.Id);
            Assert.NotNull(lineItemNextState);
            Assert.Equal(quantity, lineItemNextState.Quantity);
        }

        [Fact]
        public void UpdateOrderChangeStateOfCustomLineItem()
        {
            var states = this.ordersFixture.GetStandardStates();
            Assert.Equal(2, states.Count);
            var initialState = states.FirstOrDefault(state => state.Initial);
            var nextState = states.FirstOrDefault(state => !state.Initial);
            Assert.NotNull(initialState);
            Assert.NotNull(nextState);

            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withCustomLineItem:true);

            Assert.Single(order.CustomLineItems);
            var customLineItem = order.CustomLineItems[0];
            var quantity = 1;

            var updateActions = new List<UpdateAction<Order>>();
            var changeStateOfLineItemAction = new TransitionCustomLineItemStateUpdateAction
            {
                Quantity = quantity,
                CustomLineItemId = customLineItem.Id,
                FromState = new Reference<State> { Id = initialState.Id},
                ToState = new Reference<State> { Id = nextState.Id},
                ActualTransitionDate = DateTime.Now.AddSeconds(-500)
            };
            updateActions.Add(changeStateOfLineItemAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Single(retrievedOrder.CustomLineItems);
            var updatedCustomLineItem = retrievedOrder.CustomLineItems[0];
            var customLineItemNextState = updatedCustomLineItem.State.FirstOrDefault(s => s.State.Id == nextState.Id);
            Assert.NotNull(customLineItemNextState);
            Assert.Equal(quantity, customLineItemNextState.Quantity);
        }

        [Fact]
        public void UpdateOrderImportStateForLineItems()
        {
            var states = this.ordersFixture.GetStandardStates();
            var initialState = states.FirstOrDefault(state => state.Initial);
            var nextState = states.FirstOrDefault(state => !state.Initial);
            Assert.NotNull(initialState);
            Assert.NotNull(nextState);

            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            Assert.Single(order.LineItems);
            var lineItem = order.LineItems[0];

            var itemStates = new List<ItemState>
            {
                new ItemState { State = new Reference<State> { Id = nextState.Id}, Quantity = 1},
                new ItemState { State = new Reference<State> { Id = initialState.Id}, Quantity = lineItem.Quantity -1}
            };
            var updateActions = new List<UpdateAction<Order>>();
            var importLineItemStateAction = new ImportLineItemStateUpdateAction
            {
                LineItemId = lineItem.Id,
                State =itemStates
            };
            updateActions.Add(importLineItemStateAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Single(retrievedOrder.LineItems);
            Assert.Equal(2, retrievedOrder.LineItems[0].State.Count);
            Assert.Equal(nextState.Id, retrievedOrder.LineItems[0].State[0].State.Id);
            Assert.Equal(1, retrievedOrder.LineItems[0].State[0].Quantity);
            Assert.Equal(initialState.Id, retrievedOrder.LineItems[0].State[1].State.Id);
            Assert.Equal(lineItem.Quantity-1, retrievedOrder.LineItems[0].State[1].Quantity);
        }

        [Fact]
        public void UpdateOrderImportStateForCustomLineItems()
        {
            var states = this.ordersFixture.GetStandardStates();
            var initialState = states.FirstOrDefault(state => state.Initial);
            var nextState = states.FirstOrDefault(state => !state.Initial);
            Assert.NotNull(initialState);
            Assert.NotNull(nextState);

            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withCustomLineItem:true);

            Assert.Single(order.CustomLineItems);
            var customLineItem = order.CustomLineItems[0];

            var itemStates = new List<ItemState>
            {
                new ItemState { State = new Reference<State> { Id = nextState.Id}, Quantity = 1},
                new ItemState { State = new Reference<State> { Id = initialState.Id}, Quantity = customLineItem.Quantity -1}
            };
            var updateActions = new List<UpdateAction<Order>>();
            var importCustomLineItemStateAction = new ImportCustomLineItemStateUpdateAction
            {
                CustomLineItemId = customLineItem.Id,
                State =itemStates
            };
            updateActions.Add(importCustomLineItemStateAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Single(retrievedOrder.CustomLineItems);
            Assert.Equal(2, retrievedOrder.CustomLineItems[0].State.Count);
            Assert.Equal(nextState.Id, retrievedOrder.CustomLineItems[0].State[0].State.Id);
            Assert.Equal(1, retrievedOrder.CustomLineItems[0].State[0].Quantity);
            Assert.Equal(initialState.Id, retrievedOrder.CustomLineItems[0].State[1].State.Id);
            Assert.Equal(customLineItem.Quantity-1, retrievedOrder.CustomLineItems[0].State[1].Quantity);
        }

        [Fact]
        public void UpdateOrderAddDelivery()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withDefaultShippingCountry: false,
                withShippingMethod: true);

            Assert.NotNull(order.ShippingInfo);
            Assert.Empty(order.ShippingInfo.Deliveries);

            var lineItemId = order.LineItems[0].Id;
            var quantity = 1;

            var deliveryItems = new List<DeliveryItem>{ new DeliveryItem { Id = lineItemId, Quantity = quantity } };
            var smallParcelMeasurements = new ParcelMeasurements(1,2,3,4);
            var trackingData = new TrackingData("tracking-id-12", "carrier xyz", "provider foo", "prov trans 56",false);
            var parcels = new List<ParcelDraft>
            {
                new ParcelDraft
                {
                    Measurements = smallParcelMeasurements,
                    TrackingData = trackingData,
                    Items = deliveryItems
                }
            };


            var updateActions = new List<UpdateAction<Order>>();
            var addDeliveryAction = new AddDeliveryUpdateAction
            {
                Items = deliveryItems,
                Parcels = parcels
            };
            updateActions.Add(addDeliveryAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Single(retrievedOrder.ShippingInfo.Deliveries);
            var delivery = retrievedOrder.ShippingInfo.Deliveries[0];

            Assert.Single(delivery.Items);
            Assert.Single(delivery.Parcels);
            Assert.Single(delivery.Parcels[0].Items);
            Assert.Equal(lineItemId, delivery.Items[0].Id);
            Assert.Equal(quantity, delivery.Items[0].Quantity);
            Assert.Equal(lineItemId, delivery.Parcels[0].Items[0].Id);
            Assert.Equal(quantity, delivery.Parcels[0].Items[0].Quantity);
        }

        [Fact]
        public void UpdateOrderSetDeliveryAddress()
        {
            //First Add Delivery
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withDefaultShippingCountry: false,
                withShippingMethod: true);

            Assert.NotNull(order.ShippingInfo);
            Assert.Empty(order.ShippingInfo.Deliveries);

            var retrievedOrder = this.ordersFixture.AddDelivery(order);

            Assert.Single(retrievedOrder.ShippingInfo.Deliveries);
            var delivery = retrievedOrder.ShippingInfo.Deliveries[0];

            Assert.Null(delivery.Address);

            // Then set Delivery Address
            var country = TestingUtility.GetRandomEuropeCountry();
            var address = new Address {Country = country };
            var setDeliveryAddressAction = new SetDeliveryAddressUpdateAction
            {
                Address = address,
                DeliveryId = delivery.Id
            };
            var updateActions = new List<UpdateAction<Order>> { setDeliveryAddressAction };

            var updatedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(retrievedOrder.Id, retrievedOrder.Version, updateActions))
                .Result;


            this.ordersFixture.OrdersToDelete.Add(updatedOrder);

            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
            delivery = updatedOrder.ShippingInfo.Deliveries[0];

            Assert.NotNull(delivery.Address);
            Assert.Equal(country, delivery.Address.Country);
        }

        [Fact]
        public void UpdateOrderAddParcel()
        {
            //First Add Delivery
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withDefaultShippingCountry: false,
                withShippingMethod: true);

            var quantity = 1;
            var lineItemId = order.LineItems[0].Id;
            var deliveryItems = new List<DeliveryItem>{ new DeliveryItem { Id = lineItemId, Quantity = quantity } };

            Assert.NotNull(order.ShippingInfo);
            Assert.Empty(order.ShippingInfo.Deliveries);

            var retrievedOrder = this.ordersFixture.AddDelivery(order, quantity, withParcel: false);
            Assert.Single(retrievedOrder.ShippingInfo.Deliveries);
            var delivery = retrievedOrder.ShippingInfo.Deliveries[0];
            Assert.Empty(delivery.Parcels);

            // Then Add Parcel
            var smallParcelMeasurements = new ParcelMeasurements(1,2,3,4);
            var trackingData = new TrackingData("tracking-id-12", "carrier xyz", "provider foo", "prov trans 56",false);
            var addParcelAction = new AddParcelUpdateAction
            {
                Measurements = smallParcelMeasurements,
                TrackingData = trackingData,
                DeliveryId = delivery.Id,
                Items = deliveryItems
            };
            var updateActions = new List<UpdateAction<Order>> { addParcelAction };

            var updatedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(retrievedOrder.Id, retrievedOrder.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(updatedOrder);

            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
            delivery = updatedOrder.ShippingInfo.Deliveries[0];
            Assert.Single(delivery.Parcels);
            var parcel = delivery.Parcels[0];
            Assert.Single(parcel.Items);
            Assert.Equal(lineItemId, parcel.Items[0].Id);
            Assert.Equal(quantity, parcel.Items[0].Quantity);
        }

        [Fact]
        public void UpdateOrderSetOrderNumber()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withOrderNumber:false);

            var newOrderNumber = TestingUtility.RandomString(10);
            var updateActions = new List<UpdateAction<Order>>();
            var setOrderNumberAction = new SetOrderNumberUpdateAction
            {
                OrderNumber = newOrderNumber
            };
            updateActions.Add(setOrderNumberAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Equal(newOrderNumber, retrievedOrder.OrderNumber);
        }

        [Fact]
        public void UpdateOrderTransitionToNewState()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            Assert.Null(order.State);

            var initialOrderState = this.ordersFixture.CreateNewState(StateType.OrderState, initial: true);

            Assert.NotNull(initialOrderState.Id);

            TransitionStateUpdateAction transitionStateUpdateAction = new TransitionStateUpdateAction()
            {
                State = new Reference<State>{ Id = initialOrderState.Id}
            };
            var updateActions = new List<UpdateAction<Order>> {transitionStateUpdateAction};

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.NotNull(retrievedOrder.State);
            Assert.Equal(initialOrderState.Id, retrievedOrder.State.Id);
        }

        [Fact]
        public void UpdateOrderSetCustomerEmail()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withCustomer:false);

            Assert.Null(order.CustomerId);
            var newCustomerEmail = $"{TestingUtility.RandomString(10)}@email.com";
            var updateActions = new List<UpdateAction<Order>>();
            var setCustomerEmailUpdateAction = new SetCustomerEmailUpdateAction
            {
                Email = newCustomerEmail
            };
            updateActions.Add(setCustomerEmailUpdateAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Equal(newCustomerEmail, retrievedOrder.CustomerEmail);
        }

        [Fact]
        public void UpdateOrderSetCustomerId()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withCustomer:false);

            Assert.Null(order.CustomerId);

            var customer = this.ordersFixture.CreateNewCustomer();
            var updateActions = new List<UpdateAction<Order>>();
            var setCustomerIdUpdateAction = new SetCustomerIdUpdateAction
            {
                CustomerId = customer.Id
            };
            updateActions.Add(setCustomerIdUpdateAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Equal(customer.Id, retrievedOrder.CustomerId);
        }

        [Fact]
        public void UpdateOrderSetShippingAddress()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            string country = TestingUtility.GetRandomEuropeCountry();
            string state = $"{country}_State_{TestingUtility.RandomInt()}";
            var newShippingAddress = new Address { Country = country, State = state, Key = TestingUtility.RandomString(10)};

            var updateActions = new List<UpdateAction<Order>>();
            var setShippingAddressUpdateAction = new SetShippingAddressUpdateAction
            {
                Address = newShippingAddress
            };
            updateActions.Add(setShippingAddressUpdateAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Equal(newShippingAddress.Key, retrievedOrder.ShippingAddress.Key);
            Assert.Equal(newShippingAddress.Country, retrievedOrder.ShippingAddress.Country);
            Assert.Equal(newShippingAddress.State, retrievedOrder.ShippingAddress.State);
        }

        [Fact]
        public void UpdateOrderSetBillingAddress()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            string country = TestingUtility.GetRandomEuropeCountry();
            string state = $"{country}_State_{TestingUtility.RandomInt()}";
            var newBillingAddress = new Address { Country = country, State = state, Key = TestingUtility.RandomString(10)};

            var updateActions = new List<UpdateAction<Order>>();
            var setBillingAddressUpdateAction = new SetBillingAddressUpdateAction
            {
                Address = newBillingAddress
            };
            updateActions.Add(setBillingAddressUpdateAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Equal(newBillingAddress.Key, retrievedOrder.BillingAddress.Key);
            Assert.Equal(newBillingAddress.Country, retrievedOrder.BillingAddress.Country);
            Assert.Equal(newBillingAddress.State, retrievedOrder.BillingAddress.State);
        }

        [Fact]
        public void UpdateOrderByIdSetCustomType()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            var type = this.ordersFixture.CreateNewType();
            var fields = this.ordersFixture.CreateNewFields();

            var updateActions = new List<UpdateAction<Order>>();

            var setCustomTypeUpdateAction = new SetCustomTypeUpdateAction()
            {
                Type = new ResourceIdentifier<Type> {Key = type.Key}, Fields = fields
            };
            updateActions.Add(setCustomTypeUpdateAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version,
                    updateActions)).Result;
            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Equal(type.Id, retrievedOrder.Custom.Type.Id);
        }

        [Fact]
        public void UpdateOrderByIdSetCustomField()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            //set order with custom type, fields
            var type = this.ordersFixture.CreateNewType();
            var fields = this.ordersFixture.CreateNewFields();

            var updateActions = new List<UpdateAction<Order>>();

            var setCustomTypeUpdateAction = new SetCustomTypeUpdateAction()
            {
                Type = new ResourceIdentifier<Type> {Key = type.Key}, Fields = fields
            };
            updateActions.Add(setCustomTypeUpdateAction);

            var orderWithCustomFields = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version,
                    updateActions)).Result;

            //set the custom field with new value
            string newValue = TestingUtility.RandomString(10);
            var setCustomFieldUpdateAction = new SetCustomFieldUpdateAction()
            {
                Name = "string-field", Value = newValue
            };
            updateActions.Clear();
            updateActions.Add(setCustomFieldUpdateAction);

            var orderWithUpdatedCustomFields = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(orderWithCustomFields.Id, orderWithCustomFields.Version,
                    updateActions)).Result;
            this.ordersFixture.OrdersToDelete.Add(orderWithUpdatedCustomFields);

            Assert.Equal(newValue, orderWithUpdatedCustomFields.Custom.Fields["string-field"]);
        }

        [Fact]
        public void UpdateOrderSetLineItemCustomType()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();

            // First Create order with Line Item
            var order = this.ordersFixture.CreateOrderFromCart();
            Assert.Single(order.LineItems);

            // Then set it's custom type
            string lineItemId = order.LineItems[0].Id;
            var customType = this.ordersFixture.CreateNewType();
            var fields = this.ordersFixture.CreateNewFields();

            var setCustomTypeUpdateAction =
                new SetLineItemCustomTypeUpdateAction
                {
                    LineItemId = lineItemId,
                    Type = new ResourceIdentifier<Type>
                    {
                        Key = customType.Key
                    },
                    Fields = fields
                };
            var updateActions = new List<UpdateAction<Order>> {setCustomTypeUpdateAction};

            var orderWithCustomTypeForLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id,
                    order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(orderWithCustomTypeForLineItem);

            Assert.Single(orderWithCustomTypeForLineItem.LineItems);
            Assert.Equal(customType.Id, orderWithCustomTypeForLineItem.LineItems[0].Custom.Type.Id);
        }

        [Fact]
        public void UpdateCartSetLineItemCustomField()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();

            // First Create Order with Line Item
            var order = this.ordersFixture.CreateOrderFromCart();
            Assert.Single(order.LineItems);

            // Then set custom type for the line item
            string lineItemId = order.LineItems[0].Id;
            var customType = this.ordersFixture.CreateNewType();
            var fields = this.ordersFixture.CreateNewFields();

            var setCustomTypeUpdateAction =
                new SetLineItemCustomTypeUpdateAction
                {
                    LineItemId = lineItemId,
                    Type = new ResourceIdentifier<Type>
                    {
                        Key = customType.Key
                    },
                    Fields = fields
                };
            var updateActions = new List<UpdateAction<Order>> {setCustomTypeUpdateAction};

            var orderWithCustomTypeForLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id,
                    order.Version, updateActions))
                .Result;

            Assert.Single(orderWithCustomTypeForLineItem.LineItems);
            Assert.Equal(customType.Id, orderWithCustomTypeForLineItem.LineItems[0].Custom.Type.Id);

            // then update it's fields

            string stringFieldValue = TestingUtility.RandomString(5);
            var setCustomFieldUpdateAction =
                new SetLineItemCustomFieldUpdateAction()
                {
                    Name = "string-field",
                    Value = stringFieldValue,
                    LineItemId = lineItemId
                };
            updateActions.Clear();
            updateActions.Add(setCustomFieldUpdateAction);

            var orderWithUpdatedCustomFieldForLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(orderWithCustomTypeForLineItem.Id,
                    orderWithCustomTypeForLineItem.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(orderWithUpdatedCustomFieldForLineItem);

            Assert.Single(orderWithUpdatedCustomFieldForLineItem.LineItems);
            Assert.Equal(stringFieldValue,
                orderWithUpdatedCustomFieldForLineItem.LineItems[0].Custom.Fields["string-field"]);
        }

        [Fact]
        public void UpdateOrderSetCustomLineItemCustomType()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();

            // First Create order with Custom LineItem
            var order = this.ordersFixture.CreateOrderFromCart(withCustomLineItem:true);
            Assert.Single(order.CustomLineItems);

            // Then set it's custom type
            string customLineItemId = order.CustomLineItems[0].Id;
            var customType = this.ordersFixture.CreateNewType();
            var fields = this.ordersFixture.CreateNewFields();

            var setCustomTypeUpdateAction =
                new SetCustomLineItemCustomTypeUpdateAction
                {
                    CustomLineItemId = customLineItemId,
                    Type = new ResourceIdentifier<Type>
                    {
                        Key = customType.Key
                    },
                    Fields = fields
                };
            var updateActions = new List<UpdateAction<Order>> {setCustomTypeUpdateAction};

            var orderWithCustomTypeForLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id,
                    order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(orderWithCustomTypeForLineItem);

            Assert.Single(orderWithCustomTypeForLineItem.CustomLineItems);
            Assert.Equal(customType.Id, orderWithCustomTypeForLineItem.CustomLineItems[0].Custom.Type.Id);
        }

        [Fact]
        public void UpdateCartSetCustomLineItemCustomField()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();

            // First Create Order with Custom Line Item
            var order = this.ordersFixture.CreateOrderFromCart(withCustomLineItem:true);
            Assert.Single(order.CustomLineItems);

            // Then set custom type for the Custom Line item
            string customLineItemId = order.CustomLineItems[0].Id;
            var customType = this.ordersFixture.CreateNewType();
            var fields = this.ordersFixture.CreateNewFields();

            var setCustomTypeUpdateAction =
                new SetCustomLineItemCustomTypeUpdateAction
                {
                    CustomLineItemId = customLineItemId,
                    Type = new ResourceIdentifier<Type>
                    {
                        Key = customType.Key
                    },
                    Fields = fields
                };
            var updateActions = new List<UpdateAction<Order>> {setCustomTypeUpdateAction};

            var orderWithCustomTypeForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id,
                    order.Version, updateActions))
                .Result;

            Assert.Single(orderWithCustomTypeForCustomLineItem.CustomLineItems);
            Assert.Equal(customType.Id, orderWithCustomTypeForCustomLineItem.CustomLineItems[0].Custom.Type.Id);

            // then update it's fields

            string stringFieldValue = TestingUtility.RandomString(5);
            var setCustomFieldUpdateAction =
                new SetCustomLineItemCustomFieldUpdateAction()
                {
                    Name = "string-field",
                    Value = stringFieldValue,
                    CustomLineItemId = customLineItemId
                };
            updateActions.Clear();
            updateActions.Add(setCustomFieldUpdateAction);

            var orderWithUpdatedCustomFieldForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(orderWithCustomTypeForCustomLineItem.Id,
                    orderWithCustomTypeForCustomLineItem.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(orderWithUpdatedCustomFieldForCustomLineItem);

            Assert.Single(orderWithUpdatedCustomFieldForCustomLineItem.CustomLineItems);
            Assert.Equal(stringFieldValue,
                orderWithUpdatedCustomFieldForCustomLineItem.CustomLineItems[0].Custom.Fields["string-field"]);
        }

        [Fact]
        public void UpdateOrderAddPayment()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            Assert.Null(order.PaymentInfo);

            var payment = this.ordersFixture.CreatePayment();
            var updateActions = new List<UpdateAction<Order>>();
            var addPaymentUpdateAction = new AddPaymentUpdateAction
            {
                Payment = new Reference<Payment>{ Id = payment.Id}
            };
            updateActions.Add(addPaymentUpdateAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.NotNull(retrievedOrder.PaymentInfo);
            Assert.Single(retrievedOrder.PaymentInfo.Payments);
            Assert.Equal(payment.Id,retrievedOrder.PaymentInfo.Payments[0].Id);
        }

        [Fact]
        public void UpdateOrderRemovePayment()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            Assert.Null(order.PaymentInfo);

            //Add Payment first
            var payment = this.ordersFixture.CreatePayment();
            var updateActions = new List<UpdateAction<Order>>();
            var addPaymentUpdateAction = new AddPaymentUpdateAction
            {
                Payment = new Reference<Payment>{ Id = payment.Id}
            };
            updateActions.Add(addPaymentUpdateAction);
            var orderWithPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            Assert.NotNull(orderWithPayment.PaymentInfo);
            Assert.Single(orderWithPayment.PaymentInfo.Payments);
            Assert.Equal(payment.Id,orderWithPayment.PaymentInfo.Payments[0].Id);

            //then remove it
            var removePaymentUpdateAction = new RemovePaymentUpdateAction
            {
                Payment = new Reference<Payment> {Id = payment.Id}
            };
            updateActions.Clear();
            updateActions.Add(removePaymentUpdateAction);

            var orderWithoutPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(orderWithPayment.Id, orderWithPayment.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(orderWithoutPayment);
            Assert.Null(orderWithoutPayment.PaymentInfo);
        }

        [Fact]
        public void UpdateOrderSetLocale()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            var projectLanguages = this.ordersFixture.GetProjectLanguages();

            Assert.True(projectLanguages.Count > 0); //make sure that project has at least one language
            Assert.Null(order.Locale);

            string locale = projectLanguages[0];

            SetLocaleUpdateAction setLocaleUpdateAction = new SetLocaleUpdateAction
            {
                Locale = locale
            };

            var updateActions = new List<UpdateAction<Order>> {setLocaleUpdateAction};

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Equal(locale, retrievedOrder.Locale);
        }

        [Fact]
        public void UpdateOrderSetDeliveryItems()
        {
            //First Add Delivery
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withDefaultShippingCountry: false,
                withShippingMethod: true);

            var lineItemId = order.LineItems[0].Id;
            var quantity = 1;

            Assert.NotNull(order.ShippingInfo);
            Assert.Empty(order.ShippingInfo.Deliveries);

            var retrievedOrder = this.ordersFixture.AddDelivery(order, quantity);

            Assert.Single(retrievedOrder.ShippingInfo.Deliveries);
            var delivery = retrievedOrder.ShippingInfo.Deliveries[0];

            Assert.Equal(quantity, delivery.Items[0].Quantity);

            // Then set Delivery Items
            var newQuantity = 2;
            var newDeliveryItems = new List<DeliveryItem>{ new DeliveryItem { Id = lineItemId, Quantity = newQuantity } };
            var setDeliveryItemsAction = new SetDeliveryItemsUpdateAction
            {
                Items = newDeliveryItems,
                DeliveryId = delivery.Id
            };
            var updateActions = new List<UpdateAction<Order>> { setDeliveryItemsAction };

            var updatedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(retrievedOrder.Id, retrievedOrder.Version, updateActions))
                .Result;


            this.ordersFixture.OrdersToDelete.Add(updatedOrder);

            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
            delivery = updatedOrder.ShippingInfo.Deliveries[0];

            Assert.Single(delivery.Items);
            Assert.Equal(lineItemId, delivery.Items[0].Id);
            Assert.Equal(newQuantity, delivery.Items[0].Quantity);
        }

        [Fact]
        public void UpdateOrderRemoveParcelFromDelivery()
        {
            //First Add Delivery with parcel
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withDefaultShippingCountry: false,
                withShippingMethod: true);

            Assert.NotNull(order.ShippingInfo);
            Assert.Empty(order.ShippingInfo.Deliveries);

            var quantity = 1;
            var retrievedOrder = this.ordersFixture.AddDelivery(order, quantity, withParcel: true);

            Assert.Single(retrievedOrder.ShippingInfo.Deliveries);
            var delivery = retrievedOrder.ShippingInfo.Deliveries[0];
            Assert.Single(delivery.Parcels);
            var parcel = delivery.Parcels[0];

            //then remove it
            var removeParcelAction = new RemoveParcelFromDeliveryUpdateAction
            {
                ParcelId = parcel.Id
            };
            var updateActions = new List<UpdateAction<Order>> { removeParcelAction };

            var orderWithOutParcels = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(retrievedOrder.Id, retrievedOrder.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(orderWithOutParcels);

            Assert.Single(orderWithOutParcels.ShippingInfo.Deliveries);
            Assert.Empty(orderWithOutParcels.ShippingInfo.Deliveries[0].Parcels);

        }

        [Fact]
        public void UpdateOrderRemoveDelivery()
        {
            //First Add Delivery
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withDefaultShippingCountry: false,
                withShippingMethod: true);

            Assert.NotNull(order.ShippingInfo);
            Assert.Empty(order.ShippingInfo.Deliveries);

            var quantity = 1;
            var retrievedOrder = this.ordersFixture.AddDelivery(order, quantity);

            Assert.Single(retrievedOrder.ShippingInfo.Deliveries);
            var delivery = retrievedOrder.ShippingInfo.Deliveries[0];
            //then remove it
            var removeDeliveryAction = new RemoveDeliveryUpdateAction
            {
                DeliveryId = delivery.Id
            };
            var updateActions = new List<UpdateAction<Order>> { removeDeliveryAction };

            var orderWithOutDelivery = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(retrievedOrder.Id, retrievedOrder.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(orderWithOutDelivery);

            Assert.Empty(orderWithOutDelivery.ShippingInfo.Deliveries);

        }

        [Fact]
        public void UpdateOrderSetParcelMeasurements()
        {
            //First Add Delivery with parcel
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withDefaultShippingCountry: false,
                withShippingMethod: true);

            Assert.NotNull(order.ShippingInfo);
            Assert.Empty(order.ShippingInfo.Deliveries);

            var quantity = 1;
            var retrievedOrder = this.ordersFixture.AddDelivery(order, quantity, withParcel: true);

            Assert.Single(retrievedOrder.ShippingInfo.Deliveries);
            var delivery = retrievedOrder.ShippingInfo.Deliveries[0];
            Assert.Single(delivery.Parcels);
            var parcel = delivery.Parcels[0];

            //then set measurements
            var newMeasurements = new ParcelMeasurements(10, 12, 13, 14);
            var setParcelMeasurementsAction = new SetParcelMeasurementsUpdateAction
            {
                ParcelId = parcel.Id,
                Measurements = newMeasurements
            };
            var updateActions = new List<UpdateAction<Order>> { setParcelMeasurementsAction };

            var updatedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(retrievedOrder.Id, retrievedOrder.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(updatedOrder);

            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
            Assert.Single(updatedOrder.ShippingInfo.Deliveries[0].Parcels);
            parcel = updatedOrder.ShippingInfo.Deliveries[0].Parcels[0];

            Assert.Equal(newMeasurements, parcel.Measurements);

        }

        [Fact]
        public void UpdateOrderSetParcelTrackingData()
        {
            //First Add Delivery with parcel
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withDefaultShippingCountry: false,
                withShippingMethod: true);

            Assert.NotNull(order.ShippingInfo);
            Assert.Empty(order.ShippingInfo.Deliveries);

            var quantity = 1;
            var retrievedOrder = this.ordersFixture.AddDelivery(order, quantity, withParcel: true);

            Assert.Single(retrievedOrder.ShippingInfo.Deliveries);
            var delivery = retrievedOrder.ShippingInfo.Deliveries[0];
            Assert.Single(delivery.Parcels);
            var parcel = delivery.Parcels[0];

            //then set measurements
            var ran = TestingUtility.RandomInt();
            var newTrackingData = new TrackingData($"tracking-{ran}", $"carrier-{ran}", $"provider-{ran}", $"prov-trans--{ran}",false);
            var setParcelTrackingDataAction = new SetParcelTrackingDataUpdateAction
            {
                ParcelId = parcel.Id,
                TrackingData = newTrackingData
            };
            var updateActions = new List<UpdateAction<Order>> { setParcelTrackingDataAction };

            var updatedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(retrievedOrder.Id, retrievedOrder.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(updatedOrder);

            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
            Assert.Single(updatedOrder.ShippingInfo.Deliveries[0].Parcels);
            parcel = updatedOrder.ShippingInfo.Deliveries[0].Parcels[0];

            Assert.Equal(newTrackingData.TrackingId, parcel.TrackingData.TrackingId);
            Assert.Equal(newTrackingData.Carrier, parcel.TrackingData.Carrier);
            Assert.Equal(newTrackingData.Provider, parcel.TrackingData.Provider);
            Assert.Equal(newTrackingData.ProviderTransaction, parcel.TrackingData.ProviderTransaction);
        }

        [Fact]
        public void UpdateOrderSetParcelItems()
        {
            //First Add Delivery
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withDefaultShippingCountry: false,
                withShippingMethod: true);

            var lineItemId = order.LineItems[0].Id;
            var quantity = 1;

            Assert.NotNull(order.ShippingInfo);
            Assert.Empty(order.ShippingInfo.Deliveries);

            var retrievedOrder = this.ordersFixture.AddDelivery(order, quantity, withParcel:true);

            Assert.Single(retrievedOrder.ShippingInfo.Deliveries);
            var delivery = retrievedOrder.ShippingInfo.Deliveries[0];

            Assert.Single(delivery.Parcels);
            var parcel = delivery.Parcels[0];

            // Then set Parcel Items
            var newQuantity = 2;
            var newDeliveryItems = new List<DeliveryItem>{ new DeliveryItem { Id = lineItemId, Quantity = newQuantity } };
            var setParcelItemsAction = new SetParcelItemsUpdateAction
            {
                Items = newDeliveryItems,
                ParcelId = parcel.Id
            };
            var updateActions = new List<UpdateAction<Order>> { setParcelItemsAction };

            var updatedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(retrievedOrder.Id, retrievedOrder.Version, updateActions))
                .Result;


            this.ordersFixture.OrdersToDelete.Add(updatedOrder);

            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
            delivery = updatedOrder.ShippingInfo.Deliveries[0];

            Assert.Single(delivery.Items);
            Assert.Single(delivery.Parcels);
            Assert.Single(delivery.Parcels[0].Items);
            Assert.Equal(lineItemId, delivery.Parcels[0].Items[0].Id);
            Assert.Equal(newQuantity, delivery.Parcels[0].Items[0].Quantity);
        }

        [Fact]
        public void UpdateOrderSetLineItemShippingDetails()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();

            // First Create order with Line Item
            var order = this.ordersFixture.CreateOrderFromCart(withItemShippingAddress:true);
            Assert.Single(order.LineItems);
            Assert.Single(order.ItemShippingAddresses);

            // Then set it's shippingDetails
            string lineItemId = order.LineItems[0].Id;
            string addressKey = order.ItemShippingAddresses[0].Key;
            long quantity = order.LineItems[0].Quantity;
            var itemShippingDetailsDraft =
                this.ordersFixture.GetItemShippingDetailsDraft(addressKey,quantity);

            var setLineItemShippingDetailsAction =
                new SetLineItemShippingDetailsUpdateAction
                {
                    LineItemId = lineItemId,
                    ShippingDetails = itemShippingDetailsDraft
                };
            var updateActions = new List<UpdateAction<Order>> {setLineItemShippingDetailsAction};

            var updatedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id,
                    order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(updatedOrder);

            Assert.Single(updatedOrder.LineItems);

            Assert.NotNull(updatedOrder.LineItems[0].ShippingDetails);
            Assert.Single(updatedOrder.LineItems[0].ShippingDetails.Targets);
            Assert.Equal(itemShippingDetailsDraft.Targets[0].Quantity,
                updatedOrder.LineItems[0].ShippingDetails.Targets[0].Quantity);
            Assert.Equal(itemShippingDetailsDraft.Targets[0].AddressKey,
                updatedOrder.LineItems[0].ShippingDetails.Targets[0].AddressKey);

        }

        [Fact]
        public void UpdateOrderSetCustomLineItemShippingDetails()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();

            // First Create order with Custom Line Item
            var order = this.ordersFixture.CreateOrderFromCart(withItemShippingAddress:true, withCustomLineItem: true);
            Assert.Single(order.CustomLineItems);
            Assert.Single(order.ItemShippingAddresses);

            // Then set it's shippingDetails
            string customLineItemId = order.CustomLineItems[0].Id;
            string addressKey = order.ItemShippingAddresses[0].Key;
            long quantity = order.CustomLineItems[0].Quantity;
            var itemShippingDetailsDraft =
                this.ordersFixture.GetItemShippingDetailsDraft(addressKey,quantity);

            var setCustomLineItemShippingDetailsAction =
                new SetCustomLineItemShippingDetailsUpdateAction
                {
                    CustomLineItemId = customLineItemId,
                    ShippingDetails = itemShippingDetailsDraft
                };
            var updateActions = new List<UpdateAction<Order>> {setCustomLineItemShippingDetailsAction};

            var updatedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id,
                    order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(updatedOrder);

            Assert.Single(updatedOrder.CustomLineItems);

            Assert.NotNull(updatedOrder.CustomLineItems[0].ShippingDetails);
            Assert.Single(updatedOrder.CustomLineItems[0].ShippingDetails.Targets);
            Assert.Equal(itemShippingDetailsDraft.Targets[0].Quantity,
                updatedOrder.CustomLineItems[0].ShippingDetails.Targets[0].Quantity);
            Assert.Equal(itemShippingDetailsDraft.Targets[0].AddressKey,
                updatedOrder.CustomLineItems[0].ShippingDetails.Targets[0].AddressKey);

        }

        [Fact]
        public void UpdateOrderAddItemShippingAddress()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart();

            Assert.Empty(order.ItemShippingAddresses);

            var country = TestingUtility.GetRandomEuropeCountry();
            var key = TestingUtility.RandomString(10);
            var address = new Address {Country = country, Key = key};
            var updateActions = new List<UpdateAction<Order>>();
            var addItemShippingAddressUpdateAction = new AddItemShippingAddressUpdateAction
            {
                Address = address
            };
            updateActions.Add(addItemShippingAddressUpdateAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);
            Assert.Single(retrievedOrder.ItemShippingAddresses);
            Assert.Equal(key, retrievedOrder.ItemShippingAddresses[0].Key);
            Assert.Equal(country, retrievedOrder.ItemShippingAddresses[0].Country);
        }

        [Fact]
        public void UpdateOrderRemoveItemShippingAddress()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withItemShippingAddress:true);

            Assert.Single(order.ItemShippingAddresses);

            var key = order.ItemShippingAddresses[0].Key;

            var updateActions = new List<UpdateAction<Order>>();
            var removeItemShippingAddressAction = new RemoveItemShippingAddressUpdateAction
            {
                AddressKey = key
            };
            updateActions.Add(removeItemShippingAddressAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Empty(retrievedOrder.ItemShippingAddresses);
        }

        [Fact]
        public void UpdateOrderUpdateItemShippingAddress()
        {
            IClient commerceToolsClient = this.ordersFixture.GetService<IClient>();
            var order = this.ordersFixture.CreateOrderFromCart(withItemShippingAddress:true);

            Assert.Single(order.ItemShippingAddresses);

            var address = order.ItemShippingAddresses[0];
            var state = $"{address.Country}_State_{TestingUtility.RandomInt()}";
            address.State = state;

            var updateActions = new List<UpdateAction<Order>>();
            var updateItemShippingAddressAction = new UpdateItemShippingAddressUpdateAction
            {
                Address = address
            };
            updateActions.Add(updateItemShippingAddressAction);
            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            this.ordersFixture.OrdersToDelete.Add(retrievedOrder);

            Assert.Single(retrievedOrder.ItemShippingAddresses);
            Assert.Equal(state, retrievedOrder.ItemShippingAddresses[0].State);
        }

        #endregion
    }
}
