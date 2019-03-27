using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Carts.UpdateActions;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using LineItemDraft = commercetools.Sdk.Domain.Carts.LineItemDraft;
using SetCustomFieldUpdateAction = commercetools.Sdk.Domain.Carts.UpdateActions.SetCustomFieldUpdateAction;
using SetCustomLineItemShippingDetailsUpdateAction = commercetools.Sdk.Domain.Carts.UpdateActions.SetCustomLineItemShippingDetailsUpdateAction;
using SetCustomTypeUpdateAction = commercetools.Sdk.Domain.Carts.UpdateActions.SetCustomTypeUpdateAction;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Carts
{
    [Collection("Integration Tests")]
    public class CartIntegrationTests : IClassFixture<CartFixture>
    {
        private readonly CartFixture cartFixture;

        public CartIntegrationTests(CartFixture cartFixture)
        {
            this.cartFixture = cartFixture;
        }

        [Fact]
        public void CreateCart()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            CartDraft cartDraft = this.cartFixture.GetCartDraft();
            Cart cart = commerceToolsClient
                .ExecuteAsync(new CreateCommand<Cart>(cartDraft)).Result;
            this.cartFixture.CartToDelete.Add(cart);
            Assert.Equal(cartDraft.CustomerId, cart.CustomerId);
        }

        [Fact]
        public void GetCartById()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            this.cartFixture.CartToDelete.Add(cart);
            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<Cart>(new Guid(cart.Id))).Result;
            Assert.Equal(cart.Id, retrievedCart.Id);
        }

        [Fact]
        public void GetCartByCustomerId()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            this.cartFixture.CartToDelete.Add(cart);
            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new GetCartByCustomerIdCommand(new Guid(cart.CustomerId))).Result;
            Assert.Equal(cart.Id, retrievedCart.Id);
        }

        [Fact]
        public async void DeleteCartById()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(
                    new DeleteByIdCommand<Cart>(new Guid(cart.Id), cart.Version))
                .Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<Cart>(new Guid(retrievedCart.Id))));
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void ReplicateCartFromCart()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            ReplicaCartDraft cartReplicationDraft = new ReplicaCartDraft()
            {
                Reference = new Reference<Cart>() {Id = cart.Id, TypeId = ReferenceTypeId.Cart}
            };
            var replicatedCart = commerceToolsClient
                .ExecuteAsync(new ReplicateCartCommand(cartReplicationDraft)).Result;

            Assert.NotNull(replicatedCart);
            Assert.Equal(CartState.Active, replicatedCart.CartState);
        }

        [Fact]
        public void QueryCarts()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            this.cartFixture.CartToDelete.Add(cart);
            QueryCommand<Cart> queryCommand = new QueryCommand<Cart>();
            queryCommand.Where(c => c.Id == cart.Id.valueOf());
            PagedQueryResult<Cart> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, cd => cd.Id == cart.Id);
        }

        #region UpdateActions

        [Fact]
        public void UpdateCartAddLineItemByProductId()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            //Create Product, LineItemDraft and Cart
            Product product = this.cartFixture.CreateProduct();
            LineItemDraft lineItemDraft = this.cartFixture.GetLineItemDraft(product.Id,1, 5);
            Cart cart = this.cartFixture.CreateCart();


            AddLineItemByProductIdUpdateAction addLineItemUpdateAction = new AddLineItemByProductIdUpdateAction()
            {
                LineItem = lineItemDraft,
                ProductId = product.Id,
                Quantity = lineItemDraft.Quantity
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>>();
            updateActions.Add(addLineItemUpdateAction);

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Single(retrievedCart.LineItems);
            Assert.Equal(lineItemDraft.ProductId, retrievedCart.LineItems[0].ProductId);
            Assert.Equal(lineItemDraft.Quantity, retrievedCart.LineItems[0].Quantity);

        }

        [Fact]
        public void UpdateCartAddLineItemBySku()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            //Create Product, LineItemDraft and Cart
            Product product = this.cartFixture.CreateProduct();
            string sku = product.MasterData.Current.MasterVariant.Sku;
            LineItemDraft lineItemDraft = this.cartFixture.GetLineItemDraftBySku(sku, 5);
            Cart cart = this.cartFixture.CreateCart();


            AddLineItemBySkuUpdateAction addLineItemUpdateAction = new AddLineItemBySkuUpdateAction()
            {
                LineItem = lineItemDraft,
                Sku = sku,
                Quantity = lineItemDraft.Quantity
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addLineItemUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Single(retrievedCart.LineItems);
            Assert.Equal(lineItemDraft.Sku, retrievedCart.LineItems[0].Variant.Sku);
            Assert.Equal(lineItemDraft.Quantity, retrievedCart.LineItems[0].Quantity);
        }


        [Fact]
        public void UpdateCartSetCustomerEmail()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            Cart cart = this.cartFixture.CreateCart();

            string email = $"{this.cartFixture.RandomString(6)}@test.com";

            SetCustomerEmailUpdateAction setCustomerEmailUpdateAction = new SetCustomerEmailUpdateAction()
            {
                Email = email
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setCustomerEmailUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(email, retrievedCart.CustomerEmail);
        }

        [Fact]
        public void UpdateCartSetShippingAddress()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            Cart cart = this.cartFixture.CreateCart();

            var shippingAddress = this.cartFixture.GetRandomAddress();
            SetShippingAddressUpdateAction setShippingAddressUpdateAction = new SetShippingAddressUpdateAction()
            {
                Address = shippingAddress
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setShippingAddressUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(shippingAddress.ToString(), retrievedCart.ShippingAddress.ToString());
        }

        [Fact]
        public void UpdateCartSetBillingAddress()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            Cart cart = this.cartFixture.CreateCart();

            var billingAddress = this.cartFixture.GetRandomAddress();

            SetBillingAddressUpdateAction setBillingAddressUpdateAction = new SetBillingAddressUpdateAction()
            {
                Address = billingAddress
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setBillingAddressUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(billingAddress.ToString(), retrievedCart.BillingAddress.ToString());
        }

        [Fact]
        public void UpdateCartSetCountry()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            string country = "EG";

            SetCountryUpdateAction setCountryUpdateAction = new SetCountryUpdateAction()
            {
                Country = country
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setCountryUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(country, retrievedCart.Country);
        }

        [Fact]
        public void UpdateCartSetShippingMethod()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            //create cart with shipping address in random Europe country
            Cart cart = this.cartFixture.CreateCart(withDefaultShippingCountry: false);
            ShippingMethod shippingMethod = this.cartFixture.CreateShippingMethod(cart.ShippingAddress.Country);

            SetShippingMethodUpdateAction setShippingMethodUpdateAction = new SetShippingMethodUpdateAction()
            {
                ShippingMethod = new Reference<ShippingMethod>()
                {
                    Id = shippingMethod.Id,
                    TypeId = ReferenceTypeId.ShippingMethod
                }
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setShippingMethodUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(shippingMethod.Name, retrievedCart.ShippingInfo.ShippingMethodName);
        }

        [Fact]
        public void UpdateCartSetCustomShippingMethod()
        {
            //Arrange
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            int rand = this.cartFixture.RandomInt();
            string customShippingMethod = $"CustomShipping_{rand}";
            var taxCategory = this.cartFixture.CreateNewTaxCategory();
            var shippingRate = this.cartFixture.GetShippingRateDraft();

            SetCustomShippingMethodUpdateAction setCustomShippingMethod = new SetCustomShippingMethodUpdateAction()
            {
                ShippingMethodName = customShippingMethod,
                TaxCategory = new Reference<TaxCategory>() {Id = taxCategory.Id, TypeId = ReferenceTypeId.TaxCategory},
                ShippingRate = shippingRate
            };
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setCustomShippingMethod};

            //Act
            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            //Assert
            Assert.Equal(customShippingMethod, retrievedCart.ShippingInfo.ShippingMethodName);
            Assert.Equal(shippingRate.Price, retrievedCart.ShippingInfo.ShippingRate.Price);
            Assert.Equal(taxCategory.Id, retrievedCart.ShippingInfo.TaxCategory.Id);
        }

        [Fact]
        public void UpdateCartAddDiscountCode()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            string code = this.cartFixture.RandomString(10);
            DiscountCode discountCode = this.cartFixture.CreateDiscountCode(code);

            AddDiscountCodeUpdateAction addDiscountCodeUpdateAction = new AddDiscountCodeUpdateAction()
            {
                Code = code
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addDiscountCodeUpdateAction};

            //expansions
            List<Expansion<Cart>> expansions = new List<Expansion<Cart>>();
            ReferenceExpansion<Cart> expand = new ReferenceExpansion<Cart>(c => c.DiscountCodes.ExpandAll().DiscountCode);
            expansions.Add(expand);


            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions, expansions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Single(retrievedCart.DiscountCodes);
            Assert.Equal(code, retrievedCart.DiscountCodes[0].DiscountCode.Obj.Code);
        }

        [Fact]
        public void UpdateCartRemoveDiscountCode()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            string code = this.cartFixture.RandomString(10);
            DiscountCode discountCode = this.cartFixture.CreateDiscountCode(code);

            //Adding the discount code
            AddDiscountCodeUpdateAction addDiscountCodeUpdateAction = new AddDiscountCodeUpdateAction()
            {
                Code = code
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addDiscountCodeUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            Assert.Single(retrievedCart.DiscountCodes);

            //removing the discount code
            RemoveDiscountCodeUpdateAction removeDiscountCodeUpdateAction = new RemoveDiscountCodeUpdateAction()
            {
                DiscountCode = new Reference<DiscountCode>()
                    {Id = discountCode.Id, TypeId = ReferenceTypeId.DiscountCode}
            };
            updateActions.Clear();
            updateActions.Add(removeDiscountCodeUpdateAction);

            retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Empty(retrievedCart.DiscountCodes);
        }

        [Fact]
        public void UpdateCartSetCustomerGroup()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false);
            CustomerGroup customerGroup = this.cartFixture.CreateCustomerGroup();

            SetCustomerGroupUpdateAction setCustomerGroupUpdateAction = new SetCustomerGroupUpdateAction()
            {
                CustomerGroup = new ResourceIdentifier(){ Id = customerGroup.Id }
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setCustomerGroupUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(customerGroup.Id, retrievedCart.CustomerGroup.Id);

        }

        [Fact]
        public void UpdateCartSetCustomType()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false);

            var customType = this.cartFixture.CreateCustomType();
            var fields = this.cartFixture.CreateNewFields();

            SetCustomTypeUpdateAction setCustomTypeUpdateAction = new SetCustomTypeUpdateAction()
            {
                Type = new ResourceIdentifier() { Id = customType.Id }, Fields = fields
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setCustomTypeUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(customType.Id, retrievedCart.Custom.Type.Id);
        }

        [Fact]
        public void UpdateCartSetCustomField()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false);

            //Update the Cart first to add custom type to it
            var customType = this.cartFixture.CreateCustomType();
            var fields = this.cartFixture.CreateNewFields();

            SetCustomTypeUpdateAction setCustomTypeUpdateAction = new SetCustomTypeUpdateAction()
            {
                Type = new ResourceIdentifier() { Id = customType.Id }, Fields = fields
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setCustomTypeUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            Assert.Equal(customType.Id, retrievedCart.Custom.Type.Id);

            //Then update the custom field
            string stringFieldValue = this.cartFixture.RandomString(5);
            updateActions.Clear();
            SetCustomFieldUpdateAction setCustomFieldUpdateAction = new SetCustomFieldUpdateAction()
            {
                Name = "string-field",
                Value = stringFieldValue
            };
            updateActions.Add(setCustomFieldUpdateAction);

            retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(stringFieldValue, retrievedCart.Custom.Fields["string-field"]);
        }

        [Fact]
        public void UpdateCartChangeTaxMode()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false);

            TaxMode newTaxMode = TaxMode.ExternalAmount;

            ChangeTaxModeUpdateAction changeTaxModeUpdateAction = new ChangeTaxModeUpdateAction()
            {
                TaxMode = newTaxMode
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {changeTaxModeUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.NotEqual(cart.TaxMode, retrievedCart.TaxMode);
            Assert.Equal(newTaxMode, retrievedCart.TaxMode);

        }

        [Fact]
        public void UpdateCartSetShippingMethodTaxAmount()
        {
            //A shipping method tax amount can be set if the cart has the ExternalAmount TaxMode.

            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false, withDefaultShippingCountry: false);
            TaxMode newTaxMode = TaxMode.ExternalAmount;

            //Create update actions (make the cart with shippingMethod and externalAmount TaxMode)
            ShippingMethod shippingMethod = this.cartFixture.CreateShippingMethod(cart.ShippingAddress.Country);
            SetShippingMethodUpdateAction setShippingMethodUpdateAction = new SetShippingMethodUpdateAction()
            {
                ShippingMethod = new Reference<ShippingMethod>()
                {
                    Id = shippingMethod.Id,
                    TypeId = ReferenceTypeId.ShippingMethod
                }
            };
            ChangeTaxModeUpdateAction changeTaxModeUpdateAction = new ChangeTaxModeUpdateAction()
            {
                TaxMode = newTaxMode
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {};
            updateActions.Add(setShippingMethodUpdateAction);
            updateActions.Add(changeTaxModeUpdateAction);
            //Create update actions

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            Assert.Equal(shippingMethod.Name, retrievedCart.ShippingInfo.ShippingMethodName);
            Assert.Equal(newTaxMode, retrievedCart.TaxMode);

            // then change the tax amount
            updateActions.Clear();
            var externalTaxAmountDraft = this.cartFixture.GetExternalTaxAmountDraft();
            SetShippingMethodTaxAmountUpdateAction setTaxAmountUpdateAction = new SetShippingMethodTaxAmountUpdateAction
            {
                ExternalTaxAmount = externalTaxAmountDraft
            };
            updateActions.Add(setTaxAmountUpdateAction);


            retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);
            Assert.Equal(externalTaxAmountDraft.TotalGross, retrievedCart.ShippingInfo.TaxedPrice.TotalGross);
        }

        [Fact]
        public void UpdateCartSetShippingMethodTaxRate()
        {
            //A shipping method tax amount can be set if the cart has the ExternalAmount TaxMode.

            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false, withDefaultShippingCountry: false);
            TaxMode newTaxMode = TaxMode.External;

            //Create update actions (make the cart with shippingMethod and external TaxMode)
            ShippingMethod shippingMethod = this.cartFixture.CreateShippingMethod(cart.ShippingAddress.Country);
            SetShippingMethodUpdateAction setShippingMethodUpdateAction = new SetShippingMethodUpdateAction()
            {
                ShippingMethod = new Reference<ShippingMethod>()
                {
                    Id = shippingMethod.Id,
                    TypeId = ReferenceTypeId.ShippingMethod
                }
            };
            ChangeTaxModeUpdateAction changeTaxModeUpdateAction = new ChangeTaxModeUpdateAction()
            {
                TaxMode = newTaxMode
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {};
            updateActions.Add(setShippingMethodUpdateAction);
            updateActions.Add(changeTaxModeUpdateAction);
            //Create update actions

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            Assert.Equal(shippingMethod.Name, retrievedCart.ShippingInfo.ShippingMethodName);
            Assert.Equal(newTaxMode, retrievedCart.TaxMode);

            // then change the tax rate
            updateActions.Clear();
            var externalTaxRateDraft = this.cartFixture.GetExternalTaxRateDraft();
            SetShippingMethodTaxRateUpdateAction setTaxRateUpdateAction = new SetShippingMethodTaxRateUpdateAction
            {
                ExternalTaxRate = externalTaxRateDraft
            };
            updateActions.Add(setTaxRateUpdateAction);


            retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);
            Assert.Equal(externalTaxRateDraft.Name, retrievedCart.ShippingInfo.TaxRate.Name);
            Assert.Equal(externalTaxRateDraft.Amount, retrievedCart.ShippingInfo.TaxRate.Amount);
        }

        [Fact]
        public void UpdateCartChangeTaxRoundingMode()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer:false);

            RoundingMode newTaxRoundingMode = RoundingMode.HalfDown;

            ChangeTaxRoundingModeUpdateAction changeTaxRoundingModeUpdateAction = new ChangeTaxRoundingModeUpdateAction()
            {
                TaxRoundingMode = newTaxRoundingMode
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {changeTaxRoundingModeUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.NotEqual(cart.TaxRoundingMode, retrievedCart.TaxRoundingMode);
            Assert.Equal(newTaxRoundingMode, retrievedCart.TaxRoundingMode);

        }

        [Fact]
        public void UpdateCartSetShippingRateInput()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            throw new NotImplementedException();
        }

        [Fact]
        public void UpdateCartChangeTaxCalculationMode()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer:false);

            TaxCalculationMode newTaxCalculationMode = TaxCalculationMode.UnitPriceLevel;

            ChangeTaxCalculationModeUpdateAction changeTaxCalculationModeUpdateAction = new ChangeTaxCalculationModeUpdateAction()
            {
                TaxCalculationMode = newTaxCalculationMode
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {changeTaxCalculationModeUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.NotEqual(cart.TaxCalculationMode, retrievedCart.TaxCalculationMode);
            Assert.Equal(newTaxCalculationMode, retrievedCart.TaxCalculationMode);

        }

        [Fact]
        public void UpdateCartAddShoppingList()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer:false);
            ShoppingList shoppingList = this.cartFixture.CreateShoppingList();//Create a shopping list with lineItem

            Assert.Empty(cart.LineItems);

            AddShoppingListUpdateAction addShoppingListUpdateAction = new AddShoppingListUpdateAction()
            {
                ShoppingList = new ResourceIdentifier() { Id = shoppingList.Id }
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addShoppingListUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Single(retrievedCart.LineItems);
            Assert.Equal(shoppingList.LineItems[0].ProductId, retrievedCart.LineItems[0].ProductId);
            Assert.Equal(shoppingList.LineItems[0].Quantity, retrievedCart.LineItems[0].Quantity);

        }

        [Fact]
        public void UpdateCartSetCustomerId()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer:false);
            Customer customer = this.cartFixture.CreateCustomer();

            Assert.True(string.IsNullOrEmpty(cart.CustomerId));

            SetCustomerIdUpdateAction setCustomerIdUpdateAction = new SetCustomerIdUpdateAction
            {
                CustomerId = customer.Id
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setCustomerIdUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(customer.Id, retrievedCart.CustomerId);
        }
        [Fact]
        public void UpdateCartSetAnonymousId()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer:false);
            string anonymousId = Guid.NewGuid().ToString();

            SetAnonymousIdUpdateAction setAnonymousIdUpdateAction = new SetAnonymousIdUpdateAction
            {
                AnonymousId = anonymousId
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setAnonymousIdUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(anonymousId, retrievedCart.AnonymousId);
        }

        [Fact]
        public void UpdateCartRecalculate()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            //Create Product, LineItemDraft and Cart
            Product product = this.cartFixture.CreateProduct(cleanOnDispose: false);
            string sku = product.MasterData.Current.MasterVariant.Sku;
            LineItemDraft lineItemDraft = this.cartFixture.GetLineItemDraftBySku(sku, 1);
            Cart cart = this.cartFixture.CreateCart(withCustomer:false);

            AddLineItemBySkuUpdateAction addLineItemUpdateAction = new AddLineItemBySkuUpdateAction()
            {
                LineItem = lineItemDraft,
                Sku = sku,
                Quantity = lineItemDraft.Quantity
            };

            List<UpdateAction<Cart>> cartUpdateActions = new List<UpdateAction<Cart>> {addLineItemUpdateAction};

            Cart cartWithLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, cartUpdateActions))
                .Result;

            //make sure that the cart has one lineItem now
            Assert.Single(cartWithLineItem.LineItems);

            //update the product Price
            var oldPrice = cartWithLineItem.LineItems[0].Price;
            PriceDraft newPriceDraft = new PriceDraft()
            {
                Value = this.cartFixture.MultiplyMoney(oldPrice.Value, 2)
            };
            var changedPriceUpdateAction = new ChangePriceUpdateAction
            {
                Price = newPriceDraft, //the new price
                PriceId = product.MasterData.Current.MasterVariant.Prices[0].Id,
                Staged = false
            };
            var productUpdateActions = new List<UpdateAction<Product>> {changedPriceUpdateAction};

            Product productWithChangedPrice = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Product>(new Guid(product.Id), product.Version,
                    productUpdateActions)).Result;

            //make sure that product price is updated now
            //make sure that price of added lineItem not affected
            Assert.Equal(newPriceDraft.Value, productWithChangedPrice.MasterData.Current.MasterVariant.Prices[0].Value);
            Assert.NotEqual(newPriceDraft.Value, cartWithLineItem.LineItems[0].Price.Value);
            this.cartFixture.CleanProductOnDispose(productWithChangedPrice);

            //recalculate the cart
            RecalculateUpdateAction recalculateUpdateAction = new RecalculateUpdateAction
            {
                UpdateProductData = false // only the prices and tax rates of the line item will be updated (not name, productType, ..etc)
            };
            cartUpdateActions.Clear();
            cartUpdateActions.Add(recalculateUpdateAction);

            Cart recalculatedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cartWithLineItem.Id),
                    cartWithLineItem.Version, cartUpdateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(recalculatedCart);

            Assert.Single(cartWithLineItem.LineItems);
            // make sure that lineItem now updated , also the total price of cart
            Assert.Equal(newPriceDraft.Value, recalculatedCart.LineItems[0].Price.Value);
            Assert.NotEqual(cartWithLineItem.TotalPrice, recalculatedCart.TotalPrice);
            Assert.Equal(newPriceDraft.Value, recalculatedCart.TotalPrice);
        }

        [Fact]
        public void UpdateCartRecalculateProductData()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            throw new NotImplementedException();
        }

        [Fact]
        public void UpdateCartAddPayment()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false);
            Payment payment = this.cartFixture.CreatePayment();

            Assert.Null(cart.PaymentInfo);

            AddPaymentUpdateAction addPaymentUpdateAction = new AddPaymentUpdateAction()
            {
                Payment = new Reference<Payment>(){ Id= payment.Id, TypeId = ReferenceTypeId.Payment}
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addPaymentUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.NotNull(retrievedCart.PaymentInfo);
            Assert.Single(retrievedCart.PaymentInfo.Payments);

        }

        [Fact]
        public void UpdateCartRemovePayment()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false);
            Payment payment = this.cartFixture.CreatePayment();

            //Create Payment and Add to Cart
            Assert.Null(cart.PaymentInfo);
            AddPaymentUpdateAction addPaymentUpdateAction = new AddPaymentUpdateAction()
            {
                Payment = new Reference<Payment>(){ Id= payment.Id, TypeId = ReferenceTypeId.Payment}
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addPaymentUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            Assert.NotNull(retrievedCart.PaymentInfo);
            Assert.Single(retrievedCart.PaymentInfo.Payments);

            //Then Delete it
            RemovePaymentUpdateAction removePaymentUpdateAction = new RemovePaymentUpdateAction()
            {
                Payment = new Reference<Payment>(){ Id= payment.Id, TypeId = ReferenceTypeId.Payment}
            };

            updateActions.Clear();
            updateActions.Add(removePaymentUpdateAction);

            Cart cartWithOutPayments = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithOutPayments);
            Assert.Null(cartWithOutPayments.PaymentInfo);
        }

        [Fact]
        public void UpdateCartSetTotalTax()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false);

            //The total tax amount of the cart can be set if it has the ExternalAmount TaxMode
            TaxMode newTaxMode = TaxMode.ExternalAmount;

            ChangeTaxModeUpdateAction changeTaxModeUpdateAction = new ChangeTaxModeUpdateAction()
            {
                TaxMode = newTaxMode
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {changeTaxModeUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            Assert.Equal(newTaxMode, retrievedCart.TaxMode);

            // Then Set Cart Total Tax
            var totalTax = Money.Parse($"{this.cartFixture.RandomInt(100,1000)} EUR");
            SetCartTotalTaxUpdateAction setCartTotalTaxUpdateAction = new SetCartTotalTaxUpdateAction()
            {
                ExternalTotalGross = totalTax
            };
            updateActions.Clear();
            updateActions.Add(setCartTotalTaxUpdateAction);

            Cart cartWithTotalTax = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithTotalTax);

            Assert.Equal(totalTax, cartWithTotalTax.TaxedPrice.TotalGross);

        }
        [Fact]
        public void UpdateCartSetLocale()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer:false);
            var projectLanguages = this.cartFixture.GetProjectLanguages();

            Assert.True(projectLanguages.Count > 0);//make sure that project has at least one language
            Assert.Null(cart.Locale);

            string locale = projectLanguages[0];

            SetLocaleUpdateAction setLocaleUpdateAction = new SetLocaleUpdateAction()
            {
                Locale = locale
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setLocaleUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(locale, retrievedCart.Locale);
        }
        [Fact]
        public void UpdateCartSetDeleteDaysAfterLastModification()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart(withCustomer: false);
            int deleteDays = this.cartFixture.RandomInt(40, 100);// the created cart with DeleteDaysAfterLastModification = 30

            SetDeleteDaysAfterLastModificationUpdateAction setDeleteDaysUpdateAction = new SetDeleteDaysAfterLastModificationUpdateAction()
            {
                DeleteDaysAfterLastModification = deleteDays
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setDeleteDaysUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Equal(deleteDays, retrievedCart.DeleteDaysAfterLastModification);
        }

        [Fact]
        public void UpdateCartAddItemShippingAddress()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            Cart cart = this.cartFixture.CreateCart();
            var itemShippingAddress = this.cartFixture.GetRandomAddress();

            Assert.Empty(cart.ItemShippingAddresses);

            AddItemShippingAddressUpdateAction addItemShippingAddressUpdateAction = new AddItemShippingAddressUpdateAction()
            {
                Address = itemShippingAddress
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addItemShippingAddressUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Single(retrievedCart.ItemShippingAddresses);
            Assert.Equal(itemShippingAddress.Key, retrievedCart.ItemShippingAddresses[0].Key);
        }

        [Fact]
        public void UpdateCartRemoveItemShippingAddress()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            Cart cart = this.cartFixture.CreateCart();
            var itemShippingAddress = this.cartFixture.GetRandomAddress();

            Assert.Empty(cart.ItemShippingAddresses);

            //Add itemshippingAddress first

            AddItemShippingAddressUpdateAction addItemShippingAddressUpdateAction = new AddItemShippingAddressUpdateAction()
            {
                Address = itemShippingAddress
            };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addItemShippingAddressUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            Assert.Single(retrievedCart.ItemShippingAddresses);
            Assert.Equal(itemShippingAddress.Key, retrievedCart.ItemShippingAddresses[0].Key);

            // then remove it

            RemoveItemShippingAddressUpdateAction removeItemShippingAddressUpdateAction = new RemoveItemShippingAddressUpdateAction()
            {
                AddressKey = itemShippingAddress.Key
            };
            updateActions.Clear();
            updateActions.Add(removeItemShippingAddressUpdateAction);

            Cart cartWithEmptyItemShippingAddresses = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithEmptyItemShippingAddresses);

            Assert.Empty(cart.ItemShippingAddresses);
        }

        [Fact]
        public void UpdateCartUpdateItemShippingAddress()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            Cart cart = this.cartFixture.CreateCart();
            var itemShippingAddress = this.cartFixture.GetRandomAddress();

            Assert.Empty(cart.ItemShippingAddresses);

            //Add item shipping address first

            AddItemShippingAddressUpdateAction addItemShippingAddressUpdateAction =
                new AddItemShippingAddressUpdateAction()
                {
                    Address = itemShippingAddress
                };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addItemShippingAddressUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            Assert.Single(retrievedCart.ItemShippingAddresses);

            // then update it
            itemShippingAddress.StreetName = this.cartFixture.RandomString(10);
            UpdateItemShippingAddressUpdateAction updateItemShippingAddressUpdateAction =
                new UpdateItemShippingAddressUpdateAction()
                {
                    Address = itemShippingAddress // updated Address
                };
            updateActions.Clear();
            updateActions.Add(updateItemShippingAddressUpdateAction);

            Cart cartWithUpdatedItemShippingAddress = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithUpdatedItemShippingAddress);

            Assert.Single(retrievedCart.ItemShippingAddresses);
            Assert.Equal(itemShippingAddress.StreetName, cartWithUpdatedItemShippingAddress.ItemShippingAddresses[0].StreetName);

        }

        #endregion

        #region UpdateActionsOnCustomLineItem

        [Fact]
        public void UpdateCartAddCustomLineItem()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();
            Cart cart = this.cartFixture.CreateCart();
            var customLineItem = this.cartFixture.GetCustomLineItemDraft();

            Assert.Empty(cart.CustomLineItems);

            AddCustomLineItemUpdateAction addCustomLineItemUpdateAction =
                new AddCustomLineItemUpdateAction()
                {
                    Name = customLineItem.Name,
                    Slug = customLineItem.Slug,
                    Quantity = customLineItem.Quantity,
                    Money = customLineItem.Money,
                    TaxCategory = customLineItem.TaxCategory
                };

            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {addCustomLineItemUpdateAction};

            Cart retrievedCart = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(retrievedCart);

            Assert.Single(retrievedCart.CustomLineItems);
            Assert.Equal(customLineItem.Name["en"], retrievedCart.CustomLineItems[0].Name["en"]);
            Assert.Equal(customLineItem.Slug, retrievedCart.CustomLineItems[0].Slug);
        }

        [Fact]
        public void UpdateCartRemoveCustomLineItem()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            // First Create Cart with Custom Line Item
            Cart retrievedCart = this.cartFixture.CreateCartWithCustomLineItem();
            Assert.Single(retrievedCart.CustomLineItems);

            // then remove it
            string customLineItemId = retrievedCart.CustomLineItems[0].Id;
            RemoveCustomLineItemUpdateAction removeCustomLineItemUpdateAction = new RemoveCustomLineItemUpdateAction
            {
                CustomLineItemId = customLineItemId
            };
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {removeCustomLineItemUpdateAction};

            Cart cartWithEmptyCustomLineItems = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithEmptyCustomLineItems);

            Assert.Empty(cartWithEmptyCustomLineItems.CustomLineItems);

        }

        [Fact]
        public void UpdateCartChangeCustomLineItemQuantity()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            // First Create Cart with Custom Line Item
            Cart retrievedCart = this.cartFixture.CreateCartWithCustomLineItem();
            Assert.Single(retrievedCart.CustomLineItems);

            // Then update the Quantity of it
            string customLineItemId = retrievedCart.CustomLineItems[0].Id;
            long newQuantity = retrievedCart.CustomLineItems[0].Quantity + 2;
            ChangeCustomLineItemQuantityUpdateAction changeQuantityUpdateAction = new ChangeCustomLineItemQuantityUpdateAction
            {
                CustomLineItemId = customLineItemId,
                Quantity = newQuantity
            };
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {changeQuantityUpdateAction};

            Cart cartWithUpdatedQuantityForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithUpdatedQuantityForCustomLineItem);

            Assert.Single(cartWithUpdatedQuantityForCustomLineItem.CustomLineItems);
            Assert.Equal(newQuantity,cartWithUpdatedQuantityForCustomLineItem.CustomLineItems[0].Quantity);
        }

        [Fact]
        public void UpdateCartChangeCustomLineItemMoney()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            // First Create Cart with Custom Line Item
            Cart retrievedCart = this.cartFixture.CreateCartWithCustomLineItem();
            Assert.Single(retrievedCart.CustomLineItems);

            // Then update Money of it
            string customLineItemId = retrievedCart.CustomLineItems[0].Id;
            Money newMoney = this.cartFixture.MultiplyMoney(retrievedCart.CustomLineItems[0].Money, 2);
            ChangeCustomLineItemMoneyUpdateAction changeMoneyUpdateAction = new ChangeCustomLineItemMoneyUpdateAction
            {
                CustomLineItemId = customLineItemId,
                Money = newMoney
            };
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {changeMoneyUpdateAction};

            Cart cartWithUpdatedMoneyForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithUpdatedMoneyForCustomLineItem);

            Assert.Single(cartWithUpdatedMoneyForCustomLineItem.CustomLineItems);
            Assert.Equal(newMoney,cartWithUpdatedMoneyForCustomLineItem.CustomLineItems[0].Money);
        }

        [Fact]
        public void UpdateCartSetCustomLineItemCustomType()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            // First Create Cart with Custom Line Item
            Cart retrievedCart = this.cartFixture.CreateCartWithCustomLineItem();
            Assert.Single(retrievedCart.CustomLineItems);

            // Then set it's custom type
            string customLineItemId = retrievedCart.CustomLineItems[0].Id;
            var customType = this.cartFixture.CreateCustomType();
            var fields = this.cartFixture.CreateNewFields();

            SetCustomLineItemCustomTypeUpdateAction setCustomTypeUpdateAction = new SetCustomLineItemCustomTypeUpdateAction
            {
                CustomLineItemId = customLineItemId,
                Type = new ResourceIdentifier() { Id = customType.Id }, Fields = fields
            };
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setCustomTypeUpdateAction};

            Cart cartWithCustomTypeForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithCustomTypeForCustomLineItem);

            Assert.Single(cartWithCustomTypeForCustomLineItem.CustomLineItems);
            Assert.Equal(customType.Id, cartWithCustomTypeForCustomLineItem.CustomLineItems[0].Custom.Type.Id);
        }

        [Fact]
        public void UpdateCartSetCustomLineItemCustomField()
        {

            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            // First Create Cart with Custom Line Item
            Cart retrievedCart = this.cartFixture.CreateCartWithCustomLineItem();
            Assert.Single(retrievedCart.CustomLineItems);

            // Then set custom type for the custom line item
            string customLineItemId = retrievedCart.CustomLineItems[0].Id;
            var customType = this.cartFixture.CreateCustomType();
            var fields = this.cartFixture.CreateNewFields();

            SetCustomLineItemCustomTypeUpdateAction setCustomTypeUpdateAction = new SetCustomLineItemCustomTypeUpdateAction
            {
                CustomLineItemId = customLineItemId,
                Type = new ResourceIdentifier() { Id = customType.Id }, Fields = fields
            };
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setCustomTypeUpdateAction};

            Cart cartWithCustomTypeForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            Assert.Single(cartWithCustomTypeForCustomLineItem.CustomLineItems);
            Assert.Equal(customType.Id, cartWithCustomTypeForCustomLineItem.CustomLineItems[0].Custom.Type.Id);

            // then update it's fields

            string stringFieldValue = this.cartFixture.RandomString(5);
            SetCustomLineItemCustomFieldUpdateAction setCustomFieldUpdateAction = new SetCustomLineItemCustomFieldUpdateAction()
            {
                Name = "string-field",
                Value = stringFieldValue,
                CustomLineItemId = customLineItemId
            };
            updateActions.Clear();
            updateActions.Add(setCustomFieldUpdateAction);

            Cart cartWithUpdatedCustomFieldForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cartWithCustomTypeForCustomLineItem.Id),
                    cartWithCustomTypeForCustomLineItem.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithUpdatedCustomFieldForCustomLineItem);

            Assert.Single(cartWithUpdatedCustomFieldForCustomLineItem.CustomLineItems);
            Assert.Equal(stringFieldValue, cartWithUpdatedCustomFieldForCustomLineItem.CustomLineItems[0].Custom.Fields["string-field"]);
        }

        [Fact]
        public void UpdateCartSetCustomLineItemTaxRate()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            // First Create Cart with Custom Line Item and External Tax mode
            Cart retrievedCart = this.cartFixture.CreateCartWithCustomLineItemWithSpecificTaxMode(TaxMode.External);
            Assert.Single(retrievedCart.CustomLineItems);
            Assert.Equal(TaxMode.External, retrievedCart.TaxMode);

            // Then update TaxRate of it
            string customLineItemId = retrievedCart.CustomLineItems[0].Id;
            var externalTaxRateDraft = this.cartFixture.GetExternalTaxRateDraft();
            SetCustomLineItemTaxRateUpdateAction setTaxRateUpdateAction = new SetCustomLineItemTaxRateUpdateAction
            {
                CustomLineItemId = customLineItemId,
                ExternalTaxRate = externalTaxRateDraft
            };
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setTaxRateUpdateAction};

            Cart cartWithTaxRateForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithTaxRateForCustomLineItem);

            Assert.Single(cartWithTaxRateForCustomLineItem.CustomLineItems);
            Assert.Equal(externalTaxRateDraft.Name, cartWithTaxRateForCustomLineItem.CustomLineItems[0].TaxRate.Name);
            Assert.Equal(externalTaxRateDraft.Amount, cartWithTaxRateForCustomLineItem.CustomLineItems[0].TaxRate.Amount);
        }

        [Fact]
        public void UpdateCartSetCustomLineItemTaxAmount()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            // First Create Cart with Custom Line Item and ExternalAmount Tax mode
            Cart retrievedCart = this.cartFixture.CreateCartWithCustomLineItemWithSpecificTaxMode(TaxMode.ExternalAmount);
            Assert.Single(retrievedCart.CustomLineItems);
            Assert.Equal(TaxMode.ExternalAmount, retrievedCart.TaxMode);

            // Then update TaxRate of it
            string customLineItemId = retrievedCart.CustomLineItems[0].Id;
            var externalTaxAmountDraft = this.cartFixture.GetExternalTaxAmountDraft();
            SetCustomLineItemTaxAmountUpdateAction setTaxAmountUpdateAction = new SetCustomLineItemTaxAmountUpdateAction
            {
                CustomLineItemId = customLineItemId,
                ExternalTaxAmount = externalTaxAmountDraft
            };
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setTaxAmountUpdateAction};

            Cart cartWithTaxAmountForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithTaxAmountForCustomLineItem);

            Assert.Single(cartWithTaxAmountForCustomLineItem.CustomLineItems);
            Assert.Equal(externalTaxAmountDraft.TotalGross, cartWithTaxAmountForCustomLineItem.CustomLineItems[0].TaxedPrice.TotalGross);
        }

        [Fact]
        public void UpdateCartSetCustomLineItemShippingDetails()
        {
            IClient commerceToolsClient = this.cartFixture.GetService<IClient>();

            // First Create Cart with Custom Line Item
            Cart retrievedCart = this.cartFixture.CreateCartWithCustomLineItem(withItemShippingAddress: true);
            Assert.Single(retrievedCart.CustomLineItems);
            Assert.Single(retrievedCart.ItemShippingAddresses);

            // Then update Shipping Details of it
            string customLineItemId = retrievedCart.CustomLineItems[0].Id;
            string addressKey = retrievedCart.ItemShippingAddresses[0].Key;
            ItemShippingDetailsDraft itemShippingDetailsDraft = this.cartFixture.GetItemShippingDetailsDraft(addressKey);
            SetCustomLineItemShippingDetailsUpdateAction setShippingDetailsUpdateAction = new SetCustomLineItemShippingDetailsUpdateAction
            {
                CustomLineItemId = customLineItemId,
                ShippingDetails = itemShippingDetailsDraft
            };
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>> {setShippingDetailsUpdateAction};

            Cart cartWithShippingDetailsForCustomLineItem = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(retrievedCart.Id),
                    retrievedCart.Version, updateActions))
                .Result;

            this.cartFixture.CartToDelete.Add(cartWithShippingDetailsForCustomLineItem);

            Assert.Single(cartWithShippingDetailsForCustomLineItem.CustomLineItems);
            Assert.NotNull(cartWithShippingDetailsForCustomLineItem.CustomLineItems[0].ShippingDetails);
            Assert.Single(cartWithShippingDetailsForCustomLineItem.CustomLineItems[0].ShippingDetails.Targets);
            Assert.Equal(itemShippingDetailsDraft.Targets[0].Quantity,cartWithShippingDetailsForCustomLineItem.CustomLineItems[0].ShippingDetails.Targets[0].Quantity);
            Assert.Equal(itemShippingDetailsDraft.Targets[0].AddressKey,cartWithShippingDetailsForCustomLineItem.CustomLineItems[0].ShippingDetails.Targets[0].AddressKey);
        }

        [Fact]
        public void UpdateCartApplyDeltaToCustomLineItemShippingDetailsTargets()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
