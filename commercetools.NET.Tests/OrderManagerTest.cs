using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Carts;
using commercetools.Customers;
using commercetools.Messages;
using commercetools.Orders;
using commercetools.Products;
using commercetools.ProductTypes;
using commercetools.Project;
using commercetools.ShippingMethods;
using commercetools.TaxCategories;
using commercetools.Zones;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the OrderManager class.
    /// </summary>
    [TestFixture]
    public class OrderManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Cart> _testCarts;
        private List<Customer> _testCustomers;
        private Order _testOrder;
        private Product _testProduct;
        private ProductType _testProductType;
        private ShippingMethod _testShippingMethod;
        private TaxCategory _testTaxCategory;
        private Zone _testZone;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Project.Project> project = _client.Project().GetProjectAsync();
            project.Wait();
            _project = project.Result;

            _testCustomers = new List<Customer>();

            for (int i = 0; i < 5; i++)
            {
                CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
                Task<CustomerCreatedMessage> customerTask = _client.Customers().CreateCustomerAsync(customerDraft);
                customerTask.Wait();
                CustomerCreatedMessage customerCreatedMessage = customerTask.Result;

                Assert.NotNull(customerCreatedMessage.Customer);
                Assert.NotNull(customerCreatedMessage.Customer.Id);

                _testCustomers.Add(customerCreatedMessage.Customer);
            }

            _testCarts = new List<Cart>();
            Task<Cart> cartTask;

            for (int i = 0; i < 5; i++)
            {
                CartDraft cartDraft = Helper.GetTestCartDraft(_project, _testCustomers[i].Id);
                cartTask = _client.Carts().CreateCartAsync(cartDraft);
                cartTask.Wait();
                Cart cart = cartTask.Result;

                Assert.NotNull(cart.Id);

                _testCarts.Add(cart);
            }

            ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
            Task<ProductType> testProductType = _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
            testProductType.Wait();
            _testProductType = testProductType.Result;

            Assert.NotNull(_testProductType.Id);

            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<TaxCategory> taxCategory = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            taxCategory.Wait();
            _testTaxCategory = taxCategory.Result;

            Assert.NotNull(_testTaxCategory.Id);

            ZoneDraft zoneDraft  = Helper.GetTestZoneDraft(_project);
            Task<Zone> zoneTask = _client.Zones().CreateZoneAsync(zoneDraft);
            zoneTask.Wait();
            _testZone = zoneTask.Result;

            Assert.NotNull(_testZone.Id);

            ShippingMethodDraft shippingMethodDraft = Helper.GetTestShippingMethodDraft(_project, _testTaxCategory, _testZone);
            Task<ShippingMethod> shippingMethod = _client.ShippingMethods().CreateShippingMethodAsync(shippingMethodDraft);
            shippingMethod.Wait();
            _testShippingMethod = shippingMethod.Result;

            Assert.NotNull(_testShippingMethod.Id);

            ProductDraft productDraft = Helper.GetTestProductDraft(_project, _testProductType.Id, _testTaxCategory.Id);
            Task<Product> testProduct = _client.Products().CreateProductAsync(productDraft);
            testProduct.Wait();
            _testProduct = testProduct.Result;

            Assert.NotNull(_testProduct.Id);

            int quantity = 1;
            cartTask = _client.Carts().AddLineItemAsync(_testCarts[0], _testProduct, _testProduct.MasterData.Current.MasterVariant, quantity);
            cartTask.Wait();
            _testCarts[0] = cartTask.Result;

            Assert.NotNull(_testCarts[0].Id);
            Assert.NotNull(_testCarts[0].LineItems);
            Assert.AreEqual(_testCarts[0].LineItems.Count, 1);
            Assert.AreEqual(_testCarts[0].LineItems[0].ProductId, _testProduct.Id);
            Assert.AreEqual(_testCarts[0].LineItems[0].Variant.Id, _testProduct.MasterData.Current.MasterVariant.Id);
            Assert.AreEqual(_testCarts[0].LineItems[0].Quantity, quantity);

            OrderFromCartDraft orderFromCartDraft = Helper.GetTestOrderFromCartDraft(_testCarts[0]);
            Task<Order> order = _client.Orders().CreateOrderFromCartAsync(orderFromCartDraft);
            order.Wait();
            _testOrder = order.Result;

            Assert.NotNull(_testOrder.Id);

            cartTask = _client.Carts().GetCartByIdAsync(_testCarts[0].Id);
            cartTask.Wait();
            _testCarts[0] = cartTask.Result;

            Assert.NotNull(_testCarts[0].Id);
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            Task task = _client.Orders().DeleteOrderAsync(_testOrder);
            task.Wait();

            foreach (Cart cart in _testCarts)
            {
                task = _client.Carts().DeleteCartAsync(cart);
                task.Wait();
            }

            foreach (Customer customer in _testCustomers)
            {
                task = _client.Customers().DeleteCustomerAsync(customer);
                task.Wait();
            }

            task = _client.Products().DeleteProductAsync(_testProduct);
            task.Wait();

            task = _client.ProductTypes().DeleteProductTypeAsync(_testProductType);
            task.Wait();

            task = _client.ShippingMethods().DeleteShippingMethodAsync(_testShippingMethod);
            task.Wait();

            task = _client.TaxCategories().DeleteTaxCategoryAsync(_testTaxCategory);
            task.Wait();

            task = _client.Zones().DeleteZoneAsync(_testZone);
            task.Wait();
        }

        /// <summary>
        /// Tests the OrderManager.GetOrderByIdAsync method.
        /// </summary>
        /// <see cref="OrderManager.GetOrderByIdAsync"/>
        [Test]
        public async Task ShouldGetOrderByIdAsync()
        {
            Order order = await _client.Orders().GetOrderByIdAsync(_testOrder.Id);

            Assert.NotNull(order.Id);
            Assert.AreEqual(order.Id, _testOrder.Id);
        }

        /// <summary>
        /// Tests the OrderManager.QueryOrdersAsync method.
        /// </summary>
        /// <see cref="OrderManager.QueryOrdersAsync"/>
        [Test]
        public async Task ShouldQueryOrdersAsync()
        {
            OrderQueryResult result = await _client.Orders().QueryOrdersAsync();

            Assert.NotNull(result.Results);
            Assert.GreaterOrEqual(result.Results.Count, 1);

            int limit = 2;
            result = await _client.Orders().QueryOrdersAsync(limit: limit);

            Assert.NotNull(result.Results);
            Assert.LessOrEqual(result.Results.Count, limit);
        }

        /// <summary>
        /// Tests the OrderManager.CreateOrderFromCartAsync and DeleteOrderAsync methods.
        /// </summary>
        /// <see cref="OrderManager.CreateOrderFromCartAsync"/>
        /// <seealso cref="OrderManager.DeleteOrderAsync(commercetools.Orders.Order)"/>
        [Test]
        public async Task ShouldCreateOrderFromCartAndDeleteOrderAsync()
        {
            CartDraft cartDraft = Helper.GetTestCartDraft(_project, _testCustomers[0].Id);
            Cart cart = await _client.Carts().CreateCartAsync(cartDraft);

            Assert.NotNull(cart);

            int quantity = 3;
            cart = await _client.Carts().AddLineItemAsync(cart, _testProduct, _testProduct.MasterData.Current.MasterVariant, quantity);

            Assert.NotNull(cart.Id);
            Assert.NotNull(cart.LineItems);
            Assert.AreEqual(cart.LineItems.Count, 1);
            Assert.AreEqual(cart.LineItems[0].ProductId, _testProduct.Id);
            Assert.AreEqual(cart.LineItems[0].Variant.Id, _testProduct.MasterData.Current.MasterVariant.Id);
            Assert.AreEqual(cart.LineItems[0].Quantity, quantity);

            OrderFromCartDraft orderFromCartDraft = Helper.GetTestOrderFromCartDraft(cart);
            Order order = await _client.Orders().CreateOrderFromCartAsync(orderFromCartDraft);

            Assert.NotNull(order);

            // To get the new version number.
            cart = await _client.Carts().GetCartByIdAsync(cart.Id);

            string deletedOrderId = order.Id;

            await _client.Orders().DeleteOrderAsync(order);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate
                {
                    Task task = _client.Orders().GetOrderByIdAsync(deletedOrderId);
                    task.Wait();
                });

            await _client.Carts().DeleteCartAsync(cart);
        }

        /// <summary>
        /// Tests the OrderManager.UpdateOrderAsync method.
        /// </summary>
        /// <see cref="OrderManager.UpdateOrderAsync(commercetools.Orders.Order, System.Collections.Generic.List{Newtonsoft.Json.Linq.JObject})"/>
        [Test]
        public async Task ShouldUpdateOrderAsync()
        {
            List<JObject> actions = new List<JObject>();

            OrderState newOrderState = OrderState.Confirmed;
            string newOrderNumber = Helper.GetRandomNumber(10000, 99999).ToString();

            actions.Add(
                JObject.FromObject(new
                {
                    action = "changeOrderState",
                    orderState = newOrderState.ToString()
                })
            );

            actions.Add(
                JObject.FromObject(new
                {
                    action = "setOrderNumber",
                    orderNumber = newOrderNumber
                })
            );

            _testOrder = await _client.Orders().UpdateOrderAsync(_testOrder, actions);

            Assert.NotNull(_testOrder.Id);
            Assert.AreEqual(_testOrder.OrderState, newOrderState);
            Assert.AreEqual(_testOrder.OrderNumber, newOrderNumber);
        }
    }
}