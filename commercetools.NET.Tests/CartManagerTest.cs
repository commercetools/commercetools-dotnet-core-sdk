using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.CartDiscounts;
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
using commercetools.Types;
using commercetools.Zones;
using commercetools.Zones.UpdateActions;
using FluentAssertions;
using NUnit.Framework;

using Newtonsoft.Json.Linq;

using Type = commercetools.Types.Type;

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
        private Type _testType;
        private bool _createdTestZone;

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

            Assert.IsTrue(_project.Languages.Count > 0, "No Languages");
            Assert.IsTrue(_project.Currencies.Count > 0, "No Currencies");

            _testCustomers = new List<Customer>();
            _testCarts = new List<Cart>();
            CustomerDraft customerDraft;
            Task<Response<CustomerCreatedMessage>> customerTask;
            CustomerCreatedMessage customerCreatedMessage;
            CartDraft cartDraft;
            Cart cart;
            Task<Response<Cart>> cartTask;
            for (int i = 0; i < 5; i++)
            {
                customerDraft = Helper.GetTestCustomerDraft();
                customerTask = _client.Customers().CreateCustomerAsync(customerDraft);
                customerTask.Wait();
                Assert.IsTrue(customerTask.Result.Success);

                customerCreatedMessage = customerTask.Result.Result;
                Assert.NotNull(customerCreatedMessage.Customer);
                Assert.NotNull(customerCreatedMessage.Customer.Id);

                _testCustomers.Add(customerCreatedMessage.Customer);

                cartDraft = Helper.GetTestCartDraft(_project, customerCreatedMessage.Customer.Id);
                cartTask = _client.Carts().CreateCartAsync(cartDraft);
                cartTask.Wait();
                Assert.NotNull(cartTask.Result);
                Assert.IsTrue(cartTask.Result.Success, "CreateCartAsync failed");
                cart = cartTask.Result.Result;
                Assert.NotNull(cart.Id);
                Console.Error.WriteLine(string.Format("CartManagerTest - Information Only - Init TestCartDraft TaxMode: {0}", cartDraft.TaxMode == null ? "(default)" : cart.TaxMode.ToString()));

                _testCarts.Add(cart);
            }

            //customer/cart with external tax mode enabled

            customerDraft = Helper.GetTestCustomerDraft();
            customerTask = _client.Customers().CreateCustomerAsync(customerDraft);
            customerTask.Wait();
            Assert.IsTrue(customerTask.Result.Success);
            customerCreatedMessage = customerTask.Result.Result;
            Assert.NotNull(customerCreatedMessage.Customer);
            Assert.NotNull(customerCreatedMessage.Customer.Id);

            _testCustomers.Add(customerCreatedMessage.Customer);

            cartDraft = Helper.GetTestCartDraftUsingExternalTaxMode(_project, customerCreatedMessage.Customer.Id);
            cartTask = _client.Carts().CreateCartAsync(cartDraft);
            cartTask.Wait();
            Assert.NotNull(cartTask.Result);
            Assert.IsTrue(cartTask.Result.Success, "CreateCartAsync failed");
            cart = cartTask.Result.Result;
            Assert.NotNull(cart.Id);
            Console.Error.WriteLine(string.Format("CartManagerTest - Information Only - Init TestCartDraft TaxMode: {0}", cart.TaxMode));

            _testCarts.Add(cart);



            ProductTypeDraft productTypeDraft = Helper.GetTestProductTypeDraft();
            Task<Response<ProductType>> testProductTypeTask =
                _client.ProductTypes().CreateProductTypeAsync(productTypeDraft);
            testProductTypeTask.Wait();
            Assert.IsTrue(testProductTypeTask.Result.Success, "CreateProductType failed");
            _testProductType = testProductTypeTask.Result.Result;
            Assert.NotNull(_testProductType.Id);

            TaxCategoryDraft taxCategoryDraft = Helper.GetTestTaxCategoryDraft(_project);
            Task<Response<TaxCategory>> taxCategoryTask =
                _client.TaxCategories().CreateTaxCategoryAsync(taxCategoryDraft);
            taxCategoryTask.Wait();
            Assert.IsTrue(taxCategoryTask.Result.Success, "CreateTaxCategory failed");
            _testTaxCategory = taxCategoryTask.Result.Result;
            Assert.NotNull(_testTaxCategory.Id);

            Task<Response<ZoneQueryResult>> zoneQueryResultTask = _client.Zones().QueryZonesAsync();
            zoneQueryResultTask.Wait();
            Assert.IsTrue(zoneQueryResultTask.Result.Success);

            if (zoneQueryResultTask.Result.Result.Results.Count > 0)
            {
                _testZone = zoneQueryResultTask.Result.Result.Results[0];
                _createdTestZone = false;
            }
            else
            {
                ZoneDraft zoneDraft = Helper.GetTestZoneDraft();
                Task<Response<Zone>> zoneTask = _client.Zones().CreateZoneAsync(zoneDraft);
                zoneTask.Wait();
                Assert.IsTrue(zoneTask.Result.Success, "CreateZone failed");
                _testZone = zoneTask.Result.Result;
                _createdTestZone = true;
            }

            Assert.NotNull(_testZone.Id);

            foreach (string country in _project.Countries)
            {
                Location location =
                    _testZone.Locations
                        .Where(l => l.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                if (location == null)
                {
                    location = new Location();
                    location.Country = country;

                    AddLocationAction addLocationAction = new AddLocationAction(location);
                    Task<Response<Zone>> updateZoneTask = _client.Zones().UpdateZoneAsync(_testZone, addLocationAction);
                    updateZoneTask.Wait();
                    Assert.IsTrue(updateZoneTask.Result.Success, "UpdateZone failed");
                    _testZone = updateZoneTask.Result.Result;
                }
            }

            Assert.NotNull(_testZone.Locations.Count > 0);

            ShippingMethodDraft shippingMethodDraft =
                Helper.GetTestShippingMethodDraft(_project, _testTaxCategory, _testZone);
            Task<Response<ShippingMethod>> shippingMethodTask =
                _client.ShippingMethods().CreateShippingMethodAsync(shippingMethodDraft);
            shippingMethodTask.Wait();
            Assert.IsTrue(shippingMethodTask.Result.Success, "CreateShippingMethod failed");
            _testShippingMethod = shippingMethodTask.Result.Result;

            Assert.NotNull(_testShippingMethod.Id);

            ProductDraft productDraft = Helper.GetTestProductDraft(_project, _testProductType.Id, _testTaxCategory.Id);
            Task<Response<Product>> testProductTask = _client.Products().CreateProductAsync(productDraft);
            testProductTask.Wait();
            Assert.IsTrue(testProductTask.Result.Success, "CreateProduct failed");
            _testProduct = testProductTask.Result.Result;

            Assert.NotNull(_testProduct.Id);

            PaymentDraft paymentDraft = Helper.GetTestPaymentDraft(_project, _testCustomers[0].Id);
            Task<Response<Payment>> paymentTask = _client.Payments().CreatePaymentAsync(paymentDraft);
            paymentTask.Wait();
            Assert.IsTrue(paymentTask.Result.Success, "CreatePayment failed");
            _testPayment = paymentTask.Result.Result;

            Assert.NotNull(_testPayment.Id);

            TypeDraft typeDraft = Helper.GetTypeDraft(_project);
            Task<Response<Type>> typeTask = _client.Types().CreateTypeAsync(typeDraft);
            typeTask.Wait();
            Assert.IsTrue(typeTask.Result.Success, "CreateType failed");
            _testType = typeTask.Result.Result;
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

            if (_testPayment != null)
            {
                task = _client.Payments().DeletePaymentAsync(_testPayment);
                task.Wait();
            }

            foreach (Customer customer in _testCustomers)
            {
                task = _client.Customers().DeleteCustomerAsync(customer);
                task.Wait();
            }

            if (_testProduct != null)
            {
                task = _client.Products().DeleteProductAsync(_testProduct);
                task.Wait();
            }

            if (_testProductType != null)
            {
                task = _client.ProductTypes().DeleteProductTypeAsync(_testProductType);
                task.Wait();
            }

            if (_testShippingMethod != null)
            {
                task = _client.ShippingMethods().DeleteShippingMethodAsync(_testShippingMethod);
                task.Wait();
            }

            if (_testTaxCategory != null)
            {
                task = _client.TaxCategories().DeleteTaxCategoryAsync(_testTaxCategory);
                task.Wait();
            }

            if (_createdTestZone)
            {
                task = _client.Zones().DeleteZoneAsync(_testZone);
                task.Wait();
            }

            if (_testType != null)
            {
                task = _client.Types().DeleteTypeAsync(_testType);
                task.Wait();
            }
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
            CartDraft cartDraft = Helper.GetTestCartDraft(_project, null);

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
        /// Tests the CartManager.CreateCartAsync and CartManager.DeleteCartAsync methods for a cart in external tax mode with custom line items having an external tax rate.
        /// </summary>
        /// <see cref="CartManager.CreateCartAsync"/>
        /// <seealso cref="CartManager.DeleteCartAsync(commercetools.Carts.Cart)"/>
        [Test]
        public async Task ShouldCreateAndDeleteCartInExternalTaxModeWithCustomLineItemsHavingExternalTaxRate()
        {
            CartDraft cartDraft = Helper.GetTestCartDraftWithCustomLineItemsUsingExternalTaxMode(_project);
            Response<Cart> response = await _client.Carts().CreateCartAsync(cartDraft);
            Assert.IsTrue(response.Success);

            Cart cart = response.Result;
            Assert.NotNull(cart.Id);
            Assert.NotNull(cart.CustomLineItems);
            Assert.IsTrue(cart.CustomLineItems.Count > 0);
            Assert.NotNull(cart.CustomLineItems[0].TaxRate);
            Assert.NotNull(cart.CustomLineItems[0].TaxRate.Name);
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
        /// Tests the CartManager.CreateCartAsync and CartManager.DeleteCartAsync methods for a cart with custom line items.
        /// </summary>
        /// <see cref="CartManager.CreateCartAsync"/>
        /// <seealso cref="CartManager.DeleteCartAsync(commercetools.Carts.Cart)"/>
        [Test]
        public async Task ShouldCreateAndDeleteCartWithCustomLineItemsAsync()
        {
            CartDraft cartDraft = Helper.GetTestCartDraftWithCustomLineItems(_project);
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

            for(int i = 4; i < _testCarts.Count; i++)
            {
                var cart = _testCarts[i];
                AddLineItemAction addLineItemAction = new AddLineItemAction(_testProduct.Id, _testProduct.MasterData.Current.MasterVariant.Id);
                addLineItemAction.Quantity = quantity;

                if (cart.TaxMode != null && cart.TaxMode == TaxMode.External)
                {
                    addLineItemAction.ExternalTaxRate = new ExternalTaxRateDraft("TestTaxRate", _project.Countries[0]) { Amount = 0.1m };
                    addLineItemAction.ExternalPrice = new Money() { CentAmount = 5, CurrencyCode = _project.Currencies[0] };
                }

                Response<Cart> response = await _client.Carts().UpdateCartAsync(cart, addLineItemAction);
                Assert.IsTrue(response.Success);

                cart = response.Result;
                Assert.NotNull(cart.Id);
                Assert.NotNull(cart.LineItems, "LineItems are null");
                Assert.AreEqual(cart.LineItems.Count, 1);
                Assert.AreEqual(cart.LineItems[0].ProductId, _testProduct.Id);
                Assert.AreEqual(cart.LineItems[0].Variant.Id, _testProduct.MasterData.Current.MasterVariant.Id);
                Assert.AreEqual(cart.LineItems[0].Quantity, quantity);
                Assert.NotNull(cart.LineItems[0].TaxRate, "TaxRate is null");

                if (cart.TaxMode != null && cart.TaxMode == TaxMode.External)
                {
                    Assert.AreEqual(cart.LineItems[0].TaxRate.Name, addLineItemAction.ExternalTaxRate.Name);
                    Assert.AreEqual(cart.LineItems[0].TaxRate.Country, addLineItemAction.ExternalTaxRate.Country);
                    Assert.AreEqual(cart.LineItems[0].TaxRate.Amount, addLineItemAction.ExternalTaxRate.Amount);
                    Assert.AreEqual(cart.LineItems[0].Price.Value.CentAmount, addLineItemAction.ExternalPrice.CentAmount);
                    Assert.AreEqual(cart.LineItems[0].Price.Value.CurrencyCode, addLineItemAction.ExternalPrice.CurrencyCode);
                }
                ChangeLineItemQuantityAction changeLineItemQuantityAction =
                   new ChangeLineItemQuantityAction(cart.LineItems[0].Id, newQuantity);
                if (cart.TaxMode != null && cart.TaxMode == TaxMode.External)
                {
                    changeLineItemQuantityAction.ExternalPrice = new Money() { CentAmount = 10, CurrencyCode = _project.Currencies[0] };
                }

                response = await _client.Carts().UpdateCartAsync(cart, changeLineItemQuantityAction);
                Assert.IsTrue(response.Success, string.Format("TaxMode: {0}", cart.TaxMode));
                Console.Error.WriteLine(string.Format("CartManagerTest - Information Only - CartId: {0} TaxMode: {1}", cart.Id, cart.TaxMode));
                cart = response.Result;
                Assert.NotNull(cart.Id);
                Assert.NotNull(cart.LineItems);
                Assert.AreEqual(cart.LineItems.Count, 1);
                Assert.AreEqual(cart.LineItems[0].Quantity, newQuantity);
                RemoveLineItemAction removeLineItemAction = new RemoveLineItemAction(cart.LineItems[0].Id);
                response = await _client.Carts().UpdateCartAsync(cart, removeLineItemAction);
                Assert.IsTrue(response.Success);

                cart = response.Result;
                Assert.NotNull(cart.Id);
                Assert.NotNull(cart.LineItems);
                Assert.AreEqual(cart.LineItems.Count, 0);
            }
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

            SetBillingAddressAction setBillingAddressAction = new SetBillingAddressAction {Address = newBillingAddress};
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
        /// Tests the SetDeleteDaysAfterLastModification update action.
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldSetDeleteDaysAfterLastModificationAsync()
        {
            SetDeleteDaysAfterLastModificationAction setDeleteDays = new SetDeleteDaysAfterLastModificationAction(1);
            Response<Cart> response = await _client.Carts().UpdateCartAsync(_testCarts[2], setDeleteDays);
            Assert.IsTrue(response.Success);
            _testCarts[2] = response.Result;
            Assert.NotNull(_testCarts[2].DeleteDaysAfterLastModification);
            Assert.IsTrue(_testCarts[2].DeleteDaysAfterLastModification == 1);
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

            SetShippingMethodAction setShippingMethodAction =
                new SetShippingMethodAction {ShippingMethod = shippingMethod};
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
            Response<CustomerCreatedMessage> customerResponse =
                await _client.Customers().CreateCustomerAsync(customerDraft);
            Assert.IsTrue(customerResponse.Success);

            CustomerCreatedMessage customerCreatedMessage = customerResponse.Result;
            Assert.NotNull(customerCreatedMessage.Customer);

            Customer customer = customerCreatedMessage.Customer;

            SetCustomerIdAction setCustomerIdAction = new SetCustomerIdAction {CustomerId = customer.Id};
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
        /// Tests the SetCustomTypeAction update action.
        /// </summary>
        /// <see cref="CartManager.UpdateCartAsync(commercetools.Carts.Cart, commercetools.Common.UpdateAction)"/>
        [Test]
        public async Task ShouldSetCustomTypeAsync()
        {
            ResourceIdentifier typeResourceIdentifier = new ResourceIdentifier
            {
                Id = _testType.Id,
                TypeId = commercetools.Common.ReferenceType.Type
            };

            string fieldName = _testType.FieldDefinitions[0].Name;

            JObject fields = new JObject();
            fields.Add(fieldName, "Here is the value of my field.");

            SetCustomTypeAction setCustomTypeAction = new SetCustomTypeAction
            {
                Type = typeResourceIdentifier,
                Fields = fields
            };

            Response<Cart> cartResponse = await _client.Carts().UpdateCartAsync(_testCarts[1], setCustomTypeAction);
            Assert.IsTrue(cartResponse.Success);
            _testCarts[1] = cartResponse.Result;

            Assert.NotNull(_testCarts[1].Custom.Fields);
            Assert.AreEqual(fields[fieldName], _testCarts[1].Custom.Fields[fieldName]);
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
            Response<CustomerCreatedMessage> messageResponse =
                await _client.Customers().CreateCustomerAsync(customerDraft);
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

        [Test]
        public async Task ShouldApplyDiscountOnCartWithCustomLineItemsAsync()
        {
            // Arrange
            CartDiscount cartDiscount = await Helper.CreateCartDiscountForCustomLineItems(this._project, this._client);
            CartDraft cartDraft = Helper.GetTestCartDraftWithCustomLineItems(_project);

            // Act
            Response<Cart> response = await _client.Carts().CreateCartAsync(cartDraft);

            // Assert
            response.Result.CustomLineItems.Count.Should().Be(1);
            var customLineItem = response.Result.CustomLineItems.First();
            customLineItem.DiscountedPricePerQuantity.Count.Should().BeGreaterThan(0);
            var discountedLineItemPrice = customLineItem.DiscountedPricePerQuantity.First();
            discountedLineItemPrice.DiscountedPrice.Value.CentAmount.Should().BeGreaterThan(1);
            discountedLineItemPrice.DiscountedPrice.Value.CurrencyCode.Should().Be("EUR");
            Assert.NotNull(discountedLineItemPrice);
            Assert.NotNull(discountedLineItemPrice.DiscountedPrice);
            Assert.NotNull(discountedLineItemPrice.DiscountedPrice.Value);
            Assert.NotNull(discountedLineItemPrice.DiscountedPrice.Value);
            Assert.IsFalse(string.IsNullOrEmpty(discountedLineItemPrice.DiscountedPrice.Value.CurrencyCode));

            // Cleanup
            await this._client.CartDiscounts().DeleteCartDiscountAsync(cartDiscount.Id, 1);
            await this._client.Carts().DeleteCartAsync(response.Result);
        }
    }
}
