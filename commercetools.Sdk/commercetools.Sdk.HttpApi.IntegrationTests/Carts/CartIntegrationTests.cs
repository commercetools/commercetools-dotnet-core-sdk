using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Carts.UpdateActions;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using SetCustomFieldUpdateAction = commercetools.Sdk.Domain.Carts.UpdateActions.SetCustomFieldUpdateAction;
using SetCustomTypeUpdateAction = commercetools.Sdk.Domain.Carts.UpdateActions.SetCustomTypeUpdateAction;
using Type = commercetools.Sdk.Domain.Type;

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

        #endregion
    }
}
