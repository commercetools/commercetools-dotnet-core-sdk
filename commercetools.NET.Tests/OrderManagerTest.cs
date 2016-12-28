using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Carts;
using commercetools.Carts.UpdateActions;
using commercetools.Customers;
using commercetools.Messages;
using commercetools.Orders;
using commercetools.Orders.UpdateActions;
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
        private bool _createdTestZone = false;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            _testCustomers = new List<Customer>();

            for (int i = 0; i < 5; i++)
            {
                CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
                Task<Response<CustomerCreatedMessage>> customerTask = _client.Customers().CreateCustomerAsync(customerDraft);
                customerTask.Wait();
                Assert.IsTrue(customerTask.Result.Success);

                CustomerCreatedMessage customerCreatedMessage = customerTask.Result.Result;
                Assert.NotNull(customerCreatedMessage.Customer);
                Assert.NotNull(customerCreatedMessage.Customer.Id);

                _testCustomers.Add(customerCreatedMessage.Customer);
            }

            _testCarts = new List<Cart>();
            Task<Response<Cart>> cartTask;

            for (int i = 0; i < 5; i++)
            {
                CartDraft cartDraft = Helper.GetTestCartDraft(_project, _testCustomers[i].Id);
                cartTask = _client.Carts().CreateCartAsync(cartDraft);
                cartTask.Wait();
                Assert.IsTrue(cartTask.Result.Success);

                Cart cart = cartTask.Result.Result;
                Assert.NotNull(cart.Id);

                _testCarts.Add(cart);
            }

            ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
            Task<Response<ProductType>> productTypeTask = _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
            productTypeTask.Wait();
            Assert.IsTrue(productTypeTask.Result.Success);

            _testProductType = productTypeTask.Result.Result;
            Assert.NotNull(_testProductType.Id);

            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<Response<TaxCategory>> taxCategoryTask = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            taxCategoryTask.Wait();
            Assert.IsTrue(taxCategoryTask.Result.Success);

            _testTaxCategory = taxCategoryTask.Result.Result;
            Assert.NotNull(_testTaxCategory.Id);

            Task<Response<ZoneQueryResult>> zoneQueryResultTask = _client.Zones().QueryZonesAsync();
            zoneQueryResultTask.Wait();

            if (zoneQueryResultTask.Result.Success && zoneQueryResultTask.Result.Result.Results.Count > 0)
            {
                _testZone = zoneQueryResultTask.Result.Result.Results[0];
                _createdTestZone = false;
            }
            else
            {
                ZoneDraft zoneDraft = Helper.GetTestZoneDraft();
                Task<Response<Zone>> zoneTask = _client.Zones().CreateZoneAsync(zoneDraft);
                zoneTask.Wait();
                Assert.IsTrue(zoneTask.Result.Success);

                _testZone = zoneTask.Result.Result;
                _createdTestZone = true;
            }

            Assert.NotNull(_testZone.Id);

            ShippingMethodDraft shippingMethodDraft = Helper.GetTestShippingMethodDraft(_project, _testTaxCategory, _testZone);
            Task<Response<ShippingMethod>> shippingMethodTask = _client.ShippingMethods().CreateShippingMethodAsync(shippingMethodDraft);
            shippingMethodTask.Wait();
            Assert.IsTrue(shippingMethodTask.Result.Success);

            _testShippingMethod = shippingMethodTask.Result.Result;
            Assert.NotNull(_testShippingMethod.Id);

            ProductDraft productDraft = Helper.GetTestProductDraft(_project, _testProductType.Id, _testTaxCategory.Id);
            Task<Response<Product>> productTask = _client.Products().CreateProductAsync(productDraft);
            productTask.Wait();
            Assert.IsTrue(productTask.Result.Success);

            _testProduct = productTask.Result.Result;
            Assert.NotNull(_testProduct.Id);

            int quantity = 1;
            AddLineItemAction addLineItemAction = new AddLineItemAction(_testProduct.Id, _testProduct.MasterData.Current.MasterVariant.Id, quantity);
            cartTask = _client.Carts().UpdateCartAsync(_testCarts[0], addLineItemAction);
            cartTask.Wait();
            Assert.IsTrue(cartTask.Result.Success);

            _testCarts[0] = cartTask.Result.Result;
            Assert.NotNull(_testCarts[0].Id);
            Assert.NotNull(_testCarts[0].LineItems);
            Assert.AreEqual(_testCarts[0].LineItems.Count, 1);
            Assert.AreEqual(_testCarts[0].LineItems[0].ProductId, _testProduct.Id);
            Assert.AreEqual(_testCarts[0].LineItems[0].Variant.Id, _testProduct.MasterData.Current.MasterVariant.Id);
            Assert.AreEqual(_testCarts[0].LineItems[0].Quantity, quantity);

            OrderFromCartDraft orderFromCartDraft = Helper.GetTestOrderFromCartDraft(_testCarts[0]);
            Task<Response<Order>> orderTask = _client.Orders().CreateOrderFromCartAsync(orderFromCartDraft);
            orderTask.Wait();
            Assert.IsTrue(orderTask.Result.Success);

            _testOrder = orderTask.Result.Result;
            Assert.NotNull(_testOrder.Id);

            cartTask = _client.Carts().GetCartByIdAsync(_testCarts[0].Id);
            cartTask.Wait();
            Assert.IsTrue(cartTask.Result.Success);

            _testCarts[0] = cartTask.Result.Result;
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

            if (_createdTestZone)
            {
                task = _client.Zones().DeleteZoneAsync(_testZone);
                task.Wait();
            }
        }

        /// <summary>
        /// Tests the OrderManager.GetOrderByIdAsync method.
        /// </summary>
        /// <see cref="OrderManager.GetOrderByIdAsync"/>
        [Test]
        public async Task ShouldGetOrderByIdAsync()
        {
            Response<Order> response = await _client.Orders().GetOrderByIdAsync(_testOrder.Id);
            Assert.IsTrue(response.Success);

            Order order = response.Result;
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
            Response<OrderQueryResult> response = await _client.Orders().QueryOrdersAsync();
            Assert.IsTrue(response.Success);

            OrderQueryResult orderQueryResult = response.Result;
            Assert.NotNull(orderQueryResult.Results);
            Assert.GreaterOrEqual(orderQueryResult.Results.Count, 1);

            int limit = 2;
            response = await _client.Orders().QueryOrdersAsync(limit: limit);
            Assert.IsTrue(response.Success);

            orderQueryResult = response.Result;
            Assert.NotNull(orderQueryResult.Results);
            Assert.LessOrEqual(orderQueryResult.Results.Count, limit);
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
            Response<Cart> cartResponse = await _client.Carts().CreateCartAsync(cartDraft);
            Assert.IsTrue(cartResponse.Success);

            Cart cart = cartResponse.Result;
            Assert.NotNull(cart.Id);

            int quantity = 3;
            AddLineItemAction addLineItemAction = new AddLineItemAction(_testProduct.Id, _testProduct.MasterData.Current.MasterVariant.Id, quantity);
            cartResponse = await _client.Carts().UpdateCartAsync(cart, addLineItemAction);
            Assert.IsTrue(cartResponse.Success);

            cart = cartResponse.Result;
            Assert.NotNull(cart.Id);
            Assert.NotNull(cart.LineItems);
            Assert.AreEqual(cart.LineItems.Count, 1);
            Assert.AreEqual(cart.LineItems[0].ProductId, _testProduct.Id);
            Assert.AreEqual(cart.LineItems[0].Variant.Id, _testProduct.MasterData.Current.MasterVariant.Id);
            Assert.AreEqual(cart.LineItems[0].Quantity, quantity);

            OrderFromCartDraft orderFromCartDraft = Helper.GetTestOrderFromCartDraft(cart);
            Response<Order> orderResponse = await _client.Orders().CreateOrderFromCartAsync(orderFromCartDraft);
            Assert.IsTrue(orderResponse.Success);

            Order order = orderResponse.Result;
            Assert.NotNull(order.Id);

            // To get the new version number.
            cartResponse = await _client.Carts().GetCartByIdAsync(cart.Id);
            Assert.IsTrue(cartResponse.Success);
            cart = cartResponse.Result;

            string deletedOrderId = order.Id;

            Response<JObject> response = await _client.Orders().DeleteOrderAsync(order);
            Assert.IsTrue(response.Success);

            orderResponse = await _client.Orders().GetOrderByIdAsync(deletedOrderId);
            Assert.IsFalse(orderResponse.Success);

            await _client.Carts().DeleteCartAsync(cart);
        }

        /// <summary>
        /// Tests the OrderManager.UpdateOrderAsync method.
        /// </summary>
        /// <see cref="OrderManager.UpdateOrderAsync(commercetools.Orders.Order, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdateOrderAsync()
        {
            OrderState newOrderState = OrderState.Confirmed;
            string newOrderNumber = Helper.GetRandomNumber(10000, 99999).ToString();

            ChangeOrderStateAction changeOrderStateAction = new ChangeOrderStateAction(newOrderState);

            GenericAction setOrderNumberAction = new GenericAction("setOrderNumber");
            setOrderNumberAction.SetProperty("orderNumber", newOrderNumber);

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(changeOrderStateAction);
            actions.Add(setOrderNumberAction);

            Response<Order> response = await _client.Orders().UpdateOrderAsync(_testOrder, actions);
            Assert.IsTrue(response.Success);

            _testOrder = response.Result;
            Assert.NotNull(_testOrder.Id);
            Assert.AreEqual(_testOrder.OrderState, newOrderState);
            Assert.AreEqual(_testOrder.OrderNumber, newOrderNumber);
        }
    }
}