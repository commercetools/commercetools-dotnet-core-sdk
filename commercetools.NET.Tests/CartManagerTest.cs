using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Carts;
using commercetools.Customers;
using commercetools.Messages;
using commercetools.Payments;
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
    /// Test the API methods in the CartManager class.
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

            Task<Project.Project> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            _project = projectTask.Result;

            _testCustomers = new List<Customer>();
            _testCarts = new List<Cart>();

            for (int i = 0; i < 5; i++)
            {
                CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
                Task<CustomerCreatedMessage> customerTask = _client.Customers().CreateCustomerAsync(customerDraft);
                customerTask.Wait();
                CustomerCreatedMessage customerCreatedMessage = customerTask.Result;

                Assert.NotNull(customerCreatedMessage.Customer);
                Assert.NotNull(customerCreatedMessage.Customer.Id);

                _testCustomers.Add(customerCreatedMessage.Customer);

                CartDraft cartDraft = Helper.GetTestCartDraft(_project, customerCreatedMessage.Customer.Id);
                Task<Cart> cartTask = _client.Carts().CreateCartAsync(cartDraft);
                cartTask.Wait();
                Cart cart = cartTask.Result;

                Assert.NotNull(cart.Id);

                _testCarts.Add(cart);
            }

            ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
            Task<ProductType> testProductTypeTask = _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
            testProductTypeTask.Wait();
            _testProductType = testProductTypeTask.Result;

            Assert.NotNull(_testProductType.Id);

            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<TaxCategory> taxCategoryTask = _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            taxCategoryTask.Wait();
            _testTaxCategory = taxCategoryTask.Result;

            Assert.NotNull(_testTaxCategory.Id);

            Task<ShippingMethod> shippingMethodTask = Helper.GetShippingMethodForCountry(_client, "US");
            shippingMethodTask.Wait();

            if (shippingMethodTask.Result != null)
            {
                _testShippingMethod = shippingMethodTask.Result;
            }
            else
            {
                ZoneDraft zoneDraft = Helper.GetTestZoneDraft(_project);
                Task<Zone> zoneTask = _client.Zones().CreateZoneAsync(zoneDraft);
                zoneTask.Wait();
                _testZone = zoneTask.Result;

                Assert.NotNull(_testZone.Id);

                ShippingMethodDraft shippingMethodDraft = Helper.GetTestShippingMethodDraft(_project, _testTaxCategory, _testZone);
                shippingMethodTask = _client.ShippingMethods().CreateShippingMethodAsync(shippingMethodDraft);
                shippingMethodTask.Wait();
                _testShippingMethod = shippingMethodTask.Result;
            }

            Assert.NotNull(_testShippingMethod.Id);

            ProductDraft productDraft = Helper.GetTestProductDraft(_project, _testProductType.Id, _testTaxCategory.Id);
            Task<Product> testProductTask = _client.Products().CreateProductAsync(productDraft);
            testProductTask.Wait();
            _testProduct = testProductTask.Result;

            Assert.NotNull(_testProduct.Id);

            PaymentDraft paymentDraft = Helper.GetTestPaymentDraft(_project, _testCustomers[0].Id);
            Task<Payment> paymentTask = _client.Payments().CreatePaymentAsync(paymentDraft);
            paymentTask.Wait();
            _testPayment = paymentTask.Result;

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
            Cart cart = await _client.Carts().GetCartByIdAsync(_testCarts[0].Id);

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
            Cart cart = await _client.Carts().GetCartByCustomerIdAsync(_testCustomers[0].Id);

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
            CartQueryResult result = await _client.Carts().QueryCartsAsync();

            Assert.NotNull(result.Results);

            int limit = 2;
            result = await _client.Carts().QueryCartsAsync(limit: limit);

            Assert.NotNull(result.Results);
            Assert.LessOrEqual(result.Results.Count, limit);
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

            Cart cart = await _client.Carts().CreateCartAsync(cartDraft);

            Assert.NotNull(cart.Id);
            Assert.AreEqual(cart.Country, cartDraft.Country);
            Assert.AreEqual(cart.InventoryMode, cartDraft.InventoryMode);
            Assert.AreEqual(cart.ShippingAddress, cartDraft.ShippingAddress);
            Assert.AreEqual(cart.BillingAddress, cartDraft.BillingAddress);

            string deletedCartId = cart.Id;

            cart = await _client.Carts().DeleteCartAsync(cart);

            AggregateException ex = Assert.Throws<AggregateException>(
                delegate
                {
                    Task<Cart> task = _client.Carts().GetCartByIdAsync(deletedCartId);
                    task.Wait();
                });
        }

        /// <summary>
        /// Tests the CartManager.AddLineItemAsync, CartManager.ChangeLineItemQuantityAsync and CartManager.RemoveLineItemAsync methods.
        /// </summary>
        /// <see cref="CartManager.AddLineItemAsync(commercetools.Carts.Cart, commercetools.Products.Product, commercetools.Products.ProductVariant, int, commercetools.Common.Reference, commercetools.Common.Reference, commercetools.CustomFields.CustomFieldsDraft)"/>
        /// <seealso cref="CartManager.ChangeLineItemQuantityAsync(commercetools.Carts.Cart, string, int)"/>
        /// <seealso cref="CartManager.RemoveLineItemAsync(commercetools.Carts.Cart, string, int)"/>
        [Test]
        public async Task ShouldAddChangeAndRemoveLineItemAsync()
        {
            int quantity = 2;
            int newQuantity = 3;

            _testCarts[0] = await _client.Carts().AddLineItemAsync(_testCarts[0], _testProduct, _testProduct.MasterData.Current.MasterVariant, quantity);

            Assert.NotNull(_testCarts[0].Id);
            Assert.NotNull(_testCarts[0].LineItems);
            Assert.AreEqual(_testCarts[0].LineItems.Count, 1);
            Assert.AreEqual(_testCarts[0].LineItems[0].ProductId, _testProduct.Id);
            Assert.AreEqual(_testCarts[0].LineItems[0].Variant.Id, _testProduct.MasterData.Current.MasterVariant.Id);
            Assert.AreEqual(_testCarts[0].LineItems[0].Quantity, quantity);

            _testCarts[0] = await _client.Carts().ChangeLineItemQuantityAsync(_testCarts[0], _testCarts[0].LineItems[0].Id, newQuantity);

            Assert.NotNull(_testCarts[0].Id);
            Assert.NotNull(_testCarts[0].LineItems);
            Assert.AreEqual(_testCarts[0].LineItems.Count, 1);
            Assert.AreEqual(_testCarts[0].LineItems[0].Quantity, newQuantity);

            _testCarts[0] = await _client.Carts().RemoveLineItemAsync(_testCarts[0], _testCarts[0].LineItems[0].Id);

            Assert.NotNull(_testCarts[0].Id);
            Assert.NotNull(_testCarts[0].LineItems);
            Assert.AreEqual(_testCarts[0].LineItems.Count, 0);
        }

        /// <summary>
        /// Tests the CartManager.SetShippingAddressAsync method.
        /// </summary>
        /// <see cref="CartManager.SetShippingAddressAsync(commercetools.Carts.Cart, commercetools.Common.Address)"/>
        [Test]
        public async Task ShouldSetShippingAddressAsync()
        {
            Address newShippingAddress = Helper.GetTestAddress();

            newShippingAddress.FirstName = "New";
            newShippingAddress.LastName = "Shipping";
            newShippingAddress.StreetName = "First Ave.";
            newShippingAddress.StreetNumber = "321";
            newShippingAddress.Country = "US";
            newShippingAddress.PostalCode = Helper.GetRandomNumber(10000, 90000).ToString();

            _testCarts[1] = await _client.Carts().SetShippingAddressAsync(_testCarts[1], newShippingAddress);

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
        /// Tests the CartManager.SetBillingAddressAsync method.
        /// </summary>
        /// <see cref="CartManager.SetBillingAddressAsync(commercetools.Carts.Cart, commercetools.Common.Address)"/>
        [Test]
        public async Task ShouldSetBillingAddressAsync()
        {
            Address newBillingAddress = Helper.GetTestAddress();

            newBillingAddress.FirstName = "New";
            newBillingAddress.LastName = "Billing";
            newBillingAddress.StreetName = "Second Ave.";
            newBillingAddress.StreetNumber = "125";
            newBillingAddress.Country = "US";
            newBillingAddress.PostalCode = Helper.GetRandomNumber(10000, 90000).ToString();

            _testCarts[2] = await _client.Carts().SetBillingAddressAsync(_testCarts[2], newBillingAddress);

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
        /// Tests the CartManager.SetShippingMethodAsync method.
        /// </summary>
        /// <see cref="CartManager.SetShippingMethodAsync(commercetools.Carts.Cart, commercetools.Common.Reference)"/>
        [Test]
        public async Task ShouldSetShippingMethodAsync()
        {
            Reference shippingMethod = new Reference();
            shippingMethod.Id = _testShippingMethod.Id;
            shippingMethod.ReferenceType = Common.ReferenceType.ShippingMethod;

            _testCarts[3] = await _client.Carts().SetShippingMethodAsync(_testCarts[3], shippingMethod);

            Assert.NotNull(_testCarts[3].Id);
            Assert.NotNull(_testCarts[3].ShippingInfo);
            Assert.NotNull(_testCarts[3].ShippingInfo.ShippingMethod);
            Assert.AreEqual(_testCarts[3].ShippingInfo.ShippingMethod.Id, _testShippingMethod.Id);
        }

        /// <summary>
        /// Tests the CartManager.SetCustomerIdAsync method.
        /// </summary>
        /// <see cref="CartManager.SetCustomerIdAsync(commercetools.Carts.Cart, string)"/>
        [Test]
        public async Task ShouldSetCustomerIdAsync()
        {
            CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
            CustomerCreatedMessage message = await _client.Customers().CreateCustomerAsync(customerDraft);

            Assert.NotNull(message);
            Assert.NotNull(message.Customer);

            Customer customer = message.Customer;

            _testCarts[0] = await _client.Carts().SetCustomerIdAsync(_testCarts[0], customer.Id);

            Assert.NotNull(_testCarts[0].Id);
            Assert.AreEqual(_testCarts[0].CustomerId, customer.Id);

            _testCarts[0] = await _client.Carts().SetCustomerIdAsync(_testCarts[0]);

            Assert.NotNull(_testCarts[0].Id);
            Assert.AreNotEqual(_testCarts[0].CustomerId, customer.Id);

            await _client.Customers().DeleteCustomerAsync(customer);
        }

        /// <summary>
        /// Tests the CartManager.RecalculateAsync method.
        /// </summary>
        /// <see cref="CartManager.RecalculateAsync(commercetools.Carts.Cart)"/>
        [Test]
        public async Task ShouldRecalculateAsync()
        {
            _testCarts[0] = await _client.Carts().RecalculateAsync(_testCarts[0]);

            Assert.NotNull(_testCarts[0].Id);
        }

        /// <summary>
        /// Tests the CartManager.AddPaymentAsync and CartManager.RemovePaymentAsync methods.
        /// </summary>
        /// <returns>Task</returns>
        /// <see cref="CartManager.AddPaymentAsync(commercetools.Carts.Cart, commercetools.Common.Reference)"/>
        /// <seealso cref="CartManager.RemovePaymentAsync(commercetools.Carts.Cart, commercetools.Common.Reference)"/>
        [Test]
        public async Task ShouldAddAndRemovePaymentAsync()
        {
            Reference paymentReference = new Reference();
            paymentReference.Id = _testPayment.Id;
            paymentReference.ReferenceType = Common.ReferenceType.Payment;

            _testCarts[4] = await _client.Carts().AddPaymentAsync(_testCarts[4], paymentReference);

            Assert.NotNull(_testCarts[4].Id);
            Assert.AreEqual(_testCarts[4].PaymentInfo.Payments.Count, 1);
            Assert.AreEqual(_testCarts[4].PaymentInfo.Payments[0].Id, _testPayment.Id);

            _testCarts[4] = await _client.Carts().RemovePaymentAsync(_testCarts[4], paymentReference);

            Assert.NotNull(_testCarts[4].Id);
            Assert.Null(_testCarts[4].PaymentInfo.Payments);
        }

        /// <summary>
        /// Tests the CartManager.UpdateCartAsync method.
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, System.Collections.Generic.List{Newtonsoft.Json.Linq.JObject})"/>
        [Test]
        public async Task ShouldUpdateCartAsync()
        {
            List<JObject> actions = new List<JObject>();

            CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
            CustomerCreatedMessage message = await _client.Customers().CreateCustomerAsync(customerDraft);

            Assert.NotNull(message);
            Assert.NotNull(message.Customer);

            Customer customer = message.Customer;

            actions.Add(
                JObject.FromObject(new
                {
                    action = "setCustomerId",
                    customerId = customer.Id
                })
            );

            actions.Add(
                JObject.FromObject(new
                {
                    action = "recalculate"
                })
            );

            _testCarts[0] = await _client.Carts().UpdateCartAsync(_testCarts[0], actions);

            Assert.NotNull(_testCarts[0].Id);
            Assert.AreEqual(_testCarts[0].CustomerId, customer.Id);

            await _client.Customers().DeleteCustomerAsync(customer.Id, customer.Version);
        }
    }
}