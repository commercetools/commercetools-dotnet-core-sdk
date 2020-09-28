using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Orders.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.Stores;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Carts.CartsFixture;
using static commercetools.Sdk.IntegrationTests.Orders.OrdersFixture;
using static commercetools.Sdk.IntegrationTests.Customers.CustomersFixture;
using static commercetools.Sdk.IntegrationTests.Channels.ChannelsFixture;
using static commercetools.Sdk.IntegrationTests.States.StatesFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using static commercetools.Sdk.IntegrationTests.Payments.PaymentsFixture;
using static commercetools.Sdk.IntegrationTests.Projects.ProjectFixture;
using static commercetools.Sdk.IntegrationTests.Stores.StoresFixture;
using static commercetools.Sdk.IntegrationTests.CartDiscounts.CartDiscountsFixture;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.Orders
{
    [Collection("Integration Tests")]
    public class OrdersIntegrationTests
    {
        private readonly IClient client;

        public OrdersIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async void CreateOrderFromCart()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithOrder(client, draft => DefaultOrderFromCartDraft(draft, cart),
                        order =>
                        {
                            Assert.NotNull(order);
                            Assert.NotNull(order.Cart);
                            Assert.Equal(cart.Id, order.Cart.Id);
                        });
                });
        }

        [Fact]
        public async void CreateOrderFromCartInStore()
        {
            await WithStore(client, async store =>
            {
                await WithCartWithSingleLineItem(client, 2,
                    cartDraft => DefaultCartDraftInStore(cartDraft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        Assert.Equal(2, cart.LineItems[0].Quantity);
                        var buildDraft = DefaultOrderFromCartDraft(new OrderFromCartDraft(), cart);

                        var order = await client
                            .ExecuteAsync(new CreateCommand<Order>(buildDraft).InStore(store.Key));

                        Assert.NotNull(order);
                        Assert.NotNull(order.Cart);
                        Assert.NotNull(order.Store);
                        Assert.Equal(store.Key, order.Store.Key);
                        Assert.Equal(cart.Id, order.Cart.Id);

                        await DeleteResource(client, order);
                    });
            });
        }

        [Fact]
        public async void GetOrderById()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithOrder(client, draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            var retrievedOrder = await client
                                .ExecuteAsync(order.ToIdResourceIdentifier().GetById());
                            Assert.Equal(order.Id, retrievedOrder.Id);
                        });
                });
        }
        
        [Fact]
        public async Task GetOrderByIdExpandLineItemDiscount()
        {
            var key = $"CreateCartDiscount-{TestingUtility.RandomString()}";

            await WithCartDiscount(
                client, draft => DefaultCartDiscountDraftWithKey(draft, key),
                async cartDiscount =>
                {
                    Assert.NotNull(cartDiscount);
                    await WithCartWithSingleLineItem(client, 1, DefaultCartDraft,
                        async cart =>
                        {
                            Assert.NotNull(cart);
                            await WithOrder(client, draft => DefaultOrderFromCartDraft(draft, cart),
                                async order =>
                                {
                                    Assert.NotNull(order);
                                    
                                    var retrievedOrder = await client
                                        .ExecuteAsync(order
                                            .ToIdResourceIdentifier()
                                            .GetById()
                                            .Expand("lineItems[*].discountedPricePerQuantity[*].discountedPrice.includedDiscounts[*].discount")
                                        );

                                    Assert.Equal(order.Id, retrievedOrder.Id);
                                    var lineItem = retrievedOrder.LineItems.FirstOrDefault();
                                    Assert.NotNull(lineItem);
                                    Assert.NotNull(lineItem.DiscountedPricePerQuantity);
                            
                                    var discountedPricePerQuantity = lineItem.DiscountedPricePerQuantity.FirstOrDefault();
                                    Assert.NotNull(discountedPricePerQuantity);
                            
                                    var discountedPrice = discountedPricePerQuantity.DiscountedPrice;
                                    Assert.NotNull(discountedPrice);
                                    Assert.NotEmpty(discountedPrice.IncludedDiscounts);
                            
                                    var discountedLineItemPortion = discountedPrice.IncludedDiscounts.FirstOrDefault();
                                    Assert.NotNull(discountedLineItemPortion);
                                    Assert.NotNull(discountedLineItemPortion.Discount);
                            
                                    var discount = discountedLineItemPortion.Discount;
                                    Assert.NotNull(discount.Obj);
                                    Assert.Equal(cartDiscount.Key, discount.Obj.Key);
                                });
                        });
                });
        }

        [Fact]
        public async void GetOrderInStoreById()
        {
            await WithStore(client, async store =>
            {
                await WithCartWithSingleLineItem(client, 2,
                    cartDraft => DefaultCartDraftInStore(cartDraft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        Assert.Equal(2, cart.LineItems[0].Quantity);
                        await WithOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.NotNull(order.Store);
                                Assert.Equal(store.Key, order.Store.Key);
                                var retrievedOrder = await client
                                    .ExecuteAsync(order.ToIdResourceIdentifier().GetById().InStore(store.Key));
                                Assert.NotNull(retrievedOrder);
                                Assert.NotNull(retrievedOrder.Store);
                                Assert.Equal(store.Key, retrievedOrder.Store.Key);
                                Assert.Equal(order.Id, retrievedOrder.Id);
                            });
                    });
            });
        }

        [Fact]
        public async void GetOrderByOrderNumber()
        {
            var orderNumber = TestingUtility.RandomString();
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithOrder(client,
                        draft => DefaultOrderFromCartDraftWithNumber(
                            draft, cart, orderNumber),
                        async order =>
                        {
                            Assert.Equal(orderNumber, order.OrderNumber);
                            var retrievedOrder = await client
                                .ExecuteAsync(new GetOrderByOrderNumberCommand(order.OrderNumber));
                            Assert.Equal(order.Id, retrievedOrder.Id);
                            Assert.Equal(order.OrderNumber, retrievedOrder.OrderNumber);
                        });
                });
        }

        [Fact]
        public async void GetOrderInStoreByOrderNumber()
        {
            var orderNumber = TestingUtility.RandomString();
            await WithStore(client, async store =>
            {
                await WithCartWithSingleLineItem(client, 2,
                    cartDraft => DefaultCartDraftInStore(cartDraft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        Assert.Equal(2, cart.LineItems[0].Quantity);
                        await WithOrder(client,
                            draft => DefaultOrderFromCartDraftWithNumber(
                                draft, cart, orderNumber),
                            async order =>
                            {
                                Assert.NotNull(order.Store);
                                Assert.Equal(store.Key, order.Store.Key);
                                Assert.Equal(orderNumber, order.OrderNumber);

                                var retrievedOrder = await client
                                    .ExecuteAsync(new GetOrderByOrderNumberCommand(order.OrderNumber)
                                        .InStore(store.ToKeyResourceIdentifier()));

                                Assert.NotNull(retrievedOrder);
                                Assert.NotNull(retrievedOrder.Store);
                                Assert.Equal(store.Key, retrievedOrder.Store.Key);
                                Assert.Equal(order.Id, retrievedOrder.Id);
                                Assert.Equal(order.OrderNumber, retrievedOrder.OrderNumber);
                            });
                    });
            });
        }

        [Fact]
        public async void QueryOrders()
        {
            var orderNumber = TestingUtility.RandomString();
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithOrder(client,
                        draft => DefaultOrderFromCartDraftWithNumber(
                            draft, cart, orderNumber),
                        async order =>
                        {
                            Assert.Equal(orderNumber, order.OrderNumber);
                            var queryCommand = new QueryCommand<Order>();
                            queryCommand.Where(o => o.OrderNumber == order.OrderNumber.valueOf());
                            var returnedSet = await client.ExecuteAsync(queryCommand);
                            Assert.Single(returnedSet.Results);
                            Assert.Equal(order.OrderNumber, returnedSet.Results[0].OrderNumber);
                        });
                });
        }

        [Fact]
        public async void QueryOrdersInStore()
        {
            var orderNumber = TestingUtility.RandomString();
            await WithStore(client, async store =>
            {
                await WithCartWithSingleLineItem(client, 2,
                    cartDraft => DefaultCartDraftInStore(cartDraft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        await WithOrder(client,
                            draft => DefaultOrderFromCartDraftWithNumber(
                                draft, cart, orderNumber),
                            async order =>
                            {
                                Assert.NotNull(order);
                                Assert.NotNull(order.Store);

                                Assert.Equal(orderNumber, order.OrderNumber);
                                var queryCommand = new QueryCommand<Order>();
                                queryCommand.Where(o => o.OrderNumber == order.OrderNumber.valueOf())
                                    .InStore(store);
                                var returnedSet = await client.ExecuteAsync(queryCommand);
                                Assert.Single(returnedSet.Results);
                                var returnedOrder = returnedSet.Results[0];
                                Assert.NotNull(returnedOrder.Store);
                                Assert.Equal(store.Key, returnedOrder.Store.Key);
                                Assert.Equal(order.OrderNumber, returnedOrder.OrderNumber);
                            });
                    });
            });
        }

        [Fact]
        public async void QueryOrdersAndExpandCart()
        {
            var orderNumber = TestingUtility.RandomString();
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithOrder(client,
                        draft => DefaultOrderFromCartDraftWithNumber(
                            draft, cart, orderNumber),
                        async order =>
                        {
                            Assert.Equal(orderNumber, order.OrderNumber);
                            var queryCommand = new QueryCommand<Order>();
                            queryCommand.Where(o => o.OrderNumber == order.OrderNumber.valueOf());
                            queryCommand.Expand(o => o.Cart);
                            var returnedSet = await client.ExecuteAsync(queryCommand);
                            Assert.Single(returnedSet.Results);
                            var retrievedOrder = returnedSet.Results[0];
                            Assert.Equal(order.OrderNumber, retrievedOrder.OrderNumber);
                            Assert.NotNull(retrievedOrder.Cart.Obj);
                        });
                });
        }

        [Fact]
        public async void QueryAndSortOrders()
        {
            var email = $"joe{TestingUtility.RandomString()}@example.com";
            var ordersCount = 3;

            //create 3 carts, each one has single lineItem
            await WithCustomer(client,
                customerDraft => DefaultCustomerDraftWithEmail(customerDraft, email),
                async customer =>
                {
                    await WithListOfCartsWithSingleLineItem(client, ordersCount, 2,
                        cartDraft => DefaultCartDraftWithCustomer(cartDraft, customer),
                        async carts =>
                        {
                            Assert.Equal(ordersCount, carts.Count);
                            Assert.Single(carts[0].LineItems);
                            Assert.Equal(customer.Id, carts[0].CustomerId);

                            var draftsActions = new List<Func<OrderFromCartDraft, OrderFromCartDraft>>();

                            carts.ForEach(createdCart =>
                                draftsActions.Add(draft =>
                                    DefaultOrderFromCartDraft(draft, createdCart)));

                            await WithListOfOrders(client, draftsActions,
                                async orders =>
                                {
                                    Assert.Equal(ordersCount, orders.Count);
                                    //expected list of order numbers sorted
                                    var expectedOrderNumbersList = orders.OrderBy(o => o.OrderNumber)
                                        .Select(o => o.OrderNumber).ToList();

                                    var queryCommand = new QueryCommand<Order>();
                                    queryCommand.Where(order => order.CustomerEmail == email);
                                    queryCommand.Sort(o => o.OrderNumber);
                                    var returnedSet = await client.ExecuteAsync(queryCommand);
                                    Assert.Equal(ordersCount, returnedSet.Count);
                                    var actualOrderNumbersList =
                                        returnedSet.Results.Select(o => o.OrderNumber).ToList();

                                    Assert.True(expectedOrderNumbersList.SequenceEqual(actualOrderNumbersList));
                                });
                        });
                });
        }

        [Fact]
        public async void QueryAndLimitOrders()
        {
            var email = $"joe{TestingUtility.RandomString()}@example.com";
            var ordersCount = 3;

            //create 3 carts, each one has single lineItem
            await WithCustomer(client,
                customerDraft => DefaultCustomerDraftWithEmail(customerDraft, email),
                async customer =>
                {
                    await WithListOfCartsWithSingleLineItem(client, ordersCount, 2,
                        cartDraft => DefaultCartDraftWithCustomer(cartDraft, customer),
                        async carts =>
                        {
                            Assert.Equal(ordersCount, carts.Count);
                            Assert.Single(carts[0].LineItems);
                            Assert.Equal(customer.Id, carts[0].CustomerId);

                            var draftsActions = new List<Func<OrderFromCartDraft, OrderFromCartDraft>>();

                            carts.ForEach(createdCart =>
                                draftsActions.Add(draft =>
                                    DefaultOrderFromCartDraft(draft, createdCart)));

                            await WithListOfOrders(client, draftsActions,
                                async orders =>
                                {
                                    Assert.Equal(ordersCount, orders.Count);

                                    var queryCommand = new QueryCommand<Order>();
                                    queryCommand.SetLimit(2);
                                    queryCommand.SetWithTotal(true);
                                    queryCommand.Where(order => order.CustomerEmail == email);
                                    var returnedSet = await client.ExecuteAsync(queryCommand);

                                    Assert.Equal(2, returnedSet.Results.Count);
                                    Assert.Equal(3, returnedSet.Total);
                                });
                        });
                });
        }

        [Fact]
        public async void DeleteOrderById()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithOrder(client, draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            await client.ExecuteAsync(order.DeleteById());
                            await Assert.ThrowsAsync<NotFoundException>(
                                () => client.ExecuteAsync(new GetByIdCommand<Order>(order)));
                        });
                });
        }

        [Fact]
        public async void DeleteOrderInStoreById()
        {
            await WithStore(client, async store =>
            {
                await WithCartWithSingleLineItem(client, 2,
                    cartDraft => DefaultCartDraftInStore(cartDraft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        Assert.Equal(2, cart.LineItems[0].Quantity);
                        await WithOrder(client, draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.NotNull(order);
                                Assert.NotNull(order.Store);
                                Assert.Equal(store.Key, order.Store.Key);
                                await client.ExecuteAsync(order.DeleteById().InStore(store));
                                await Assert.ThrowsAsync<NotFoundException>(
                                    () => client.ExecuteAsync(
                                        new GetByIdCommand<Order>(order).InStore(store)));
                            });
                    });
            });
        }

        [Fact]
        public async void DeleteOrderByOrderNumber()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithOrder(client, draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            await client.ExecuteAsync(new DeleteByOrderNumberCommand(order.OrderNumber, order.Version));
                            await Assert.ThrowsAsync<NotFoundException>(
                                () => client.ExecuteAsync(new GetByIdCommand<Order>(order)));
                        });
                });
        }

        [Fact]
        public async void DeleteOrderInStoreByOrderNumber()
        {
            await WithStore(client, async store =>
            {
                await WithCartWithSingleLineItem(client, 2,
                    cartDraft => DefaultCartDraftInStore(cartDraft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        Assert.Equal(2, cart.LineItems[0].Quantity);
                        await WithOrder(client, draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.NotNull(order);
                                Assert.NotNull(order.Store);
                                Assert.Equal(store.Key, order.Store.Key);

                                await client.ExecuteAsync(
                                    new DeleteByOrderNumberCommand(order.OrderNumber, order.Version)
                                        .InStore(store));
                                await Assert.ThrowsAsync<NotFoundException>(
                                    () => client.ExecuteAsync(
                                        new GetByIdCommand<Order>(order).InStore(store)));
                            });
                    });
            });
        }

        #region UpdateActions

        [Fact]
        public async void UpdateOrderByOrderNumberChangeOrderState()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraftWithOrderState(
                            draft, cart, OrderState.Open),
                        async order =>
                        {
                            Assert.Equal(OrderState.Open, order.OrderState);

                            var newOrderState = OrderState.Complete;
                            var updateActions = new List<UpdateAction<Order>>();
                            var action = new ChangeOrderStateUpdateAction
                            {
                                OrderState = newOrderState
                            };
                            updateActions.Add(action);
                            var updatedOrder = await client
                                .ExecuteAsync(new UpdateByOrderNumberCommand(order.OrderNumber, order.Version,
                                    updateActions));
                            Assert.Equal(newOrderState, updatedOrder.OrderState);
                            return updatedOrder;
                        });
                });
        }
        
        [Fact]
        public async void UpdateOrderInStoreByOrderNumberChangeOrderState()
        {
            await WithStore(client, async store =>
            {
                await WithCartWithSingleLineItem(client, 2,
                    cartDraft => DefaultCartDraftInStore(cartDraft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        Assert.Equal(2, cart.LineItems[0].Quantity);
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraftWithOrderState(
                                draft, cart, OrderState.Open),
                            async order =>
                            {
                                Assert.NotNull(order);
                                Assert.NotNull(order.Store);
                                Assert.Equal(store.Key, order.Store.Key);
                                Assert.Equal(OrderState.Open, order.OrderState);

                                var newOrderState = OrderState.Complete;
                                var updateActions = new List<UpdateAction<Order>>();
                                var action = new ChangeOrderStateUpdateAction
                                {
                                    OrderState = newOrderState
                                };
                                updateActions.Add(action);
                                var updatedOrder = await client
                                    .ExecuteAsync(new UpdateByOrderNumberCommand(order.OrderNumber, order.Version,
                                        updateActions).InStore(store));

                                Assert.NotNull(updatedOrder);
                                Assert.NotNull(updatedOrder.Store);
                                Assert.Equal(store.Key, updatedOrder.Store.Key);
                                Assert.Equal(newOrderState, updatedOrder.OrderState);
                                return updatedOrder;
                            });
                    });
            });

        }

        [Fact]
        public async void UpdateOrderByOrderNumberChangeShipmentState()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraftWithShipmentState(
                            draft, cart, ShipmentState.Pending),
                        async order =>
                        {
                            Assert.Equal(ShipmentState.Pending, order.ShipmentState);

                            var newShipmentState = ShipmentState.Shipped;
                            var updateActions = new List<UpdateAction<Order>>();
                            var action = new ChangeShipmentStateUpdateAction
                            {
                                ShipmentState = newShipmentState
                            };
                            updateActions.Add(action);
                            var updatedOrder = await client
                                .ExecuteAsync(new UpdateByOrderNumberCommand(order.OrderNumber, order.Version,
                                    updateActions));
                            Assert.Equal(newShipmentState, updatedOrder.ShipmentState);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderByIdChangePaymentState()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraftWithPaymentState(
                            draft, cart, PaymentState.Pending),
                        async order =>
                        {
                            Assert.Equal(PaymentState.Pending, order.PaymentState);

                            var newPaymentState = PaymentState.Paid;
                            var action = new ChangePaymentStateUpdateAction
                            {
                                PaymentState = newPaymentState
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));
                            Assert.Equal(newPaymentState, updatedOrder.PaymentState);
                            return updatedOrder;
                        });
                });
        }
        
        [Fact]
        public async void UpdateOrderInStoreByIdChangePaymentState()
        {
            await WithStore(client, async store =>
            {
                await WithCartWithSingleLineItem(client, 2, 
                    cartDraft => DefaultCartDraftInStore(cartDraft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        Assert.Equal(2, cart.LineItems[0].Quantity);
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraftWithPaymentState(
                                draft, cart, PaymentState.Pending),
                            async order =>
                            {
                                Assert.NotNull(order);
                                Assert.NotNull(order.Store);
                                Assert.Equal(store.Key, order.Store.Key);
                                Assert.Equal(PaymentState.Pending, order.PaymentState);

                                var newPaymentState = PaymentState.Paid;
                                var action = new ChangePaymentStateUpdateAction
                                {
                                    PaymentState = newPaymentState
                                };
                                var updatedOrder = await client
                                    .ExecuteAsync(order.
                                        UpdateById(actions => actions.AddUpdate(action))
                                        .InStore(store));
                                
                                Assert.NotNull(updatedOrder.Store);
                                Assert.NotNull(updatedOrder.Store);
                                Assert.Equal(store.Key, updatedOrder.Store.Key);
                                Assert.Equal(newPaymentState, updatedOrder.PaymentState);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderUpdateSyncInfo()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    var roles = new List<ChannelRole>
                    {
                        ChannelRole.OrderImport
                    };

                    await WithChannel(client,
                        channelDraft =>
                            DefaultChannelDraftWithRoles(channelDraft, roles),
                        async channel =>
                        {
                            Assert.Single(channel.Roles);
                            Assert.Equal(ChannelRole.OrderImport, channel.Roles[0]);
                            await WithUpdateableOrder(client,
                                draft => DefaultOrderFromCartDraft(draft, cart),
                                async order =>
                                {
                                    Assert.Empty(order.SyncInfo);
                                    var externalId = TestingUtility.RandomString();
                                    var action = new UpdateSyncInfoUpdateAction
                                    {
                                        Channel = channel.ToKeyResourceIdentifier(),
                                        ExternalId = externalId
                                    };
                                    var updatedOrder = await client
                                        .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                                    Assert.Single(updatedOrder.SyncInfo);
                                    var syncInfo = updatedOrder.SyncInfo[0];
                                    Assert.Equal(channel.Id, syncInfo.Channel.Id);
                                    Assert.Equal(externalId, syncInfo.ExternalId);
                                    return updatedOrder;
                                });
                        });
                });
        }

        [Fact]
        public async void UpdateOrderByIdAddReturnInfo()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Empty(order.ReturnInfo);

                            var lineItemId = order.LineItems[0].Id;
                            var lineItemReturnItemDraft = TestingUtility.GetLineItemReturnItemDraft(lineItemId);
                            var returnTrackingId = TestingUtility.RandomString();

                            var action = new AddReturnInfoUpdateAction
                            {
                                Items = new List<ReturnItemDraft> {lineItemReturnItemDraft},
                                ReturnTrackingId = returnTrackingId
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.ReturnInfo);
                            var returnInfo = updatedOrder.ReturnInfo[0];

                            Assert.Single(returnInfo.Items);
                            Assert.Equal(returnTrackingId, returnInfo.ReturnTrackingId);
                            Assert.Equal(lineItemReturnItemDraft.ShipmentState, returnInfo.Items[0].ShipmentState);
                            Assert.IsType<LineItemReturnItem>(returnInfo.Items[0]);

                            var item = returnInfo.Items[0] as LineItemReturnItem;
                            Assert.NotNull(item);
                            Assert.Equal(lineItemId, item.LineItemId);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetReturnShipmentState()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Empty(order.ReturnInfo);

                            var lineItemId = order.LineItems[0].Id;
                            var lineItemReturnItemDraft = TestingUtility.GetLineItemReturnItemDraft(lineItemId);
                            var returnTrackingId = TestingUtility.RandomString();

                            var action = new AddReturnInfoUpdateAction
                            {
                                Items = new List<ReturnItemDraft> {lineItemReturnItemDraft},
                                ReturnTrackingId = returnTrackingId
                            };
                            var orderWithReturnInfo = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Single(orderWithReturnInfo.ReturnInfo);
                            Assert.Single(orderWithReturnInfo.ReturnInfo[0].Items);

                            var returnItemId = orderWithReturnInfo.ReturnInfo[0].Items[0].Id;
                            var returnShipmentState = ReturnShipmentState.BackInStock;

                            Assert.NotEqual(returnShipmentState,
                                orderWithReturnInfo.ReturnInfo[0].Items[0].ShipmentState);

                            var setReturnShipmentStateAction = new SetReturnShipmentStateUpdateAction
                            {
                                ReturnItemId = returnItemId,
                                ShipmentState = returnShipmentState
                            };

                            var updatedOrder = await client
                                .ExecuteAsync(orderWithReturnInfo.UpdateById(
                                    actions => actions.AddUpdate(setReturnShipmentStateAction)));

                            Assert.Single(updatedOrder.ReturnInfo);
                            Assert.Single(updatedOrder.ReturnInfo[0].Items);
                            Assert.Equal(returnShipmentState, updatedOrder.ReturnInfo[0].Items[0].ShipmentState);

                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetReturnPaymentState()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Empty(order.ReturnInfo);

                            var lineItemId = order.LineItems[0].Id;
                            var lineItemReturnItemDraft = TestingUtility.GetLineItemReturnItemDraft(lineItemId);
                            var returnTrackingId = TestingUtility.RandomString();

                            var action = new AddReturnInfoUpdateAction
                            {
                                Items = new List<ReturnItemDraft> {lineItemReturnItemDraft},
                                ReturnTrackingId = returnTrackingId
                            };
                            var orderWithReturnInfo = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Single(orderWithReturnInfo.ReturnInfo);
                            Assert.Single(orderWithReturnInfo.ReturnInfo[0].Items);

                            var returnItemId = orderWithReturnInfo.ReturnInfo[0].Items[0].Id;
                            var returnPaymentState = ReturnPaymentState.NotRefunded;

                            Assert.NotEqual(returnPaymentState,
                                orderWithReturnInfo.ReturnInfo[0].Items[0].PaymentState);

                            var setReturnPaymentStateAction = new SetReturnPaymentStateUpdateAction
                            {
                                ReturnItemId = returnItemId,
                                PaymentState = returnPaymentState
                            };


                            var updatedOrder = await client
                                .ExecuteAsync(orderWithReturnInfo.UpdateById(
                                    actions => actions.AddUpdate(setReturnPaymentStateAction)));

                            Assert.Single(updatedOrder.ReturnInfo);
                            Assert.Single(updatedOrder.ReturnInfo[0].Items);
                            Assert.Equal(returnPaymentState, updatedOrder.ReturnInfo[0].Items[0].PaymentState);

                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderChangeStateOfLineItem()
        {
            var quantity = 2;
            var states = GetStandardStates(client);
            Assert.Equal(2, states.Count);
            var initialState = states.FirstOrDefault(state => state.Initial);
            var nextState = states.FirstOrDefault(state => !state.Initial);
            Assert.NotNull(initialState);
            Assert.NotNull(nextState);

            await WithCartWithSingleLineItem(client, quantity, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(quantity, cart.LineItems[0].Quantity);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.LineItems);
                            var lineItem = order.LineItems[0];

                            var action = new TransitionLineItemStateUpdateAction
                            {
                                Quantity = quantity,
                                LineItemId = lineItem.Id,
                                FromState = new Reference<State> {Id = initialState.Id},
                                ToState = new Reference<State> {Id = nextState.Id},
                                ActualTransitionDate = DateTime.Now.AddSeconds(-500)
                            };

                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.LineItems);
                            var updatedLineItem = updatedOrder.LineItems[0];
                            var lineItemNextState =
                                updatedLineItem.State.FirstOrDefault(s => s.State.Id == nextState.Id);
                            Assert.NotNull(lineItemNextState);
                            Assert.Equal(quantity, lineItemNextState.Quantity);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderChangeStateOfCustomLineItem()
        {
            var quantity = 2;
            var states = GetStandardStates(client);
            Assert.Equal(2, states.Count);
            var initialState = states.FirstOrDefault(state => state.Initial);
            var nextState = states.FirstOrDefault(state => !state.Initial);
            Assert.NotNull(initialState);
            Assert.NotNull(nextState);

            await WithCartWithSingleCustomLineItem(client, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.CustomLineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.CustomLineItems);
                            var customLineItem = order.CustomLineItems[0];

                            var action = new TransitionCustomLineItemStateUpdateAction
                            {
                                Quantity = quantity,
                                CustomLineItemId = customLineItem.Id,
                                FromState = new Reference<State> {Id = initialState.Id},
                                ToState = new Reference<State> {Id = nextState.Id},
                                ActualTransitionDate = DateTime.Now.AddSeconds(-500)
                            };

                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.CustomLineItems);
                            var updatedCustomLineItem = updatedOrder.CustomLineItems[0];
                            var customLineItemNextState =
                                updatedCustomLineItem.State.FirstOrDefault(s => s.State.Id == nextState.Id);
                            Assert.NotNull(customLineItemNextState);
                            Assert.Equal(quantity, customLineItemNextState.Quantity);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderImportStateForLineItems()
        {
            var states = GetStandardStates(client);
            var initialState = states.FirstOrDefault(state => state.Initial);
            var nextState = states.FirstOrDefault(state => !state.Initial);
            Assert.NotNull(initialState);
            Assert.NotNull(nextState);
            var quantity = 6;

            await WithCartWithSingleLineItem(client, quantity, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(quantity, cart.LineItems[0].Quantity);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.LineItems);
                            var lineItem = order.LineItems[0];

                            var itemStates = new List<ItemState>
                            {
                                new ItemState {State = new Reference<State> {Id = nextState.Id}, Quantity = 1},
                                new ItemState
                                {
                                    State = new Reference<State> {Id = initialState.Id},
                                    Quantity = lineItem.Quantity - 1
                                }
                            };
                            var action = new ImportLineItemStateUpdateAction
                            {
                                LineItemId = lineItem.Id,
                                State = itemStates
                            };

                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));
                            Assert.Single(updatedOrder.LineItems);
                            Assert.Equal(2, updatedOrder.LineItems[0].State.Count);
                            Assert.Equal(nextState.Id, updatedOrder.LineItems[0].State[0].State.Id);
                            Assert.Equal(1, updatedOrder.LineItems[0].State[0].Quantity);
                            Assert.Equal(initialState.Id, updatedOrder.LineItems[0].State[1].State.Id);
                            Assert.Equal(lineItem.Quantity - 1, updatedOrder.LineItems[0].State[1].Quantity);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderImportStateForCustomLineItems()
        {
            var states = GetStandardStates(client);
            var initialState = states.FirstOrDefault(state => state.Initial);
            var nextState = states.FirstOrDefault(state => !state.Initial);
            Assert.NotNull(initialState);
            Assert.NotNull(nextState);

            await WithCartWithSingleCustomLineItem(client, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.CustomLineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.CustomLineItems);
                            var customLineItem = order.CustomLineItems[0];

                            var itemStates = new List<ItemState>
                            {
                                new ItemState {State = new Reference<State> {Id = nextState.Id}, Quantity = 1},
                                new ItemState
                                {
                                    State = new Reference<State> {Id = initialState.Id},
                                    Quantity = customLineItem.Quantity - 1
                                }
                            };
                            var action = new ImportCustomLineItemStateUpdateAction
                            {
                                CustomLineItemId = customLineItem.Id,
                                State = itemStates
                            };

                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.CustomLineItems);
                            Assert.Equal(2, updatedOrder.CustomLineItems[0].State.Count);
                            Assert.Equal(nextState.Id, updatedOrder.CustomLineItems[0].State[0].State.Id);
                            Assert.Equal(1, updatedOrder.CustomLineItems[0].State[0].Quantity);
                            Assert.Equal(initialState.Id, updatedOrder.CustomLineItems[0].State[1].State.Id);
                            Assert.Equal(customLineItem.Quantity - 1,
                                updatedOrder.CustomLineItems[0].State[1].Quantity);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderAddDelivery()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.LineItems);
                            Assert.NotNull(order.ShippingInfo);
                            Assert.Empty(order.ShippingInfo.Deliveries);

                            var lineItem = order.LineItems[0];
                            var quantity = lineItem.Quantity;

                            var deliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = quantity
                                }
                            };
                            var smallParcelMeasurements = new ParcelMeasurements(1, 2, 3, 4);
                            var trackingData = new TrackingData("tracking-id-12", "carrier xyz", "provider foo",
                                "prov trans 56", false);
                            var parcels = new List<ParcelDraft>
                            {
                                new ParcelDraft
                                {
                                    Measurements = smallParcelMeasurements,
                                    TrackingData = trackingData,
                                    Items = deliveryItems
                                }
                            };
                            var action = new AddDeliveryUpdateAction
                            {
                                Items = deliveryItems,
                                Parcels = parcels
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
                            var delivery = updatedOrder.ShippingInfo.Deliveries[0];

                            Assert.Single(delivery.Items);
                            Assert.Single(delivery.Parcels);
                            Assert.Single(delivery.Parcels[0].Items);
                            Assert.Equal(lineItem.Id, delivery.Items[0].Id);
                            Assert.Equal(quantity, delivery.Items[0].Quantity);
                            Assert.Equal(lineItem.Id, delivery.Parcels[0].Items[0].Id);
                            Assert.Equal(quantity, delivery.Parcels[0].Items[0].Quantity);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetDeliveryAddress()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            //First Add Delivery
                            Assert.Single(order.LineItems);
                            Assert.NotNull(order.ShippingInfo);
                            Assert.Empty(order.ShippingInfo.Deliveries);

                            var lineItem = order.LineItems[0];
                            var quantity = lineItem.Quantity;

                            var deliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = quantity
                                }
                            };
                            var smallParcelMeasurements = new ParcelMeasurements(1, 2, 3, 4);
                            var trackingData = new TrackingData("tracking-id-12", "carrier xyz", "provider foo",
                                "prov trans 56", false);
                            var parcels = new List<ParcelDraft>
                            {
                                new ParcelDraft
                                {
                                    Measurements = smallParcelMeasurements,
                                    TrackingData = trackingData,
                                    Items = deliveryItems
                                }
                            };
                            var addDeliveryAction = new AddDeliveryUpdateAction
                            {
                                Items = deliveryItems,
                                Parcels = parcels
                            };
                            var orderWithDelivery = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(addDeliveryAction)));

                            Assert.Single(orderWithDelivery.ShippingInfo.Deliveries);
                            var delivery = orderWithDelivery.ShippingInfo.Deliveries[0];
                            Assert.Null(delivery.Address);

                            // Then set Delivery Address
                            var country = TestingUtility.GetRandomEuropeCountry();
                            var address = new Address {Country = country};
                            var action = new SetDeliveryAddressUpdateAction
                            {
                                Address = address,
                                DeliveryId = delivery.Id
                            };

                            var updatedOrder = await client
                                .ExecuteAsync(orderWithDelivery.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
                            delivery = updatedOrder.ShippingInfo.Deliveries[0];

                            Assert.NotNull(delivery.Address);
                            Assert.Equal(country, delivery.Address.Country);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderAddParcel()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            //First Add Delivery
                            Assert.Single(order.LineItems);
                            Assert.NotNull(order.ShippingInfo);
                            Assert.Empty(order.ShippingInfo.Deliveries);

                            var lineItem = order.LineItems[0];
                            var quantity = lineItem.Quantity;

                            var deliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = quantity
                                }
                            };

                            var addDeliveryAction = new AddDeliveryUpdateAction
                            {
                                Items = deliveryItems
                            };
                            var orderWithDelivery = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(addDeliveryAction)));

                            Assert.Single(orderWithDelivery.ShippingInfo.Deliveries);
                            var delivery = orderWithDelivery.ShippingInfo.Deliveries[0];
                            Assert.Empty(delivery.Parcels);

                            // Then Add Parcel
                            var smallParcelMeasurements = new ParcelMeasurements(1, 2, 3, 4);
                            var trackingData = new TrackingData("tracking-id-12", "carrier xyz", "provider foo",
                                "prov trans 56", false);
                            var action = new AddParcelUpdateAction
                            {
                                Measurements = smallParcelMeasurements,
                                TrackingData = trackingData,
                                DeliveryId = delivery.Id,
                                Items = deliveryItems
                            };

                            var updatedOrder = await client
                                .ExecuteAsync(orderWithDelivery.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
                            delivery = updatedOrder.ShippingInfo.Deliveries[0];
                            Assert.Single(delivery.Parcels);
                            var parcel = delivery.Parcels[0];
                            Assert.Single(parcel.Items);
                            Assert.Equal(lineItem.Id, parcel.Items[0].Id);
                            Assert.Equal(quantity, parcel.Items[0].Quantity);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetOrderNumber()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(2, cart.LineItems[0].Quantity);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraftWithNumber(draft, cart, orderNumber: null),
                        async order =>
                        {
                            Assert.Null(order.OrderNumber);
                            var newOrderNumber = TestingUtility.RandomString();
                            var action = new SetOrderNumberUpdateAction
                            {
                                OrderNumber = newOrderNumber
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Equal(newOrderNumber, updatedOrder.OrderNumber);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderTransitionToNewState()
        {
            await WithState(client,
                stateDraft => DefaultStateDraftWithType(stateDraft, StateType.OrderState),
                async state =>
                {
                    await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                        async cart =>
                        {
                            Assert.Single(cart.LineItems);
                            await WithUpdateableOrder(client,
                                draft => DefaultOrderFromCartDraft(draft, cart),
                                async order =>
                                {
                                    Assert.Null(order.State);
                                    var action = new TransitionStateUpdateAction()
                                    {
                                        State = state.ToKeyResourceIdentifier()
                                    };
                                    var updatedOrder = await client
                                        .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                                    Assert.NotNull(updatedOrder.State);
                                    Assert.Equal(state.Id, updatedOrder.State.Id);
                                    return updatedOrder;
                                });
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetCustomerEmail()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Null(order.CustomerEmail);
                            var newEmail = $"joe{TestingUtility.RandomInt()}@example.com";
                            var action = new SetCustomerEmailUpdateAction
                            {
                                Email = newEmail
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Equal(newEmail, updatedOrder.CustomerEmail);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetCustomerId()
        {
            await WithCustomer(client, async customer =>
            {
                await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.Null(order.CustomerId);
                                var action = new SetCustomerIdUpdateAction
                                {
                                    CustomerId = customer.Id
                                };
                                var updatedOrder = await client
                                    .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                                Assert.Equal(customer.Id, updatedOrder.CustomerId);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderSetShippingAddress()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            var country = TestingUtility.GetRandomEuropeCountry();
                            var state = $"{country}_State_{TestingUtility.RandomInt()}";
                            var newShippingAddress = new Address
                            {
                                Country = country, State = state,
                                Key = TestingUtility.RandomString()
                            };
                            var action = new SetShippingAddressUpdateAction
                            {
                                Address = newShippingAddress
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Equal(newShippingAddress.Key, updatedOrder.ShippingAddress.Key);
                            Assert.Equal(newShippingAddress.Country, updatedOrder.ShippingAddress.Country);
                            Assert.Equal(newShippingAddress.State, updatedOrder.ShippingAddress.State);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetBillingAddress()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            var country = TestingUtility.GetRandomEuropeCountry();
                            var state = $"{country}_State_{TestingUtility.RandomInt()}";
                            var newBillingAddress = new Address
                            {
                                Country = country, State = state,
                                Key = TestingUtility.RandomString()
                            };
                            var action = new SetBillingAddressUpdateAction
                            {
                                Address = newBillingAddress
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Equal(newBillingAddress.Key, updatedOrder.BillingAddress.Key);
                            Assert.Equal(newBillingAddress.Country, updatedOrder.BillingAddress.Country);
                            Assert.Equal(newBillingAddress.State, updatedOrder.BillingAddress.State);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderByIdSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                var action = new SetCustomTypeUpdateAction
                                {
                                    Type = type.ToKeyResourceIdentifier(),
                                    Fields = fields
                                };
                                var updatedOrder = await client
                                    .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                                Assert.Equal(type.Id, updatedOrder.Custom.Type.Id);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderByIdSetCustomField()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                var setCustomTypeAction = new SetCustomTypeUpdateAction
                                {
                                    Type = type.ToKeyResourceIdentifier(),
                                    Fields = fields
                                };
                                var orderWithCustomType = await client
                                    .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(setCustomTypeAction)));

                                Assert.Equal(type.Id, orderWithCustomType.Custom.Type.Id);

                                //set the custom field with new value
                                var newValue = TestingUtility.RandomString(10);
                                var action = new SetCustomFieldUpdateAction()
                                {
                                    Name = "string-field", Value = newValue
                                };
                                var updatedOrder = await client
                                    .ExecuteAsync(orderWithCustomType.UpdateById(actions => actions.AddUpdate(action)));

                                Assert.Equal(newValue, updatedOrder.Custom.Fields["string-field"]);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderSetLineItemCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                    async cart =>
                    {
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.Single(order.LineItems);
                                var lineItem = order.LineItems[0];

                                var action = new SetLineItemCustomTypeUpdateAction
                                {
                                    LineItemId = lineItem.Id,
                                    Type = type.ToKeyResourceIdentifier(),
                                    Fields = fields
                                };
                                var updatedOrder = await client
                                    .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                                Assert.Single(updatedOrder.LineItems);
                                Assert.Equal(type.Id, updatedOrder.LineItems[0].Custom.Type.Id);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderSetLineItemCustomField()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                    async cart =>
                    {
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.Single(order.LineItems);
                                var lineItem = order.LineItems[0];

                                var setLineItemCustomTypeUpdateAction = new SetLineItemCustomTypeUpdateAction
                                {
                                    LineItemId = lineItem.Id,
                                    Type = type.ToKeyResourceIdentifier(),
                                    Fields = fields
                                };
                                var orderWithLineItemWithCustomType = await client
                                    .ExecuteAsync(order.UpdateById(actions =>
                                        actions.AddUpdate(setLineItemCustomTypeUpdateAction)));

                                Assert.Single(orderWithLineItemWithCustomType.LineItems);
                                Assert.Equal(type.Id, orderWithLineItemWithCustomType.LineItems[0].Custom.Type.Id);

                                // then update it's fields

                                var stringFieldValue = TestingUtility.RandomString();
                                var action =
                                    new SetLineItemCustomFieldUpdateAction()
                                    {
                                        Name = "string-field",
                                        Value = stringFieldValue,
                                        LineItemId = lineItem.Id
                                    };
                                var updatedOrder = await client
                                    .ExecuteAsync(orderWithLineItemWithCustomType.UpdateById(
                                        actions => actions.AddUpdate(action)));
                                Assert.Single(updatedOrder.LineItems);
                                Assert.Equal(stringFieldValue,
                                    updatedOrder.LineItems[0].Custom.Fields["string-field"]);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderSetCustomLineItemCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithCartWithSingleCustomLineItem(client, DefaultCartDraft,
                    async cart =>
                    {
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.Single(order.CustomLineItems);
                                var customLineItem = order.CustomLineItems[0];

                                var action = new SetCustomLineItemCustomTypeUpdateAction
                                {
                                    CustomLineItemId = customLineItem.Id,
                                    Type = type.ToKeyResourceIdentifier(),
                                    Fields = fields
                                };
                                var updatedOrder = await client
                                    .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                                Assert.Single(updatedOrder.CustomLineItems);
                                Assert.Equal(type.Id, updatedOrder.CustomLineItems[0].Custom.Type.Id);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderSetCustomLineItemCustomField()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithCartWithSingleCustomLineItem(client, DefaultCartDraft,
                    async cart =>
                    {
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.Single(order.CustomLineItems);
                                var customLineItem = order.CustomLineItems[0];

                                var setLineItemCustomTypeUpdateAction = new SetCustomLineItemCustomTypeUpdateAction
                                {
                                    CustomLineItemId = customLineItem.Id,
                                    Type = type.ToKeyResourceIdentifier(),
                                    Fields = fields
                                };
                                var orderWithCustomLineItemWithCustomType = await client
                                    .ExecuteAsync(order.UpdateById(actions =>
                                        actions.AddUpdate(setLineItemCustomTypeUpdateAction)));

                                Assert.Single(orderWithCustomLineItemWithCustomType.CustomLineItems);
                                Assert.Equal(type.Id,
                                    orderWithCustomLineItemWithCustomType.CustomLineItems[0].Custom.Type.Id);

                                // then update it's fields

                                var stringFieldValue = TestingUtility.RandomString();
                                var action =
                                    new SetCustomLineItemCustomFieldUpdateAction()
                                    {
                                        Name = "string-field",
                                        Value = stringFieldValue,
                                        CustomLineItemId = customLineItem.Id
                                    };
                                var updatedOrder = await client
                                    .ExecuteAsync(orderWithCustomLineItemWithCustomType.UpdateById(
                                        actions => actions.AddUpdate(action)));
                                Assert.Single(updatedOrder.CustomLineItems);
                                Assert.Equal(stringFieldValue,
                                    updatedOrder.CustomLineItems[0].Custom.Fields["string-field"]);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderAddPayment()
        {
            await WithPayment(client, async payment =>
            {
                await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                    async cart =>
                    {
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.Single(order.LineItems);
                                Assert.Null(order.PaymentInfo);

                                var action = new AddPaymentUpdateAction
                                {
                                    Payment = payment.ToKeyResourceIdentifier()
                                };
                                var updatedOrder = await client
                                    .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                                Assert.NotNull(updatedOrder.PaymentInfo);
                                Assert.Single(updatedOrder.PaymentInfo.Payments);
                                Assert.Equal(payment.Id, updatedOrder.PaymentInfo.Payments[0].Id);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderRemovePayment()
        {
            await WithPayment(client, async payment =>
            {
                await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                    async cart =>
                    {
                        await WithUpdateableOrder(client,
                            draft => DefaultOrderFromCartDraft(draft, cart),
                            async order =>
                            {
                                Assert.Single(order.LineItems);
                                Assert.Null(order.PaymentInfo);

                                //Add Payment first
                                var addPaymentAction = new AddPaymentUpdateAction
                                {
                                    Payment = payment.ToKeyResourceIdentifier()
                                };
                                var orderWithPayment = await client
                                    .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(addPaymentAction)));

                                Assert.NotNull(orderWithPayment.PaymentInfo);
                                Assert.Single(orderWithPayment.PaymentInfo.Payments);
                                Assert.Equal(payment.Id, orderWithPayment.PaymentInfo.Payments[0].Id);

                                //then remove it
                                var action = new RemovePaymentUpdateAction
                                {
                                    Payment = payment.ToKeyResourceIdentifier()
                                };
                                var updatedOrder = await client
                                    .ExecuteAsync(orderWithPayment.UpdateById(
                                        actions => actions.AddUpdate(action)));
                                Assert.Null(updatedOrder.PaymentInfo);
                                return updatedOrder;
                            });
                    });
            });
        }

        [Fact]
        public async void UpdateOrderSetLocale()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.LineItems);
                            Assert.Null(order.Locale);
                            var projectLanguages = GetProjectLanguages(client);
                            Assert.True(projectLanguages.Count > 0); //make sure that project has at least one language

                            var locale = projectLanguages[0];
                            var action = new SetLocaleUpdateAction
                            {
                                Locale = locale
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Equal(locale, updatedOrder.Locale);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetDeliveryItems()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            //First Add Delivery
                            Assert.Single(order.LineItems);
                            Assert.NotNull(order.ShippingInfo);
                            Assert.Empty(order.ShippingInfo.Deliveries);

                            var lineItem = order.LineItems[0];
                            var quantity = lineItem.Quantity - 1;

                            var deliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = quantity
                                }
                            };
                            var addDeliveryAction = new AddDeliveryUpdateAction
                            {
                                Items = deliveryItems,
                                Parcels = null
                            };
                            var orderWithDelivery = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(addDeliveryAction)));

                            Assert.Single(orderWithDelivery.ShippingInfo.Deliveries);
                            var delivery = orderWithDelivery.ShippingInfo.Deliveries[0];
                            Assert.Equal(quantity, delivery.Items[0].Quantity);

                            // Then set Delivery Items
                            var newQuantity = lineItem.Quantity;
                            var newDeliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = newQuantity
                                }
                            };
                            var action = new SetDeliveryItemsUpdateAction
                            {
                                Items = newDeliveryItems,
                                DeliveryId = delivery.Id
                            };

                            var updatedOrder = await client
                                .ExecuteAsync(orderWithDelivery.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
                            delivery = updatedOrder.ShippingInfo.Deliveries[0];

                            Assert.Single(delivery.Items);
                            Assert.Equal(lineItem.Id, delivery.Items[0].Id);
                            Assert.Equal(newQuantity, delivery.Items[0].Quantity);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderRemoveParcelFromDelivery()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            //First Add Delivery with parcel
                            Assert.Single(order.LineItems);
                            Assert.NotNull(order.ShippingInfo);
                            Assert.Empty(order.ShippingInfo.Deliveries);

                            var lineItem = order.LineItems[0];
                            var quantity = lineItem.Quantity;

                            var deliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = quantity
                                }
                            };
                            var smallParcelMeasurements = new ParcelMeasurements(1, 2, 3, 4);
                            var trackingData = new TrackingData("tracking-id-12", "carrier xyz", "provider foo",
                                "prov trans 56", false);
                            var parcels = new List<ParcelDraft>
                            {
                                new ParcelDraft
                                {
                                    Measurements = smallParcelMeasurements,
                                    TrackingData = trackingData,
                                    Items = deliveryItems
                                }
                            };
                            var addDeliveryUpdateAction = new AddDeliveryUpdateAction
                            {
                                Items = deliveryItems,
                                Parcels = parcels
                            };
                            var orderWithParcel = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(addDeliveryUpdateAction)));

                            Assert.Single(orderWithParcel.ShippingInfo.Deliveries);
                            var delivery = orderWithParcel.ShippingInfo.Deliveries[0];
                            Assert.Single(delivery.Parcels);
                            var parcel = delivery.Parcels[0];

                            //then remove it
                            var action = new RemoveParcelFromDeliveryUpdateAction
                            {
                                ParcelId = parcel.Id
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(orderWithParcel.UpdateById(
                                    actions => actions.AddUpdate(action)));
                            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
                            Assert.Empty(updatedOrder.ShippingInfo.Deliveries[0].Parcels);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderRemoveDelivery()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            //First Add Delivery
                            Assert.Single(order.LineItems);
                            Assert.NotNull(order.ShippingInfo);
                            Assert.Empty(order.ShippingInfo.Deliveries);

                            var lineItem = order.LineItems[0];
                            var quantity = lineItem.Quantity - 1;

                            var deliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = quantity
                                }
                            };
                            var addDeliveryAction = new AddDeliveryUpdateAction
                            {
                                Items = deliveryItems,
                                Parcels = null
                            };
                            var orderWithDelivery = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(addDeliveryAction)));

                            Assert.Single(orderWithDelivery.ShippingInfo.Deliveries);
                            var delivery = orderWithDelivery.ShippingInfo.Deliveries[0];
                            Assert.Equal(quantity, delivery.Items[0].Quantity);

                            //then remove it
                            var action = new RemoveDeliveryUpdateAction
                            {
                                DeliveryId = delivery.Id
                            };

                            var updatedOrder = await client
                                .ExecuteAsync(orderWithDelivery.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Empty(updatedOrder.ShippingInfo.Deliveries);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetParcelMeasurements()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            //First Add Delivery with parcel
                            Assert.Single(order.LineItems);
                            Assert.NotNull(order.ShippingInfo);
                            Assert.Empty(order.ShippingInfo.Deliveries);

                            var lineItem = order.LineItems[0];
                            var quantity = lineItem.Quantity;

                            var deliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = quantity
                                }
                            };
                            var smallParcelMeasurements = new ParcelMeasurements(1, 2, 3, 4);
                            var trackingData = new TrackingData("tracking-id-12", "carrier xyz", "provider foo",
                                "prov trans 56", false);
                            var parcels = new List<ParcelDraft>
                            {
                                new ParcelDraft
                                {
                                    Measurements = smallParcelMeasurements,
                                    TrackingData = trackingData,
                                    Items = deliveryItems
                                }
                            };
                            var addDeliveryUpdateAction = new AddDeliveryUpdateAction
                            {
                                Items = deliveryItems,
                                Parcels = parcels
                            };
                            var orderWithParcel = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(addDeliveryUpdateAction)));

                            Assert.Single(orderWithParcel.ShippingInfo.Deliveries);
                            var delivery = orderWithParcel.ShippingInfo.Deliveries[0];
                            Assert.Single(delivery.Parcels);
                            var parcel = delivery.Parcels[0];

                            //then set measurements
                            var newMeasurements = new ParcelMeasurements(10, 12, 13, 14);
                            var action = new SetParcelMeasurementsUpdateAction
                            {
                                ParcelId = parcel.Id,
                                Measurements = newMeasurements
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(orderWithParcel.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
                            Assert.Single(updatedOrder.ShippingInfo.Deliveries[0].Parcels);
                            parcel = updatedOrder.ShippingInfo.Deliveries[0].Parcels[0];

                            Assert.Equal(newMeasurements, parcel.Measurements);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetParcelTrackingData()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            //First Add Delivery with parcel
                            Assert.Single(order.LineItems);
                            Assert.NotNull(order.ShippingInfo);
                            Assert.Empty(order.ShippingInfo.Deliveries);

                            var lineItem = order.LineItems[0];
                            var quantity = lineItem.Quantity;

                            var deliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = quantity
                                }
                            };
                            var smallParcelMeasurements = new ParcelMeasurements(1, 2, 3, 4);
                            var trackingData = new TrackingData("tracking-id-12", "carrier xyz", "provider foo",
                                "prov trans 56", false);
                            var parcels = new List<ParcelDraft>
                            {
                                new ParcelDraft
                                {
                                    Measurements = smallParcelMeasurements,
                                    TrackingData = trackingData,
                                    Items = deliveryItems
                                }
                            };
                            var addDeliveryUpdateAction = new AddDeliveryUpdateAction
                            {
                                Items = deliveryItems,
                                Parcels = parcels
                            };
                            var orderWithParcel = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(addDeliveryUpdateAction)));

                            Assert.Single(orderWithParcel.ShippingInfo.Deliveries);
                            var delivery = orderWithParcel.ShippingInfo.Deliveries[0];
                            Assert.Single(delivery.Parcels);
                            var parcel = delivery.Parcels[0];

                            //then set tracking data
                            var ran = TestingUtility.RandomInt();
                            var newTrackingData = new TrackingData($"tracking-{ran}", $"carrier-{ran}",
                                $"provider-{ran}", $"prov-trans--{ran}", false);
                            var action = new SetParcelTrackingDataUpdateAction
                            {
                                ParcelId = parcel.Id,
                                TrackingData = newTrackingData
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(orderWithParcel.UpdateById(
                                    actions => actions.AddUpdate(action)));


                            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
                            Assert.Single(updatedOrder.ShippingInfo.Deliveries[0].Parcels);
                            parcel = updatedOrder.ShippingInfo.Deliveries[0].Parcels[0];

                            Assert.Equal(newTrackingData.TrackingId, parcel.TrackingData.TrackingId);
                            Assert.Equal(newTrackingData.Carrier, parcel.TrackingData.Carrier);
                            Assert.Equal(newTrackingData.Provider, parcel.TrackingData.Provider);
                            Assert.Equal(newTrackingData.ProviderTransaction, parcel.TrackingData.ProviderTransaction);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetParcelItems()
        {
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            //First Add Delivery with parcel
                            Assert.Single(order.LineItems);
                            Assert.NotNull(order.ShippingInfo);
                            Assert.Empty(order.ShippingInfo.Deliveries);

                            var lineItem = order.LineItems[0];
                            var quantity = lineItem.Quantity - 1;

                            var deliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem
                                {
                                    Id = lineItem.Id,
                                    Quantity = quantity
                                }
                            };
                            var smallParcelMeasurements = new ParcelMeasurements(1, 2, 3, 4);
                            var trackingData = new TrackingData("tracking-id-12", "carrier xyz", "provider foo",
                                "prov trans 56", false);
                            var parcels = new List<ParcelDraft>
                            {
                                new ParcelDraft
                                {
                                    Measurements = smallParcelMeasurements,
                                    TrackingData = trackingData,
                                    Items = deliveryItems
                                }
                            };
                            var addDeliveryUpdateAction = new AddDeliveryUpdateAction
                            {
                                Items = deliveryItems,
                                Parcels = parcels
                            };
                            var orderWithParcel = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(addDeliveryUpdateAction)));

                            Assert.Single(orderWithParcel.ShippingInfo.Deliveries);
                            var delivery = orderWithParcel.ShippingInfo.Deliveries[0];
                            Assert.Single(delivery.Parcels);
                            var parcel = delivery.Parcels[0];

                            // Then set Parcel Items
                            var newQuantity = lineItem.Quantity;
                            var newDeliveryItems = new List<DeliveryItem>
                            {
                                new DeliveryItem {Id = lineItem.Id, Quantity = newQuantity}
                            };
                            var action = new SetParcelItemsUpdateAction
                            {
                                Items = newDeliveryItems,
                                ParcelId = parcel.Id
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(orderWithParcel.UpdateById(
                                    actions => actions.AddUpdate(action)));


                            Assert.Single(updatedOrder.ShippingInfo.Deliveries);
                            delivery = updatedOrder.ShippingInfo.Deliveries[0];

                            Assert.Single(delivery.Items);
                            Assert.Single(delivery.Parcels);
                            Assert.Single(delivery.Parcels[0].Items);
                            Assert.Equal(lineItem.Id, delivery.Parcels[0].Items[0].Id);
                            Assert.Equal(newQuantity, delivery.Parcels[0].Items[0].Quantity);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetLineItemShippingDetails()
        {
            var addresses = new List<Address> {TestingUtility.GetRandomAddress()};
            await WithCartWithSingleLineItem(client, 2,
                cartDraft => DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.LineItems);
                            Assert.Single(order.ItemShippingAddresses);
                            var lineItem = order.LineItems[0];
                            var addressKey = order.ItemShippingAddresses[0].Key;
                            var quantity = order.LineItems[0].Quantity;
                            var itemShippingDetailsDraft =
                                TestingUtility.GetItemShippingDetailsDraft(addressKey, quantity);

                            var action = new SetLineItemShippingDetailsUpdateAction
                            {
                                LineItemId = lineItem.Id,
                                ShippingDetails = itemShippingDetailsDraft
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.LineItems);

                            Assert.NotNull(updatedOrder.LineItems[0].ShippingDetails);
                            Assert.Single(updatedOrder.LineItems[0].ShippingDetails.Targets);
                            Assert.Equal(itemShippingDetailsDraft.Targets[0].Quantity,
                                updatedOrder.LineItems[0].ShippingDetails.Targets[0].Quantity);
                            Assert.Equal(itemShippingDetailsDraft.Targets[0].AddressKey,
                                updatedOrder.LineItems[0].ShippingDetails.Targets[0].AddressKey);

                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderSetCustomLineItemShippingDetails()
        {
            var addresses = new List<Address> {TestingUtility.GetRandomAddress()};
            await WithCartWithSingleCustomLineItem(client,
                cartDraft => DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.CustomLineItems);
                            Assert.Single(order.ItemShippingAddresses);
                            var customLineItem = order.CustomLineItems[0];
                            var addressKey = order.ItemShippingAddresses[0].Key;
                            var quantity = order.CustomLineItems[0].Quantity;
                            var itemShippingDetailsDraft =
                                TestingUtility.GetItemShippingDetailsDraft(addressKey, quantity);

                            var action = new SetCustomLineItemShippingDetailsUpdateAction
                            {
                                CustomLineItemId = customLineItem.Id,
                                ShippingDetails = itemShippingDetailsDraft
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.CustomLineItems);

                            Assert.NotNull(updatedOrder.CustomLineItems[0].ShippingDetails);
                            Assert.Single(updatedOrder.CustomLineItems[0].ShippingDetails.Targets);
                            Assert.Equal(itemShippingDetailsDraft.Targets[0].Quantity,
                                updatedOrder.CustomLineItems[0].ShippingDetails.Targets[0].Quantity);
                            Assert.Equal(itemShippingDetailsDraft.Targets[0].AddressKey,
                                updatedOrder.CustomLineItems[0].ShippingDetails.Targets[0].AddressKey);

                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderAddItemShippingAddress()
        {
            var address = TestingUtility.GetRandomAddress();
            await WithCartWithSingleLineItem(client, 2, DefaultCartDraft,
                async cart =>
                {
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.LineItems);
                            Assert.Empty(order.ItemShippingAddresses);

                            var action = new AddItemShippingAddressUpdateAction
                            {
                                Address = address
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.ItemShippingAddresses);
                            Assert.Equal(address.Key, updatedOrder.ItemShippingAddresses[0].Key);
                            Assert.Equal(address.Country, updatedOrder.ItemShippingAddresses[0].Country);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderRemoveItemShippingAddress()
        {
            var addresses = new List<Address> {TestingUtility.GetRandomAddress()};
            await WithCartWithSingleLineItem(client, 2,
                cartDraft => DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.LineItems);
                            Assert.Single(order.ItemShippingAddresses);
                            var key = order.ItemShippingAddresses[0].Key;
                            var action = new RemoveItemShippingAddressUpdateAction
                            {
                                AddressKey = key
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Empty(updatedOrder.ItemShippingAddresses);
                            return updatedOrder;
                        });
                });
        }

        [Fact]
        public async void UpdateOrderUpdateItemShippingAddress()
        {
            var addresses = new List<Address> {TestingUtility.GetRandomAddress()};
            await WithCartWithSingleLineItem(client, 2,
                cartDraft => DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    await WithUpdateableOrder(client,
                        draft => DefaultOrderFromCartDraft(draft, cart),
                        async order =>
                        {
                            Assert.Single(order.LineItems);
                            Assert.Single(order.ItemShippingAddresses);

                            var address = order.ItemShippingAddresses[0];
                            var state = $"{address.Country}_State_{TestingUtility.RandomInt()}";
                            address.State = state;

                            var action = new UpdateItemShippingAddressUpdateAction
                            {
                                Address = address
                            };
                            var updatedOrder = await client
                                .ExecuteAsync(order.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedOrder.ItemShippingAddresses);
                            Assert.Equal(state, updatedOrder.ItemShippingAddresses[0].State);
                            return updatedOrder;
                        });
                });
        }

        #endregion
    }
}