using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Carts;
using commercetools.Carts.UpdateActions;
using commercetools.Customers;
using commercetools.Messages;
using commercetools.Payments;
using commercetools.Products;
using commercetools.ProductTypes;
using commercetools.Project;
using commercetools.ShippingMethods;
using commercetools.TaxCategories;
using commercetools.Zones;

using NUnit.Framework;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the CartManager class, along with some of the cart update actions.
    /// </summary>
    [TestFixture]
    public class CartManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Cart> _testCarts;
        private List<Customer> _testCustomers;
        private Payment _testPayment;
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

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            _testCustomers = new List<Customer>();
            _testCarts = new List<Cart>();

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

                CartDraft cartDraft = Helper.GetTestCartDraft(_project, customerCreatedMessage.Customer.Id);
                Task<Response<Cart>> cartTask = _client.Carts().CreateCartAsync(cartDraft);
                cartTask.Wait();
                Assert.IsTrue(cartTask.Result.Success);
                Cart cart = cartTask.Result.Result;
                Assert.NotNull(cart.Id);

                _testCarts.Add(cart);
            }

            ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
            Task<Response<ProductType>> testProductTypeTask = _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
            testProductTypeTask.Wait();
            Assert.IsTrue(testProductTypeTask.Result.Success);
            _testProductType = testProductTypeTask.Result.Result;
            Assert.NotNull(_testProductType.Id);

            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<Response<TaxCategory>> taxCategoryTask = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            taxCategoryTask.Wait();
            Assert.IsTrue(taxCategoryTask.Result.Success);
            _testTaxCategory = taxCategoryTask.Result.Result;
            Assert.NotNull(_testTaxCategory.Id);

            Task<Response<ShippingMethodQueryResult>> shippingMethodQueryResultTask =
                _client.ShippingMethods().QueryShippingMethodsAsync();
            shippingMethodQueryResultTask.Wait();
            Assert.IsTrue(shippingMethodQueryResultTask.Result.Success);

            if (shippingMethodQueryResultTask.Result.Result.Results.Count > 0)
            {
                _testShippingMethod = shippingMethodQueryResultTask.Result.Result.Results[0];
            }
            else
            {
                ZoneDraft zoneDraft = Helper.GetTestZoneDraft();
                Task<Response<Zone>> zoneTask = _client.Zones().CreateZoneAsync(zoneDraft);
                zoneTask.Wait();
                Assert.IsTrue(zoneTask.Result.Success);
                _testZone = zoneTask.Result.Result;

                Assert.NotNull(_testZone.Id);

                ShippingMethodDraft shippingMethodDraft = Helper.GetTestShippingMethodDraft(_project, _testTaxCategory, _testZone);
                Task<Response<ShippingMethod>> shippingMethodTask = _client.ShippingMethods().CreateShippingMethodAsync(shippingMethodDraft);
                shippingMethodTask.Wait();
                Assert.IsTrue(shippingMethodTask.Result.Success);
                _testShippingMethod = shippingMethodTask.Result.Result;
            }

            Assert.NotNull(_testShippingMethod.Id);

            ProductDraft productDraft = Helper.GetTestProductDraft(_project, _testProductType.Id, _testTaxCategory.Id);
            Task<Response<Product>> testProductTask = _client.Products().CreateProductAsync(productDraft);
            testProductTask.Wait();
            Assert.IsTrue(testProductTask.Result.Success);
            _testProduct = testProductTask.Result.Result;

            Assert.NotNull(_testProduct.Id);

            PaymentDraft paymentDraft = Helper.GetTestPaymentDraft(_project, _testCustomers[0].Id);
            Task<Response<Payment>> paymentTask = _client.Payments().CreatePaymentAsync(paymentDraft);
            paymentTask.Wait();
            Assert.IsTrue(paymentTask.Result.Success);
            _testPayment = paymentTask.Result.Result;

            Assert.NotNull(_testPayment.Id);
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            Task task;

            foreach (Cart cart in _testCarts)
            {
                task = _client.Carts().DeleteCartAsync(cart);
                task.Wait();
            }

            task = _client.Payments().DeletePaymentAsync(_testPayment);
            task.Wait();

            foreach (Customer customer in _testCustomers)
            {
                task = _client.Customers().DeleteCustomerAsync(customer);
                task.Wait();
            }

            task = _client.Products().DeleteProductAsync(_testProduct);
            task.Wait();

            task = _client.ProductTypes().DeleteProductTypeAsync(_testProductType);
            task.Wait();

            task = _client.TaxCategories().DeleteTaxCategoryAsync(_testTaxCategory);
            task.Wait();

            if (_testZone != null)
            {
                task = _client.Zones().DeleteZoneAsync(_testZone);
                task.Wait();
            }

            task = _client.ShippingMethods().DeleteShippingMethodAsync(_testShippingMethod);
            task.Wait();
        }

        /// <summary>
        /// Tests the CartManager.GetCartByIdAsync method.
        /// </summary>
        /// <see cref="CartManager.GetCartByIdAsync"/>
        [Test]
        public async Task ShouldGetCartByIdAsync()
        {
            Response<Cart> response = await _client.Carts().GetCartByIdAsync(_testCarts[0].Id);
            Assert.IsTrue(response.Success);

            Cart cart = response.Result;
            Assert.NotNull(cart.Id);
            Assert.AreEqual(cart.Id, _testCarts[0].Id);
        }

        /// <summary>
        /// Tests the CartManager.GetCartByCustomerIdAsync method.
        /// </summary>
        /// <see cref="CartManager.GetCartByCustomerIdAsync"/>
        [Test]
        public async Task ShouldGetCartByCustomerIdAsync()
        {
            Response<Cart> response = await _client.Carts().GetCartByCustomerIdAsync(_testCustomers[0].Id);
            Assert.IsTrue(response.Success);

            Cart cart = response.Result;
            Assert.NotNull(cart.Id);
            Assert.AreEqual(cart.CustomerId, _testCustomers[0].Id);
        }

        /// <summary>
        /// Tests the CartManager.QueryCartsAsync method.
        /// </summary>
        /// <see cref="CartManager.QueryCartsAsync"/>
        [Test]
        public async Task ShouldQueryCartsAsync()
        {
            Response<CartQueryResult> response = await _client.Carts().QueryCartsAsync();
            Assert.IsTrue(response.Success);

            CartQueryResult cartQueryResult = response.Result;
            Assert.NotNull(cartQueryResult.Results);

            int limit = 2;
            response = await _client.Carts().QueryCartsAsync(limit: limit);
            Assert.IsTrue(response.Success);

            cartQueryResult = response.Result;
            Assert.NotNull(cartQueryResult.Results);
            Assert.LessOrEqual(cartQueryResult.Results.Count, limit);
        }

        /// <summary>
        /// Tests the CartManager.CreateCartAsync and CartManager.DeleteCartAsync methods.
        /// </summary>
        /// <see cref="CartManager.CreateCartAsync"/>
        /// <seealso cref="CartManager.DeleteCartAsync(commercetools.Carts.Cart)"/>
        [Test]
        public async Task ShouldCreateAndDeleteCartAsync()
        {
            CartDraft cartDraft = Helper.GetTestCartDraft(_project);

            Response<Cart> response = await _client.Carts().CreateCartAsync(cartDraft);
            Assert.IsTrue(response.Success);

            Cart cart = response.Result;
            Assert.NotNull(cart.Id);
            Assert.AreEqual(cart.Country, cartDraft.Country);
            Assert.AreEqual(cart.InventoryMode, cartDraft.InventoryMode);
            Assert.AreEqual(cart.ShippingAddress, cartDraft.ShippingAddress);
            Assert.AreEqual(cart.BillingAddress, cartDraft.BillingAddress);

            string deletedCartId = cart.Id;

            response = await _client.Carts().DeleteCartAsync(cart);
            Assert.IsTrue(response.Success);

            cart = response.Result;

            response = await _client.Carts().GetCartByIdAsync(deletedCartId);
            Assert.IsFalse(response.Success);
        }

        /// <summary>
        /// Tests the AddLineItemAction, ChangeLineItemQuantityAction and RemoveLineItemAction update actions.
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldAddChangeAndRemoveLineItemAsync()
        {
            int quantity = 2;
            int newQuantity = 3;

            AddLineItemAction addLineItemAction =
                new AddLineItemAction(_testProduct.Id, _testProduct.MasterData.Current.MasterVariant.Id, quantity);
            Response<Cart> response = await _client.Carts().UpdateCartAsync(_testCarts[0], addLineItemAction);
            Assert.IsTrue(response.Success);

            _testCarts[0] = response.Result;
            Assert.NotNull(_testCarts[0].Id);
            Assert.NotNull(_testCarts[0].LineItems);
            Assert.AreEqual(_testCarts[0].LineItems.Count, 1);
            Assert.AreEqual(_testCarts[0].LineItems[0].ProductId, _testProduct.Id);
            Assert.AreEqual(_testCarts[0].LineItems[0].Variant.Id, _testProduct.MasterData.Current.MasterVariant.Id);
            Assert.AreEqual(_testCarts[0].LineItems[0].Quantity, quantity);

            ChangeLineItemQuantityAction changeLineItemQuantityAction =
                new ChangeLineItemQuantityAction(_testCarts[0].LineItems[0].Id, newQuantity);
            response = await _client.Carts().UpdateCartAsync(_testCarts[0], changeLineItemQuantityAction);
            Assert.IsTrue(response.Success);

            _testCarts[0] = response.Result;
            Assert.NotNull(_testCarts[0].Id);
            Assert.NotNull(_testCarts[0].LineItems);
            Assert.AreEqual(_testCarts[0].LineItems.Count, 1);
            Assert.AreEqual(_testCarts[0].LineItems[0].Quantity, newQuantity);

            RemoveLineItemAction removeLineItemAction = new RemoveLineItemAction(_testCarts[0].LineItems[0].Id);
            response = await _client.Carts().UpdateCartAsync(_testCarts[0], removeLineItemAction);
            Assert.IsTrue(response.Success);

            _testCarts[0] = response.Result;
            Assert.NotNull(_testCarts[0].Id);
            Assert.NotNull(_testCarts[0].LineItems);
            Assert.AreEqual(_testCarts[0].LineItems.Count, 0);
        }

        /// <summary>
        /// Tests the SetShippingAddressAction update action.
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldSetShippingAddressAsync()
        {
            Address newShippingAddress = Helper.GetTestAddress(_project);

            newShippingAddress.FirstName = "New";
            newShippingAddress.LastName = "Shipping";
            newShippingAddress.StreetName = "First Ave.";
            newShippingAddress.StreetNumber = "321";
            newShippingAddress.Country = "US";
            newShippingAddress.PostalCode = Helper.GetRandomNumber(10000, 90000).ToString();

            SetShippingAddressAction setShippingAddressAction = new SetShippingAddressAction
            {
                Address = newShippingAddress
            };
            Response<Cart> response = await _client.Carts().UpdateCartAsync(_testCarts[1], setShippingAddressAction);
            Assert.IsTrue(response.Success);

            _testCarts[1] = response.Result;
            Assert.NotNull(_testCarts[1].Id);
            Assert.NotNull(_testCarts[1].ShippingAddress);
            Assert.AreEqual(_testCarts[1].ShippingAddress.FirstName, newShippingAddress.FirstName);
            Assert.AreEqual(_testCarts[1].ShippingAddress.LastName, newShippingAddress.LastName);
            Assert.AreEqual(_testCarts[1].ShippingAddress.StreetName, newShippingAddress.StreetName);
            Assert.AreEqual(_testCarts[1].ShippingAddress.StreetNumber, newShippingAddress.StreetNumber);
            Assert.AreEqual(_testCarts[1].ShippingAddress.Country, newShippingAddress.Country);
            Assert.AreEqual(_testCarts[1].ShippingAddress.PostalCode, newShippingAddress.PostalCode);
        }

        /// <summary>
        /// Tests the SetBillingAddressAction update action.
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldSetBillingAddressAsync()
        {
            Address newBillingAddress = Helper.GetTestAddress(_project);

            newBillingAddress.FirstName = "New";
            newBillingAddress.LastName = "Billing";
            newBillingAddress.StreetName = "Second Ave.";
            newBillingAddress.StreetNumber = "125";
            newBillingAddress.Country = "US";
            newBillingAddress.PostalCode = Helper.GetRandomNumber(10000, 90000).ToString();

            SetBillingAddressAction setBillingAddressAction = new SetBillingAddressAction { Address = newBillingAddress };
            Response<Cart> response = await _client.Carts().UpdateCartAsync(_testCarts[2], setBillingAddressAction);
            Assert.IsTrue(response.Success);

            _testCarts[2] = response.Result;
            Assert.NotNull(_testCarts[2].Id);
            Assert.NotNull(_testCarts[2].BillingAddress);
            Assert.AreEqual(_testCarts[2].BillingAddress.FirstName, newBillingAddress.FirstName);
            Assert.AreEqual(_testCarts[2].BillingAddress.LastName, newBillingAddress.LastName);
            Assert.AreEqual(_testCarts[2].BillingAddress.StreetName, newBillingAddress.StreetName);
            Assert.AreEqual(_testCarts[2].BillingAddress.StreetNumber, newBillingAddress.StreetNumber);
            Assert.AreEqual(_testCarts[2].BillingAddress.Country, newBillingAddress.Country);
            Assert.AreEqual(_testCarts[2].BillingAddress.PostalCode, newBillingAddress.PostalCode);
        }

        /// <summary>
        /// Tests the SetShippingMethodAction update action.
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldSetShippingMethodAsync()
        {
            Reference shippingMethod = new Reference();
            shippingMethod.Id = _testShippingMethod.Id;
            shippingMethod.ReferenceType = Common.ReferenceType.ShippingMethod;

            SetShippingMethodAction setShippingMethodAction = new SetShippingMethodAction { ShippingMethod = shippingMethod };
            Response<Cart> response = await _client.Carts().UpdateCartAsync(_testCarts[3], setShippingMethodAction);
            Assert.IsTrue(response.Success);

            _testCarts[3] = response.Result;
            Assert.NotNull(_testCarts[3].Id);
            Assert.NotNull(_testCarts[3].ShippingInfo);
            Assert.NotNull(_testCarts[3].ShippingInfo.ShippingMethod);
            Assert.AreEqual(_testCarts[3].ShippingInfo.ShippingMethod.Id, _testShippingMethod.Id);
        }

        /// <summary>
        /// Tests the SetCustomerIdAction update action.
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldSetCustomerIdAsync()
        {
            CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
            Response<CustomerCreatedMessage> customerResponse = await _client.Customers().CreateCustomerAsync(customerDraft);
            Assert.IsTrue(customerResponse.Success);

            CustomerCreatedMessage customerCreatedMessage = customerResponse.Result;
            Assert.NotNull(customerCreatedMessage.Customer);

            Customer customer = customerCreatedMessage.Customer;

            SetCustomerIdAction setCustomerIdAction = new SetCustomerIdAction { CustomerId = customer.Id };
            Response<Cart> cartResponse = await _client.Carts().UpdateCartAsync(_testCarts[0], setCustomerIdAction);
            Assert.IsTrue(cartResponse.Success);

            _testCarts[0] = cartResponse.Result;
            Assert.NotNull(_testCarts[0].Id);
            Assert.AreEqual(_testCarts[0].CustomerId, customer.Id);

            setCustomerIdAction = new SetCustomerIdAction();
            cartResponse = await _client.Carts().UpdateCartAsync(_testCarts[0], setCustomerIdAction);
            Assert.IsTrue(cartResponse.Success);

            _testCarts[0] = cartResponse.Result;
            Assert.NotNull(_testCarts[0].Id);
            Assert.AreNotEqual(_testCarts[0].CustomerId, customer.Id);

            await _client.Customers().DeleteCustomerAsync(customer);
        }

        /// <summary>
        /// Tests the RecalculateAction update action.
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldRecalculateAsync()
        {
            RecalculateAction recalculateAction = new RecalculateAction();
            Response<Cart> response = await _client.Carts().UpdateCartAsync(_testCarts[0], recalculateAction);
            Assert.IsTrue(response.Success);

            _testCarts[0] = response.Result;
            Assert.NotNull(_testCarts[0].Id);
        }

        /// <summary>
        /// Tests the AddPaymentAction and RemovePaymentAction update actions.
        /// </summary>
        /// <returns>Task</returns>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldAddAndRemovePaymentAsync()
        {
            Reference paymentReference = new Reference();
            paymentReference.Id = _testPayment.Id;
            paymentReference.ReferenceType = Common.ReferenceType.Payment;

            AddPaymentAction addPaymentAction = new AddPaymentAction(paymentReference);
            Response<Cart> response = await _client.Carts().UpdateCartAsync(_testCarts[4], addPaymentAction);
            Assert.IsTrue(response.Success);

            _testCarts[4] = response.Result;
            Assert.NotNull(_testCarts[4].Id);
            Assert.AreEqual(_testCarts[4].PaymentInfo.Payments.Count, 1);
            Assert.AreEqual(_testCarts[4].PaymentInfo.Payments[0].Id, _testPayment.Id);

            RemovePaymentAction removePaymentAction = new RemovePaymentAction(paymentReference);
            response = await _client.Carts().UpdateCartAsync(_testCarts[4], removePaymentAction);
            Assert.IsTrue(response.Success);

            _testCarts[4] = response.Result;
            Assert.NotNull(_testCarts[4].Id);
            Assert.Null(_testCarts[4].PaymentInfo.Payments);
        }

        /// <summary>
        /// Tests the CartManager.UpdateCartAsync method..
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdateCartAsync()
        {
            CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
            Response<CustomerCreatedMessage> messageResponse = await _client.Customers().CreateCustomerAsync(customerDraft);
            Assert.IsTrue(messageResponse.Success);

            CustomerCreatedMessage customerCreatedMessage = messageResponse.Result;
            Assert.NotNull(customerCreatedMessage.Customer);

            Customer customer = customerCreatedMessage.Customer;
            Assert.NotNull(customer.Id);

            SetCustomerIdAction setCustomerIdAction = new SetCustomerIdAction();
            setCustomerIdAction.CustomerId = customer.Id;

            GenericAction recalculateAction = new GenericAction("recalculate");

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(setCustomerIdAction);
            actions.Add(recalculateAction);

            Response<Cart> cartResponse = await _client.Carts().UpdateCartAsync(_testCarts[0], actions);
            Assert.IsTrue(cartResponse.Success);

            _testCarts[0] = cartResponse.Result;
            Assert.NotNull(_testCarts[0].Id);
            Assert.AreEqual(_testCarts[0].CustomerId, customer.Id);

            await _client.Customers().DeleteCustomerAsync(customer.Id, customer.Version);
        }
    }
}
