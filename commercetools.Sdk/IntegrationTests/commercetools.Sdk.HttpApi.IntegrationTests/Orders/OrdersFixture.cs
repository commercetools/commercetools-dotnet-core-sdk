using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Orders.UpdateActions;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.HttpApi.IntegrationTests.Carts;
using commercetools.Sdk.HttpApi.IntegrationTests.Channels;
using commercetools.Sdk.HttpApi.IntegrationTests.Customers;
using commercetools.Sdk.HttpApi.IntegrationTests.Payments;
using commercetools.Sdk.HttpApi.IntegrationTests.Products;
using commercetools.Sdk.HttpApi.IntegrationTests.Project;
using commercetools.Sdk.HttpApi.IntegrationTests.States;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Orders
{
    public class OrdersFixture : ClientFixture, IDisposable
    {
        private readonly CartFixture cartFixture;
        private readonly ProductFixture productFixture;
        private readonly CustomerFixture customerFixture;
        private readonly ChannelFixture channelFixture;
        private readonly StatesFixture statesFixture;
        private readonly TypeFixture typeFixture;
        private readonly PaymentsFixture paymentsFixture;
        private readonly ProjectFixture projectFixture;

        public List<Order> OrdersToDelete { get; }

        public OrdersFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.OrdersToDelete = new List<Order>();
            this.cartFixture = new CartFixture(serviceProviderFixture);
            this.productFixture = new ProductFixture(serviceProviderFixture);
            this.customerFixture = new CustomerFixture(serviceProviderFixture);
            this.channelFixture = new ChannelFixture(serviceProviderFixture);
            this.statesFixture = new StatesFixture(serviceProviderFixture);
            this.typeFixture = new TypeFixture(serviceProviderFixture);
            this.paymentsFixture = new PaymentsFixture(serviceProviderFixture);
            this.projectFixture = new ProjectFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.OrdersToDelete.Reverse();
            foreach (Order order in this.OrdersToDelete)
            {
                var deletedType = this.TryDeleteResource(order).Result;
            }
            this.cartFixture.Dispose();
            this.productFixture.Dispose();
            this.customerFixture.Dispose();
            this.channelFixture.Dispose();
            this.statesFixture.Dispose();
            this.typeFixture.Dispose();
            this.paymentsFixture.Dispose();
            this.projectFixture.Dispose();
        }

        public OrderFromCartDraft GetOrderFromCartDraft(bool withDefaultShippingCountry = true, bool withShippingMethod = false, bool withOrderNumber = true, bool withCustomer = true, bool withCustomLineItem = false, bool withItemShippingAddress = false)
        {
            //Create A Cart
            Cart cart = null;
            if (withCustomLineItem)
            {
                cart = this.cartFixture.CreateCartWithCustomLineItem(
                    withDefaultShippingCountry: withDefaultShippingCountry,
                    withCustomer: withCustomer, withItemShippingAddress: withItemShippingAddress);
            }
            else
            {
                cart = this.cartFixture.CreateCartWithLineItem(withDefaultShippingCountry: withDefaultShippingCountry,
                    withShippingMethod: withShippingMethod, withCustomer: withCustomer,
                    withItemShippingAddress: withItemShippingAddress);
            }

            //Then Create Order from this Cart
            OrderFromCartDraft orderFromCartDraft = new OrderFromCartDraft
            {
                Id = cart.Id,
                Version = cart.Version
            };

            if (withOrderNumber)
            {
                orderFromCartDraft.OrderNumber = TestingUtility.RandomString(10);
            }

            return orderFromCartDraft;
        }

        /// <summary>
        /// Create Order from cart
        /// </summary>
        /// <param name="withReturnInfo">if yes, add returnInfo to the order with one lineItem</param>
        /// <param name="withShippingMethod">if yes, create the cart with the shipping Method</param>
        /// <param name="withDefaultShippingCountry">if yes, create the cart in DE else create it with random country and random state</param>
        /// <param name="withOrderNumber">if yes, create the order with order number</param>
        /// <param name="withCustomer">if yes, create the order with customer</param>
        /// <param name="withCustomLineItem">if yes, create the order with Custom LineItem</param>
        /// <param name="withItemShippingAddress">if yes, create the order with itemShippingAddresses</param>
        /// <returns></returns>
        public Order CreateOrderFromCart(bool withReturnInfo = false, bool withDefaultShippingCountry = true, bool withShippingMethod = false, bool withOrderNumber = true, bool withCustomer = true, bool withCustomLineItem = false, bool withItemShippingAddress = false)
        {
            var order = this.CreateOrderFromCart(this.GetOrderFromCartDraft(withDefaultShippingCountry, withShippingMethod, withOrderNumber, withCustomer, withCustomLineItem, withItemShippingAddress));
            if (withReturnInfo)
            {
                IClient commerceToolsClient = this.GetService<IClient>();

                var lineItemId = order.LineItems[0].Id;
                var lineItemReturnItemDraft = this.GetLineItemReturnItemDraft(lineItemId);

                var updateActions = new List<UpdateAction<Order>>();
                var addReturnInfoUpdateAction = new AddReturnInfoUpdateAction
                {
                    Items = new List<ReturnItemDraft>{lineItemReturnItemDraft}
                };
                updateActions.Add(addReturnInfoUpdateAction);

                order = commerceToolsClient
                    .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                    .Result;
            }
            return order;
        }

        /// <summary>
        /// Add a Delivery to the order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="quantity"></param>
        /// <param name="withParcel"></param>
        /// <returns>Order with Delivery</returns>
        public Order AddDelivery(Order order, long quantity = 1, bool withParcel = false)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var lineItemId = order.LineItems[0].Id;
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
            var addDeliveryAction = new AddDeliveryUpdateAction { Items = deliveryItems };
            if (withParcel)
            {
                addDeliveryAction.Parcels = parcels;
            }
            updateActions.Add(addDeliveryAction);

            var retrievedOrder = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Order>(order.Id, order.Version, updateActions))
                .Result;

            return retrievedOrder;
        }
        public Order CreateOrderFromCart(OrderFromCartDraft orderFromCartDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Order order = commerceToolsClient.ExecuteAsync(new CreateCommand<Order>(orderFromCartDraft)).Result;
            AddCartToDeletedCarts(order.Cart.Id);
            return order;
        }

        public void AddCartToDeletedCarts(string cartId)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var cart = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<Cart>(cartId)).Result;
            this.cartFixture.CartToDelete.Add(cart);
        }

        /// <summary>
        /// Create Order Import Draft
        /// </summary>
        /// <param name="bySku">if true, then use sku to create the lineItemImportDraft else then use the productId and VariantId</param>
        /// <returns></returns>
        public OrderImportDraft GetOrderImportDraft(bool bySku = false)
        {
            //Create Product
            var product = this.productFixture.CreateProduct();
            this.productFixture.ProductsToDelete.Add(product);

            //CreateLineItemImportDraft
            LineItemImportDraft lineItemImportDraft = null;
            lineItemImportDraft = bySku ? this.productFixture.GetLineItemImportDraftBySku(product.MasterData.Staged.MasterVariant.Sku)
                                    : this.productFixture.GetLineItemImportDraftByProductId(product.Id, 1);
            //Create OrderImportDraft
            var orderImportDraft = new OrderImportDraft
            {
                LineItems = new List<LineItemImportDraft> {lineItemImportDraft},
                OrderNumber = TestingUtility.RandomString(10),
                TotalPrice = lineItemImportDraft.Price.Value.ToMoney()
            };
            return orderImportDraft;
        }

        public Channel CreateNewChannel(ChannelRole role)
        {
            var channel = this.channelFixture.CreateChannel(role);
            this.channelFixture.ChannelsToDelete.Add(channel);
            return channel;
        }

        public LineItemReturnItemDraft GetLineItemReturnItemDraft(string lineItemId)
        {
            var lineItemReturnItemDraft = new LineItemReturnItemDraft
            {
                Quantity = 1,
                Comment = "comment",
                LineItemId = lineItemId,
                ShipmentState = ReturnShipmentState.Returned
            };
            return lineItemReturnItemDraft;
        }

        public State CreateNewState(StateType stateType = StateType.ProductState,bool initial = true)
        {
            string stateKey = $"Key-{TestingUtility.RandomInt()}";
            State state = this.statesFixture.CreateState(stateKey, stateType, initial);
            this.statesFixture.StatesToDelete.Add(state);
            return state;
        }

        public Customer CreateNewCustomer()
        {
            var customer = this.customerFixture.CreateCustomer();
            this.customerFixture.CustomersToDelete.Add(customer);
            return customer;
        }

        public Fields CreateNewFields()
        {
            Fields fields = this.typeFixture.CreateNewFields();
            return fields;
        }

        public Type CreateNewType()
        {
            Type type = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(type);
            return type;
        }

        public Payment CreatePayment()
        {
            var payment = this.paymentsFixture.CreatePayment();
            this.paymentsFixture.PaymentsToDelete.Add(payment);
            return payment;
        }

        public List<State> GetStandardStates()
        {
            var states = this.statesFixture.GetStandardStates();
            return states;
        }
        public List<string> GetProjectLanguages()
        {
            return this.projectFixture.GetProjectLanguages();
        }

        public ItemShippingDetailsDraft GetItemShippingDetailsDraft(string addressKey, long quantity)
        {
            return this.cartFixture.GetItemShippingDetailsDraft(addressKey, quantity);
        }
    }
}
