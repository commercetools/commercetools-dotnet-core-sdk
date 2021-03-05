using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Carts.UpdateActions;
using commercetools.Sdk.Domain.CustomObjects;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products.UpdateActions;
using commercetools.Sdk.Domain.Projects;
using commercetools.Sdk.Domain.Projects.UpdateActions;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.Stores;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using commercetools.Sdk.IntegrationTests.CustomObjects;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Carts.CartsFixture;
using static commercetools.Sdk.IntegrationTests.Customers.CustomersFixture;
using static commercetools.Sdk.IntegrationTests.ShippingMethods.ShippingMethodsFixture;
using static commercetools.Sdk.IntegrationTests.TaxCategories.TaxCategoriesFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using static commercetools.Sdk.IntegrationTests.DiscountCodes.DiscountCodesFixture;
using static commercetools.Sdk.IntegrationTests.CustomerGroups.CustomerGroupsFixture;
using static commercetools.Sdk.IntegrationTests.CustomObjects.CustomObjectsFixture;
using static commercetools.Sdk.IntegrationTests.ShoppingLists.ShoppingListsFixture;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;
using static commercetools.Sdk.IntegrationTests.Payments.PaymentsFixture;
using static commercetools.Sdk.IntegrationTests.Projects.ProjectFixture;
using static commercetools.Sdk.IntegrationTests.Stores.StoresFixture;
using static commercetools.Sdk.IntegrationTests.CartDiscounts.CartDiscountsFixture;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.Carts
{
    [Collection("Integration Tests")]
    public class CartsIntegrationTests
    {
        private readonly IClient client;

        public CartsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateCart()
        {
            await WithCart(
                client, DefaultCartDraft,
                cart =>
                {
                    Assert.NotNull(cart);
                    Assert.Null(cart.CustomerId);
                });
        }

        [Fact]
        public async Task CreateCartInStore()
        {
            await WithStore(client, async store =>
            {
                var buildDraft = DefaultCartDraft(new CartDraft());
                var cart = await client
                    .ExecuteAsync(new CreateCommand<Cart>(buildDraft).InStore(store.Key));

                Assert.NotNull(cart);
                Assert.NotNull(cart.Store);
                Assert.Equal(store.Key, cart.Store.Key);
                await DeleteResource(client, cart);
            });
        }

        [Fact]
        public async Task GetCartById()
        {
            await WithCart(
                client, DefaultCartDraft,
                async cart =>
                {
                    var retrievedCart = await client
                        .ExecuteAsync(cart.ToIdResourceIdentifier().GetById());
                    Assert.Equal(cart.Id, retrievedCart.Id);
                });
        }


        [Fact]
        public async Task GetCartByIdExpandLineItemDiscount()
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
                            var retrievedCart = await client
                                .ExecuteAsync(cart
                                    .ToIdResourceIdentifier()
                                    .GetById()
                                    .Expand("lineItems[*].discountedPricePerQuantity[*].discountedPrice.includedDiscounts[*].discount")
                                );

                            Assert.Equal(cart.Id, retrievedCart.Id);
                            var lineItem = retrievedCart.LineItems.FirstOrDefault();
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
        }

        [Fact]
        public async Task GetCartInStoreById()
        {
            await WithStore(client, async store =>
            {
                await WithCart(
                    client,
                    cartDraft => DefaultCartDraftInStore(cartDraft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.NotNull(cart.Store);
                        var retrievedCart = await client
                            .ExecuteAsync(cart.ToIdResourceIdentifier().GetById().InStore(store.Key));
                        Assert.Equal(cart.Id, retrievedCart.Id);
                        Assert.NotNull(retrievedCart.Store);
                        Assert.Equal(store.Key, retrievedCart.Store.Key);
                    });
            });
        }

        [Fact]
        public async Task GetCartByCustomerId()
        {
            await WithCustomer(client, async customer =>
            {
                await WithCart(
                    client,
                    cartDraft => DefaultCartDraftWithCustomer(cartDraft, customer),
                    async cart =>
                    {
                        Assert.NotNull(cart.CustomerId);
                        Assert.Equal(customer.Id, cart.CustomerId);

                        var retrievedCart = await client
                            .ExecuteAsync(new GetCartByCustomerIdCommand(customer.Id));

                        Assert.Equal(cart.Id, retrievedCart.Id);
                    });
            });
        }

        [Fact]
        public async Task GetCartInStoreByCustomerId()
        {
            await WithStore(client, async store =>
            {
                await WithCustomer(client, async customer =>
                {
                    await WithCart(
                        client,
                        draft =>
                        {
                            var cartDraft = DefaultCartDraftWithCustomer(draft, customer);
                            cartDraft.Store = store.ToKeyResourceIdentifier();
                            return cartDraft;
                        },
                        async cart =>
                        {
                            Assert.NotNull(cart.CustomerId);
                            Assert.Equal(customer.Id, cart.CustomerId);
                            Assert.NotNull(cart.Store);
                            Assert.Equal(store.Key, cart.Store.Key);

                            var retrievedCart = await client
                                .ExecuteAsync(new GetCartByCustomerIdCommand(customer.Id)
                                    .InStore(store.Key));

                            Assert.Equal(cart.Id, retrievedCart.Id);
                            Assert.NotNull(cart.Store);
                            Assert.Equal(store.Key, cart.Store.Key);
                        });
                });
            });
        }


        [Fact]
        public async Task ReplicateCartFromCart()
        {
            await WithCart(
                client, DefaultCartDraft,
                async cart =>
                {
                    var cartReplicationDraft = new ReplicaCartDraft()
                    {
                        Reference = new Reference<Cart>() {Id = cart.Id}
                    };

                    var replicatedCart = await client
                        .ExecuteAsync(new ReplicateCartCommand(cartReplicationDraft));

                    Assert.NotNull(replicatedCart);
                    Assert.Equal(CartState.Active, replicatedCart.CartState);
                });
        }

        [Fact]
        public async Task QueryCarts()
        {
            await WithCart(
                client, DefaultCartDraft,
                async cart =>
                {
                    var queryCommand = new QueryCommand<Cart>();
                    queryCommand.Where(c => c.Id == cart.Id.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(cart.Id, returnedSet.Results[0].Id);
                });
        }

        [Fact]
        public async Task QueryCartsInStore()
        {
            await WithStore(client, async store =>
            {
                await WithCart(
                    client,
                    draft => DefaultCartDraftInStore(draft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.NotNull(cart.Store);

                        var command = new QueryCommand<Cart>()
                            .Where(c => c.Id == cart.Id.valueOf())
                            .InStore(store.Key);

                        var returnedSet = await client.ExecuteAsync(command);
                        Assert.Single(returnedSet.Results);
                        Assert.Equal(cart.Id, returnedSet.Results[0].Id);
                        Assert.NotNull(returnedSet.Results[0].Store);
                        Assert.Equal(store.Key, returnedSet.Results[0].Store.Key);
                    });
            });
        }

        [Fact]
        public async Task DeleteCartById()
        {
            await WithCart(
                client, DefaultCartDraft,
                async cart =>
                {
                    await client.ExecuteAsync(cart.DeleteById());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Cart>(cart))
                    );
                });
        }

        [Fact]
        public async Task DeleteCartInStoreById()
        {
            await WithStore(client, async store =>
            {
                await WithCart(
                    client,
                    draft => DefaultCartDraftInStore(draft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.NotNull(cart.Store);
                        await client.ExecuteAsync(cart.DeleteById().InStore(store.Key));
                        await Assert.ThrowsAsync<NotFoundException>(
                            () => client.ExecuteAsync(
                                new GetByIdCommand<Cart>(cart).InStore(store.Key))
                        );
                    });
            });
        }

        #region UpdateActions

        [Fact]
        public async void UpdateCartSetCustomerEmail()
        {
            var email = $"{TestingUtility.RandomString()}@email.com";

            await WithUpdateableCart(client, async cart =>
            {
                var action = new SetCustomerEmailUpdateAction
                {
                    Email = email
                };

                var updatedCart = await client
                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(email, updatedCart.CustomerEmail);
                return updatedCart;
            });
        }

        [Fact]
        public async void UpdateCartInStoreSetCustomerEmail()
        {
            var email = $"{TestingUtility.RandomString()}@email.com";

            await WithStore(client, async store =>
            {
                await WithUpdateableCart(client,
                    draft => DefaultCartDraftInStore(draft, store.ToKeyResourceIdentifier()),
                    async cart =>
                    {
                        Assert.NotNull(cart.Store);
                        Assert.Equal(store.Key, cart.Store.Key);
                        var action = new SetCustomerEmailUpdateAction
                        {
                            Email = email
                        };

                        var updatedCart = await client
                            .ExecuteAsync(cart.UpdateById(
                                actions => actions.AddUpdate(action)).InStore(store.Key));

                        Assert.NotNull(updatedCart.Store);
                        Assert.Equal(store.Key, updatedCart.Store.Key);
                        Assert.Equal(email, updatedCart.CustomerEmail);
                        return updatedCart;
                    });
            });
        }

        [Fact]
        public async void UpdateCartSetShippingAddress()
        {
            await WithUpdateableCart(client, async cart =>
            {
                var shippingAddress = TestingUtility.GetRandomAddress();

                var action = new SetShippingAddressUpdateAction
                {
                    Address = shippingAddress
                };

                var updatedCart = await client
                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(shippingAddress.ToString(), updatedCart.ShippingAddress.ToString());
                return updatedCart;
            });
        }

        [Fact]
        public async void UpdateCartSetBillingAddress()
        {
            await WithUpdateableCart(client, async cart =>
            {
                var billingAddress = TestingUtility.GetRandomAddress();

                var action = new SetBillingAddressUpdateAction
                {
                    Address = billingAddress
                };

                var updatedCart = await client
                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(billingAddress.ToString(), updatedCart.BillingAddress.ToString());
                return updatedCart;
            });
        }

        [Fact]
        public async void UpdateCartSetCountry()
        {
            await WithUpdateableCart(client, async cart =>
            {
                var country = "EG";

                var action = new SetCountryUpdateAction
                {
                    Country = country
                };

                var updatedCart = await client
                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(country, updatedCart.Country);
                return updatedCart;
            });
        }

        [Fact]
        public async void UpdateCartSetShippingMethod()
        {
            var shippingAddress = TestingUtility.GetRandomAddress();
            await WithShippingMethodWithZoneRateAndTaxCategory(client,
                DefaultShippingMethodDraft, shippingAddress,
                async shippingMethod =>
                {
                    await WithUpdateableCart(client,
                        cartDraft => DefaultCartDraftWithShippingAddress(
                            cartDraft, shippingAddress),
                        async cart =>
                        {
                            Assert.NotNull(cart.ShippingAddress);

                            var action = new SetShippingMethodUpdateAction
                            {
                                ShippingMethod = shippingMethod.ToKeyResourceIdentifier()
                            };

                            var updatedCart = await client
                                .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Equal(shippingMethod.Name, updatedCart.ShippingInfo.ShippingMethodName);
                            return updatedCart;
                        });
                });
        }

        [Fact]
        public async void UpdateCartSetCustomShippingMethod()
        {
            var shippingAddress = TestingUtility.GetRandomAddress();
            var customShippingMethod = $"CustomShipping_{TestingUtility.RandomInt()}";
            var shippingRate = TestingUtility.GetShippingRateDraft();
            var taxRateDraft = TestingUtility.GetTaxRateDraft(shippingAddress);

            await WithTaxCategory(client,
                draft => DefaultTaxCategoryDraftWithTaxRate(draft, taxRateDraft),
                async taxCategory =>
                {
                    await WithUpdateableCart(client,
                        cartDraft => DefaultCartDraftWithShippingAddress(cartDraft, shippingAddress),
                        async cart =>
                        {
                            var action = new SetCustomShippingMethodUpdateAction
                            {
                                ShippingRate = shippingRate,
                                TaxCategory = taxCategory.ToKeyResourceIdentifier(),
                                ShippingMethodName = customShippingMethod
                            };

                            var updatedCart = await client
                                .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Equal(customShippingMethod, updatedCart.ShippingInfo.ShippingMethodName);
                            Assert.Equal(shippingRate.Price, updatedCart.ShippingInfo.ShippingRate.Price);
                            Assert.Equal(taxCategory.Id, updatedCart.ShippingInfo.TaxCategory.Id);

                            return updatedCart;
                        });
                });
        }

        [Fact]
        public async void UpdateCartAddDiscountCode()
        {
            var code = TestingUtility.RandomString();

            await WithDiscountCode(client,
                draft => DefaultDiscountCodeDraftWithCode(draft, code),
                async discountCode =>
                {
                    await WithUpdateableCart(client, async cart =>
                    {
                        var action = new AddDiscountCodeUpdateAction
                        {
                            Code = discountCode.Code
                        };

                        var updatedCart = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action))
                                .Expand(c => c.DiscountCodes.ExpandAll().DiscountCode));

                        Assert.Single(updatedCart.DiscountCodes);
                        Assert.Equal(code, updatedCart.DiscountCodes[0].DiscountCode.Obj.Code);
                        return updatedCart;
                    });
                });
        }

        [Fact]
        public async void UpdateCartRemoveDiscountCode()
        {
            var code = TestingUtility.RandomString();

            await WithDiscountCode(client,
                draft => DefaultDiscountCodeDraftWithCode(draft, code),
                async discountCode =>
                {
                    await WithUpdateableCart(client, async cart =>
                    {
                        Assert.Empty(cart.DiscountCodes);

                        var action = new AddDiscountCodeUpdateAction
                        {
                            Code = discountCode.Code
                        };

                        var cartWithDiscountCode = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action))
                                .Expand(c => c.DiscountCodes.ExpandAll().DiscountCode));

                        Assert.Single(cartWithDiscountCode.DiscountCodes);

                        var removeAction = new RemoveDiscountCodeUpdateAction
                        {
                            DiscountCode = discountCode.ToReference()
                        };

                        var cartWithoutDiscountCode = await client
                            .ExecuteAsync(cartWithDiscountCode.UpdateById(actions => actions.AddUpdate(removeAction)));

                        Assert.Empty(cart.DiscountCodes);

                        return cartWithoutDiscountCode;
                    });
                });
        }

        [Fact]
        public async void UpdateCartSetCustomerGroup()
        {
            await WithCustomerGroup(client, async customerGroup =>
            {
                await WithUpdateableCart(client, async cart =>
                {
                    Assert.Null(cart.CustomerGroup);
                    var action = new SetCustomerGroupUpdateAction
                    {
                        CustomerGroup = customerGroup.ToKeyResourceIdentifier()
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.NotNull(updatedCart.CustomerGroup);
                    Assert.Equal(customerGroup.Id, updatedCart.CustomerGroup.Id);
                    return updatedCart;
                });
            });
        }

        [Fact]
        public async Task UpdateCartSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableCart(client,
                    async cart =>
                    {
                        var action = new SetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };

                        var updatedCart = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.Equal(type.Id, updatedCart.Custom.Type.Id);
                        return updatedCart;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableCart(client,
                    cartDraft => DefaultCartDraftWithCustomType(cartDraft, type, fields),
                    async cart =>
                    {
                        var action = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };

                        var updatedCart = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.Equal(newValue, updatedCart.Custom.Fields["string-field"]);
                        return updatedCart;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartCustomFieldAsCustomObject()
        {
            var fooBar = new FooBar();

            await WithCustomObject<FooBar>(
                client,
                customObjectDraft =>
                    DefaultCustomObjectDraft(customObjectDraft, fooBar),
                async customObject =>
                {
                    Assert.Equal(fooBar.Bar, customObject.Value.Bar);

                    var fields = CreateNewFields();
                    // add the reference to the carts custom field
                    fields.Add("customobjectfield", customObject.ToReference());

                    await WithType(client, async type =>
                    {
                        await WithUpdateableCart(client,
                            async cart =>
                            {
                                var action = new SetCustomTypeUpdateAction
                                {
                                    Type = type.ToKeyResourceIdentifier(),
                                    Fields = fields
                                };

                                // update the cart and expand the custom object reference
                                var cartWithCustomType = await client
                                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action))
                                        .Expand(cart1 =>
                                            cart1.Custom.Fields.ExpandReferenceField("customobjectfield")));

                                Assert.Equal(type.Id, cartWithCustomType.Custom.Type.Id);

                                var customFieldReference =
                                    (Reference<CustomObject>) cartWithCustomType.Custom.Fields["customobjectfield"];
                                Assert.NotNull(customFieldReference.Obj);
                                var retrievedFooBar = customFieldReference.Obj as CustomObject;
                                Assert.NotNull(retrievedFooBar);
                                return cartWithCustomType;
                            });
                    });
                });
        }

        [Fact]
        public async void UpdateCartChangeTaxMode()
        {
            await WithUpdateableCart(client, cartDraft =>
                DefaultCartDraftWithTaxMode(cartDraft, TaxMode.Platform), async cart =>
            {
                Assert.Equal(TaxMode.Platform, cart.TaxMode);

                var newTaxMode = TaxMode.ExternalAmount;
                var action = new ChangeTaxModeUpdateAction
                {
                    TaxMode = newTaxMode
                };

                var updatedCart = await client
                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(newTaxMode, updatedCart.TaxMode);
                return updatedCart;
            });
        }

        [Fact]
        public async void UpdateCartSetShippingMethodTaxAmount()
        {
            //A shipping method tax amount can be set if the cart has the ExternalAmount TaxMode.
            var shippingAddress = TestingUtility.GetRandomAddress();
            await WithShippingMethodWithZoneRateAndTaxCategory(client,
                DefaultShippingMethodDraft, shippingAddress,
                async shippingMethod =>
                {
                    await WithUpdateableCart(client,
                        draft =>
                        {
                            var cartDraftWithExternalAmountMode =
                                DefaultCartDraftWithTaxMode(draft, TaxMode.ExternalAmount);
                            var cartDraftWithShippingAddress = DefaultCartDraftWithShippingAddress(
                                cartDraftWithExternalAmountMode
                                , shippingAddress);
                            var cartDraftWithShippingMethod =
                                DefaultCartDraftWithShippingMethod(cartDraftWithShippingAddress,
                                    shippingMethod);
                            return cartDraftWithShippingMethod;
                        },
                        async cart =>
                        {
                            Assert.Equal(TaxMode.ExternalAmount, cart.TaxMode);
                            Assert.Equal(shippingAddress.ToString(), cart.ShippingAddress.ToString());
                            Assert.Equal(shippingMethod.Name, cart.ShippingInfo.ShippingMethodName);

                            var externalTaxAmountDraft = TestingUtility.GetExternalTaxAmountDraft();

                            var action = new SetShippingMethodTaxAmountUpdateAction
                            {
                                ExternalTaxAmount = externalTaxAmountDraft
                            };

                            var updatedCart = await client
                                .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Equal(externalTaxAmountDraft.TotalGross,
                                updatedCart.ShippingInfo.TaxedPrice.TotalGross);
                            return updatedCart;
                        });
                });
        }

        [Fact]
        public async void UpdateCartSetShippingMethodTaxRate()
        {
            //A shipping method tax rate can be set if the cart has the External TaxMode.
            var shippingAddress = TestingUtility.GetRandomAddress();
            await WithShippingMethodWithZoneRateAndTaxCategory(client,
                DefaultShippingMethodDraft, shippingAddress,
                async shippingMethod =>
                {
                    await WithUpdateableCart(client,
                        draft =>
                        {
                            var cartDraftWithExternalAmountMode =
                                DefaultCartDraftWithTaxMode(draft, TaxMode.External);
                            var cartDraftWithShippingAddress = DefaultCartDraftWithShippingAddress(
                                cartDraftWithExternalAmountMode
                                , shippingAddress);
                            var cartDraftWithShippingMethod =
                                DefaultCartDraftWithShippingMethod(cartDraftWithShippingAddress,
                                    shippingMethod);
                            return cartDraftWithShippingMethod;
                        },
                        async cart =>
                        {
                            Assert.Equal(TaxMode.External, cart.TaxMode);
                            Assert.Equal(shippingAddress.ToString(), cart.ShippingAddress.ToString());
                            Assert.Equal(shippingMethod.Name, cart.ShippingInfo.ShippingMethodName);

                            var externalTaxRateDraft = TestingUtility.GetExternalTaxRateDraft();

                            var action = new SetShippingMethodTaxRateUpdateAction
                            {
                                ExternalTaxRate = externalTaxRateDraft
                            };

                            var updatedCart = await client
                                .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Equal(externalTaxRateDraft.Name, updatedCart.ShippingInfo.TaxRate.Name);
                            Assert.Equal(externalTaxRateDraft.Amount, updatedCart.ShippingInfo.TaxRate.Amount);
                            return updatedCart;
                        });
                });
        }

        [Fact]
        public async void UpdateCartChangeTaxRoundingMode()
        {
            await WithUpdateableCart(client, cartDraft =>
                DefaultCartDraftWithTaxRoundingMode(cartDraft, RoundingMode.HalfEven), async cart =>
            {
                Assert.Equal(RoundingMode.HalfEven, cart.TaxRoundingMode);

                var newTaxRoundingMode = RoundingMode.HalfDown;
                var action = new ChangeTaxRoundingModeUpdateAction
                {
                    TaxRoundingMode = newTaxRoundingMode
                };

                var updatedCart = await client
                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(newTaxRoundingMode, updatedCart.TaxRoundingMode);
                return updatedCart;
            });
        }


        //[Fact]
        private async void UpdateCartSetShippingRateInputAsScore()
        {
            var shippingRateInputType = new CartScoreShippingRateInputType();
            var shippingAddress = new Address {Country = "DE"};

            await WithCurrentProject(client,
                project => SetShippingRateInputType(project, shippingRateInputType),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.NotNull(project.ShippingRateInputType);
                    Assert.IsType<CartScoreShippingRateInputType>(project.ShippingRateInputType);

                    await WithUpdateableCart(client, draft =>
                    {
                        var cartDraft = DefaultCartDraftWithTaxMode(draft, TaxMode.External);
                        var cartWithShippingAddress =
                            DefaultCartDraftWithShippingAddress(cartDraft, shippingAddress);
                        return cartWithShippingAddress;
                    }, async cart =>
                    {
                        var externalTaxRateDraft = TestingUtility.GetExternalTaxRateDraft();
                        var shippingRateDraft = TestingUtility.GetShippingRateDraftWithPriceTiers();
                        var customShippingMethod = $"CustomShipping_{TestingUtility.RandomInt()}";
                        var secondScorePrice =
                            (shippingRateDraft.Tiers[1] as CartScoreShippingRatePriceTier)?.Price;

                        var setCustomShippingMethodAction = new SetCustomShippingMethodUpdateAction()
                        {
                            ShippingMethodName = customShippingMethod,
                            ShippingRate = shippingRateDraft, //with shipping rate price tiers
                            ExternalTaxRate = externalTaxRateDraft,
                            TaxCategory = null
                        };

                        var cartWithShippingMethod = await client
                            .ExecuteAsync(cart.UpdateById(actions =>
                                actions.AddUpdate(setCustomShippingMethodAction)));
                        Assert.NotNull(cartWithShippingMethod.ShippingInfo);
                        Assert.Equal(customShippingMethod, cartWithShippingMethod.ShippingInfo.ShippingMethodName);

                        var setShippingRateInputAction = new SetShippingRateInputUpdateAction
                        {
                            ShippingRateInput = new ScoreShippingRateInputDraft {Score = 1}
                        };
                        var cartWithShippingMethodWithScore1 = await client
                            .ExecuteAsync(cartWithShippingMethod.UpdateById(actions =>
                                actions.AddUpdate(setShippingRateInputAction)));

                        Assert.NotNull(cartWithShippingMethodWithScore1.ShippingRateInput);
                        Assert.IsType<ScoreShippingRateInput>(cartWithShippingMethodWithScore1.ShippingRateInput);
                        Assert.Equal(secondScorePrice, cartWithShippingMethodWithScore1.ShippingInfo.Price);
                        return cartWithShippingMethodWithScore1;
                    });

                    // then reset current project shippingRateInputType
                    var undoProjectAction = new SetShippingRateInputTypeUpdateAction
                    {
                        ShippingRateInputType = null
                    };
                    var projectWithNullShippingRateInputType = await
                        TryToUpdateCurrentProject(client, project, undoProjectAction.ToList());

                    Assert.Null(projectWithNullShippingRateInputType.ShippingRateInputType);
                    return projectWithNullShippingRateInputType;
                });
        }

        [Fact]
        public async void UpdateCartSetShippingRateInputAsClassification()
        {
            var shippingAddress = new Address {Country = "DE"};
            var classificationValues =
                TestingUtility.GetCartClassificationTestValues(); //Small, Medium and Heavy classifications
            var shippingRateInputType = new CartClassificationShippingRateInputType
            {
                Values = classificationValues
            };

            await WithCurrentProject(client,
                project => SetShippingRateInputType(project, shippingRateInputType),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.NotNull(project.ShippingRateInputType);
                    Assert.IsType<CartClassificationShippingRateInputType>(project.ShippingRateInputType);

                    await WithUpdateableCart(client, draft =>
                        {
                            var cartDraft = DefaultCartDraftWithTaxMode(draft, TaxMode.External);
                            var cartWithShippingAddress =
                                DefaultCartDraftWithShippingAddress(cartDraft, shippingAddress);
                            return cartWithShippingAddress;
                        },
                        async cart =>
                        {
                            var externalTaxRateDraft = TestingUtility.GetExternalTaxRateDraft();
                            var shippingRateDraft = TestingUtility.GetShippingRateDraftWithCartClassifications();
                            string customShippingMethod = $"CustomShipping_{TestingUtility.RandomInt()}";
                            var smallClassificationPrice =
                                (shippingRateDraft.Tiers[0] as CartClassificationShippingRatePriceTier)?.Price;

                            var setCustomShippingMethodAction = new SetCustomShippingMethodUpdateAction()
                            {
                                ShippingMethodName = customShippingMethod,
                                ShippingRate = shippingRateDraft, //with shipping rate price tiers
                                ExternalTaxRate = externalTaxRateDraft,
                                TaxCategory = null
                            };

                            var cartWithShippingMethod = await client
                                .ExecuteAsync(cart.UpdateById(actions =>
                                    actions.AddUpdate(setCustomShippingMethodAction)));
                            Assert.NotNull(cartWithShippingMethod.ShippingInfo);
                            Assert.Equal(customShippingMethod,
                                cartWithShippingMethod.ShippingInfo.ShippingMethodName);

                            var setShippingRateInputAction = new SetShippingRateInputUpdateAction
                            {
                                ShippingRateInput = new ClassificationShippingRateInputDraft {Key = "Small"}
                            };
                            var cartWithShippingMethodWithSmallClassification = await client
                                .ExecuteAsync(cartWithShippingMethod.UpdateById(actions =>
                                    actions.AddUpdate(setShippingRateInputAction)));

                            Assert.NotNull(cartWithShippingMethodWithSmallClassification.ShippingRateInput);
                            Assert.IsType<ClassificationShippingRateInput>(
                                cartWithShippingMethodWithSmallClassification
                                    .ShippingRateInput);
                            Assert.Equal(smallClassificationPrice,
                                cartWithShippingMethodWithSmallClassification.ShippingInfo.Price);
                            return cartWithShippingMethodWithSmallClassification;
                        });

                    // then reset current project shippingRateInputType
                    var undoProjectAction = new SetShippingRateInputTypeUpdateAction
                    {
                        ShippingRateInputType = null
                    };
                    var projectWithNullShippingRateInputType = await
                        TryToUpdateCurrentProject(client, project, undoProjectAction.ToList());

                    Assert.Null(projectWithNullShippingRateInputType.ShippingRateInputType);
                    return projectWithNullShippingRateInputType;
                });
        }

        [Fact]
        public async void UpdateCartChangeTaxCalculationMode()
        {
            await WithUpdateableCart(client, cartDraft =>
                    DefaultCartDraftWithTaxCalculationMode(cartDraft, TaxCalculationMode.LineItemLevel),
                async cart =>
                {
                    Assert.Equal(TaxCalculationMode.LineItemLevel, cart.TaxCalculationMode);

                    var newTaxCalculationMode = TaxCalculationMode.UnitPriceLevel;
                    var action = new ChangeTaxCalculationModeUpdateAction
                    {
                        TaxCalculationMode = newTaxCalculationMode
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(newTaxCalculationMode, updatedCart.TaxCalculationMode);
                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartAddShoppingList()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithProduct(client,
                    productDraft =>
                        DefaultProductDraftWithTaxCategory(productDraft, taxCategory),
                    async product =>
                    {
                        await WithShoppingList(client,
                            shoppingListDraft => DefaultShoppingListDraftWithSingleLineItem(shoppingListDraft, product),
                            async shoppingList =>
                            {
                                Assert.Single(shoppingList.LineItems);
                                await WithUpdateableCart(client,
                                    async cart =>
                                    {
                                        Assert.Empty(cart.LineItems);
                                        var action = new AddShoppingListUpdateAction
                                        {
                                            ShoppingList = shoppingList.ToKeyResourceIdentifier()
                                        };

                                        var updatedCart = await client
                                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                                        Assert.Single(updatedCart.LineItems);
                                        Assert.Equal(shoppingList.LineItems[0].ProductId,
                                            updatedCart.LineItems[0].ProductId);
                                        Assert.Equal(shoppingList.LineItems[0].Quantity,
                                            updatedCart.LineItems[0].Quantity);
                                        return updatedCart;
                                    });
                            });
                    });
            });
        }

        [Fact]
        public async Task UpdateCartSetCustomerId()
        {
            await WithCustomer(client, async customer =>
            {
                await WithUpdateableCart(client,
                    async cart =>
                    {
                        Assert.True(string.IsNullOrEmpty(cart.CustomerId));

                        var action = new SetCustomerIdUpdateAction
                        {
                            CustomerId = customer.Id
                        };

                        var updatedCart = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.Equal(customer.Id, updatedCart.CustomerId);
                        return updatedCart;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartSetAnonymousId()
        {
            await WithUpdateableCart(client,
                async cart =>
                {
                    var anonymousId = Guid.NewGuid().ToString();

                    var action = new SetAnonymousIdUpdateAction
                    {
                        AnonymousId = anonymousId
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(anonymousId, updatedCart.AnonymousId);
                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartRecalculate()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithUpdateableProduct(client,
                    productDraft =>
                        DefaultProductDraftWithTaxCategory(productDraft, taxCategory),
                    async product =>
                    {
                        Product updatedProduct = null;
                        var lineItemDraft = new LineItemDraft
                        {
                            Sku = product.MasterData.Current.MasterVariant.Sku,
                            Quantity = 1
                        };
                        await WithUpdateableCart(client,
                            async cart =>
                            {
                                Assert.Empty(cart.LineItems);
                                var action = new AddLineItemUpdateAction
                                {
                                    Sku = lineItemDraft.Sku,
                                    Quantity = lineItemDraft.Quantity
                                };

                                var cartWithLineItem = await client
                                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                                //make sure that the cart has one lineItem now
                                Assert.Single(cartWithLineItem.LineItems);

                                //update the product Price
                                var oldPrice = cartWithLineItem.LineItems[0].Price;
                                var newPriceDraft = new PriceDraft()
                                {
                                    Value = TestingUtility.MultiplyMoney(oldPrice.Value, 2)
                                };

                                var changedPriceUpdateAction = new ChangePriceUpdateAction
                                {
                                    Price = newPriceDraft, //the new price
                                    PriceId = product.MasterData.Current.MasterVariant.Prices[0].Id,
                                    Staged = false
                                };

                                var productWithChangedPrice = await client
                                    .ExecuteAsync(product.UpdateById(
                                        actions => actions.AddUpdate(changedPriceUpdateAction)));

                                updatedProduct = productWithChangedPrice;

                                //make sure that product price is updated now
                                //make sure that price of added lineItem not affected
                                cartWithLineItem =
                                    await client.ExecuteAsync(cartWithLineItem.GetById()); //retrieve the cart again

                                Assert.Equal(newPriceDraft.Value,
                                    productWithChangedPrice.MasterData.Current.MasterVariant.Prices[0].Value);
                                Assert.NotEqual(newPriceDraft.Value, cartWithLineItem.LineItems[0].Price.Value);

                                //recalculate the cart
                                var recalculateUpdateAction = new RecalculateUpdateAction
                                {
                                    UpdateProductData =
                                        false // only the prices and tax rates of the line item will be updated (not name, productType, ..etc)
                                };

                                var recalculatedCart = await client
                                    .ExecuteAsync(cartWithLineItem.UpdateById(actions =>
                                        actions.AddUpdate(recalculateUpdateAction)));

                                Assert.Single(recalculatedCart.LineItems);
                                // make sure that lineItem now updated , also the total price of cart
                                Assert.Equal(newPriceDraft.Value, recalculatedCart.LineItems[0].Price.Value);
                                Assert.NotEqual(cartWithLineItem.TotalPrice, recalculatedCart.TotalPrice);
                                return recalculatedCart;
                            });
                        return updatedProduct;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartRecalculateProductData()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithUpdateableProduct(client,
                    productDraft =>
                        DefaultProductDraftWithTaxCategory(productDraft, taxCategory),
                    async product =>
                    {
                        Product updatedProduct = null;
                        var lineItemDraft = new LineItemDraft
                        {
                            Sku = product.MasterData.Current.MasterVariant.Sku,
                            Quantity = 1
                        };
                        await WithUpdateableCart(client,
                            async cart =>
                            {
                                Assert.Empty(cart.LineItems);
                                var action = new AddLineItemUpdateAction
                                {
                                    Sku = lineItemDraft.Sku,
                                    Quantity = lineItemDraft.Quantity
                                };

                                var cartWithLineItem = await client
                                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                                //make sure that the cart has one lineItem now
                                Assert.Single(cartWithLineItem.LineItems);

                                //update the product, change it's textAttributeValue
                                var newTextAttributeValue = TestingUtility.RandomString();
                                var textAttributeName = "text-attribute-name";
                                string oldTextAttributeValue =
                                    product.MasterData.Current.MasterVariant.GetTextAttributeValue(textAttributeName);

                                var setAttributeUpdateAction = new SetAttributeInAllVariantsUpdateAction
                                {
                                    Name = textAttributeName,
                                    Value = newTextAttributeValue
                                };

                                var productWithChangedAttribute = await client
                                    .ExecuteAsync(product.UpdateById(
                                        actions => actions.AddUpdate(setAttributeUpdateAction)));

                                updatedProduct = productWithChangedAttribute;

                                //make sure that product price is updated now
                                //make sure that price of added lineItem not affected
                                cartWithLineItem =
                                    await client.ExecuteAsync(cartWithLineItem.GetById()); //retrieve the cart again

                                var productAttributeValue =
                                    productWithChangedAttribute.MasterData.Current.MasterVariant.GetTextAttributeValue(
                                        textAttributeName);
                                var lineItemAttributeValue = cartWithLineItem.LineItems[0].Variant
                                    .GetTextAttributeValue(textAttributeName);

                                //make sure both attributes has value and only the product attribute is the updated, and the lineItem Product attribute still has old value
                                Assert.NotNull(productAttributeValue);
                                Assert.NotNull(lineItemAttributeValue);
                                Assert.Equal(newTextAttributeValue, productAttributeValue);
                                Assert.Equal(oldTextAttributeValue, lineItemAttributeValue);

                                //recalculate the cart to update the attribute of the product of the line item
                                var recalculateUpdateAction = new RecalculateUpdateAction
                                {
                                    UpdateProductData =
                                        true // the line item product data (name, variant, productType and attributes) will also be updated
                                };

                                var recalculatedCart = await client
                                    .ExecuteAsync(cartWithLineItem.UpdateById(actions =>
                                        actions.AddUpdate(recalculateUpdateAction)));

                                Assert.Single(recalculatedCart.LineItems);
                                // make sure that lineItem now updated
                                lineItemAttributeValue = recalculatedCart.LineItems[0].Variant
                                    .GetTextAttributeValue(textAttributeName);
                                Assert.NotNull(lineItemAttributeValue);
                                Assert.Equal(newTextAttributeValue, lineItemAttributeValue);
                                return recalculatedCart;
                            });
                        return updatedProduct;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartAddPayment()
        {
            await WithPayment(client, async payment =>
            {
                await WithUpdateableCart(client,
                    async cart =>
                    {
                        Assert.Null(cart.PaymentInfo);
                        var action = new AddPaymentUpdateAction
                        {
                            Payment = payment.ToKeyResourceIdentifier()
                        };

                        var updatedCart = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.NotNull(updatedCart.PaymentInfo);
                        Assert.Single(updatedCart.PaymentInfo.Payments);
                        return updatedCart;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartRemovePayment()
        {
            await WithPayment(client, async payment =>
            {
                await WithUpdateableCart(client,
                    async cart =>
                    {
                        Assert.Null(cart.PaymentInfo);
                        var action = new AddPaymentUpdateAction
                        {
                            Payment = payment.ToKeyResourceIdentifier()
                        };

                        var cartWithPayment = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.NotNull(cartWithPayment.PaymentInfo);
                        Assert.Single(cartWithPayment.PaymentInfo.Payments);

                        //Then Delete it
                        var removePaymentUpdateAction = new RemovePaymentUpdateAction()
                        {
                            Payment = payment.ToKeyResourceIdentifier()
                        };


                        var cartWithOutPayments = await client
                            .ExecuteAsync(cartWithPayment.UpdateById(actions =>
                                actions.AddUpdate(removePaymentUpdateAction)));

                        Assert.Null(cartWithOutPayments.PaymentInfo);
                        return cartWithPayment;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartSetTotalTax()
        {
            await WithUpdateableCart(client,
                draft =>
                {
                    var cartDraftWithExternalAmountMode =
                        DefaultCartDraftWithTaxMode(draft, TaxMode.ExternalAmount);
                    return cartDraftWithExternalAmountMode;
                },
                async cart =>
                {
                    Assert.Equal(TaxMode.ExternalAmount, cart.TaxMode);

                    var totalTax = Money.FromDecimal("EUR", TestingUtility.RandomInt(100, 1000));
                    var action = new SetCartTotalTaxUpdateAction
                    {
                        ExternalTotalGross = totalTax
                    };

                    var cartWithTotalTax = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(totalTax, cartWithTotalTax.TaxedPrice.TotalGross);
                    return cartWithTotalTax;
                });
        }

        [Fact]
        public async Task UpdateCartSetLocale()
        {
            await WithUpdateableCart(client, async cart =>
            {
                var projectLanguages = GetProjectLanguages(client);

                Assert.True(projectLanguages.Count > 0); //make sure that project has at least one language
                Assert.Null(cart.Locale);

                var locale = projectLanguages[0];

                var action = new SetLocaleUpdateAction()
                {
                    Locale = locale
                };

                var updatedCart = await client
                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(locale, updatedCart.Locale);
                return updatedCart;
            });
        }

        [Fact]
        public async Task UpdateCartSetDeleteDaysAfterLastModification()
        {
            await WithUpdateableCart(client, async cart =>
            {
                var deleteDays = TestingUtility.RandomInt(1, 100);
                var action = new SetDeleteDaysAfterLastModificationUpdateAction
                {
                    DeleteDaysAfterLastModification = deleteDays
                };

                var updatedCart = await client
                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(deleteDays, updatedCart.DeleteDaysAfterLastModification);
                return updatedCart;
            });
        }

        [Fact]
        public async Task UpdateCartAddItemShippingAddress()
        {
            await WithUpdateableCart(client, async cart =>
            {
                var itemShippingAddress = TestingUtility.GetRandomAddress();

                Assert.Empty(cart.ItemShippingAddresses);
                var action = new AddItemShippingAddressUpdateAction
                {
                    Address = itemShippingAddress
                };

                var updatedCart = await client
                    .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedCart.ItemShippingAddresses);
                Assert.Equal(itemShippingAddress.Key, updatedCart.ItemShippingAddresses[0].Key);
                return updatedCart;
            });
        }

        [Fact]
        public async Task UpdateCartRemoveItemShippingAddress()
        {
            var addresses = new List<Address> {TestingUtility.GetRandomAddress()};

            await WithUpdateableCart(client, cartDraft =>
                    DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    Assert.Single(cart.ItemShippingAddresses);
                    var action = new RemoveItemShippingAddressUpdateAction
                    {
                        AddressKey = addresses[0].Key
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Empty(updatedCart.ItemShippingAddresses);
                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartUpdateItemShippingAddress()
        {
            var oldAddress = TestingUtility.GetRandomAddress();
            var newAddress = TestingUtility.GetRandomAddress();
            newAddress.Key = oldAddress.Key; //so it can be updated
            var addresses = new List<Address> {oldAddress};

            await WithUpdateableCart(client, cartDraft =>
                    DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    Assert.Single(cart.ItemShippingAddresses);
                    Assert.Equal(oldAddress.ToString(), cart.ItemShippingAddresses[0].ToString());

                    var action = new UpdateItemShippingAddressUpdateAction
                    {
                        Address = newAddress
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.ItemShippingAddresses);
                    Assert.Equal(newAddress.ToString(), updatedCart.ItemShippingAddresses[0].ToString());
                    return updatedCart;
                });
        }

        #endregion

        #region UpdateActionsOnLineItems

        [Fact]
        public async Task UpdateCartAddLineItemByProductId()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithProduct(client,
                    productDraft =>
                        DefaultProductDraftWithTaxCategory(productDraft, taxCategory),
                    async product =>
                    {
                        await WithUpdateableCart(client, async cart =>
                        {
                            Assert.Empty(cart.LineItems);
                            var lineItemDraft = new LineItemDraft
                            {
                                ProductId = product.Id,
                                VariantId = 1,
                                Quantity = 5
                            };
                            var action = new AddLineItemUpdateAction
                            {
                                ProductId = product.Id,
                                VariantId = lineItemDraft.VariantId,
                                Quantity = lineItemDraft.Quantity
                            };

                            var updatedCart = await client
                                .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedCart.LineItems);
                            Assert.Equal(lineItemDraft.ProductId, updatedCart.LineItems[0].ProductId);
                            Assert.Equal(lineItemDraft.Quantity, updatedCart.LineItems[0].Quantity);
                            return updatedCart;
                        });
                    });
            });
        }

        [Fact]
        public async Task UpdateCartAddLineItemBySku()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithProduct(client,
                    productDraft =>
                        DefaultProductDraftWithTaxCategory(productDraft, taxCategory),
                    async product =>
                    {
                        await WithUpdateableCart(client, async cart =>
                        {
                            Assert.Empty(cart.LineItems);
                            var lineItemDraft = new LineItemDraft
                            {
                                Sku = product.MasterData.Staged.MasterVariant.Sku,
                                Quantity = 5
                            };
                            var action = new AddLineItemUpdateAction
                            {
                                Sku = lineItemDraft.Sku,
                                Quantity = lineItemDraft.Quantity
                            };

                            var updatedCart = await client
                                .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedCart.LineItems);
                            Assert.Equal(lineItemDraft.Sku, updatedCart.LineItems[0].Variant.Sku);
                            Assert.Equal(lineItemDraft.Quantity, updatedCart.LineItems[0].Quantity);
                            return updatedCart;
                        });
                    });
            });
        }

        [Fact]
        public async Task UpdateCartRemoveLineItem()
        {
            var quantity = 5;
            await WithUpdateableCartWithSingleLineItem(client, quantity,
                DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    var lineItem = cart.LineItems[0];
                    Assert.Equal(quantity, lineItem.Quantity);

                    var action = new RemoveLineItemUpdateAction
                    {
                        LineItemId = lineItem.Id,
                        Quantity = lineItem.Quantity
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Empty(updatedCart.LineItems);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartRemoveLineItemDecreasesQuantity()
        {
            var quantity = 5;
            await WithUpdateableCartWithSingleLineItem(client, quantity,
                DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    var lineItem = cart.LineItems[0];
                    Assert.Equal(quantity, lineItem.Quantity);

                    var decreasedQuantity = lineItem.Quantity - 1;
                    var action = new RemoveLineItemUpdateAction
                    {
                        LineItemId = lineItem.Id,
                        Quantity = decreasedQuantity
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.LineItems);
                    Assert.Equal(1, updatedCart.LineItems[0].Quantity);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartChangeLineItemQuantity()
        {
            var quantity = 5;
            await WithUpdateableCartWithSingleLineItem(client, quantity,
                DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    var lineItem = cart.LineItems[0];
                    Assert.Equal(quantity, lineItem.Quantity);

                    var newQuantity = lineItem.Quantity - 1;

                    var action = new ChangeLineItemQuantityUpdateAction
                    {
                        LineItemId = lineItem.Id,
                        Quantity = newQuantity
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.LineItems);
                    Assert.Equal(newQuantity, updatedCart.LineItems[0].Quantity);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartSetLineItemTaxRate()
        {
            var quantity = 5;
            await WithUpdateableCartWithSingleLineItem(client, quantity,
                cartDraft => DefaultCartDraftWithTaxMode(cartDraft, TaxMode.External),
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(TaxMode.External, cart.TaxMode);

                    var lineItemId = cart.LineItems[0].Id;
                    var externalTaxRateDraft = TestingUtility.GetExternalTaxRateDraft();
                    var action = new SetLineItemTaxRateUpdateAction
                    {
                        LineItemId = lineItemId,
                        ExternalTaxRate = externalTaxRateDraft
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.LineItems);
                    Assert.Equal(externalTaxRateDraft.Name, updatedCart.LineItems[0].TaxRate.Name);
                    Assert.Equal(externalTaxRateDraft.Amount,
                        updatedCart.LineItems[0].TaxRate.Amount);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartSetLineItemTaxAmount()
        {
            var quantity = 5;
            await WithUpdateableCartWithSingleLineItem(client, quantity,
                cartDraft => DefaultCartDraftWithTaxMode(cartDraft, TaxMode.ExternalAmount),
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(TaxMode.ExternalAmount, cart.TaxMode);

                    var lineItemId = cart.LineItems[0].Id;
                    var externalTaxAmountDraft = TestingUtility.GetExternalTaxAmountDraft();
                    var action = new SetLineItemTaxAmountUpdateAction
                    {
                        LineItemId = lineItemId,
                        ExternalTaxAmount = externalTaxAmountDraft
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.LineItems);
                    Assert.Equal(externalTaxAmountDraft.TotalGross,
                        updatedCart.LineItems[0].TaxedPrice.TotalGross);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartSetLineItemPrice()
        {
            var quantity = 5;
            await WithUpdateableCartWithSingleLineItem(client, quantity,
                cartDraft => DefaultCartDraftWithTaxMode(cartDraft, TaxMode.ExternalAmount),
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(TaxMode.ExternalAmount, cart.TaxMode);

                    var lineItemId = cart.LineItems[0].Id;
                    var newPriceValue = TestingUtility.MultiplyMoney(cart.LineItems[0].Price.Value, 2);
                    var action = new SetLineItemPriceUpdateAction
                    {
                        LineItemId = lineItemId,
                        ExternalPrice = newPriceValue
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.LineItems);
                    Assert.Equal(LineItemPriceMode.ExternalPrice,
                        updatedCart.LineItems[0].PriceMode);
                    Assert.Equal(newPriceValue,
                        updatedCart.LineItems[0].Price.Value);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartSetLineItemTotalPrice()
        {
            var quantity = 5;
            await WithUpdateableCartWithSingleLineItem(client, quantity,
                cartDraft => DefaultCartDraftWithTaxMode(cartDraft, TaxMode.ExternalAmount),
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Equal(TaxMode.ExternalAmount, cart.TaxMode);

                    var lineItemId = cart.LineItems[0].Id;
                    var newPriceValue = TestingUtility.MultiplyMoney(cart.LineItems[0].Price.Value, 2);
                    var newTotalPriceValue = TestingUtility.MultiplyMoney(cart.LineItems[0].TotalPrice, 2);
                    var action = new SetLineItemTotalPriceUpdateAction
                    {
                        LineItemId = lineItemId,
                        ExternalTotalPrice = new ExternalLineItemTotalPrice()
                        {
                            TotalPrice = newTotalPriceValue,
                            Price = newPriceValue
                        }
                    };
                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.LineItems);
                    Assert.Equal(LineItemPriceMode.ExternalTotal,
                        updatedCart.LineItems[0].PriceMode);
                    Assert.Equal(newPriceValue,
                        updatedCart.LineItems[0].Price.Value);
                    Assert.Equal(newTotalPriceValue,
                        updatedCart.LineItems[0].TotalPrice);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartSetLineItemCustomType()
        {
            var quantity = 5;
            await WithType(client, async type =>
            {
                await WithUpdateableCartWithSingleLineItem(client, quantity,
                    cartDraft => DefaultCartDraftWithTaxMode(cartDraft, TaxMode.ExternalAmount),
                    async cart =>
                    {
                        Assert.Single(cart.LineItems);
                        var fields = CreateNewFields();

                        var lineItem = cart.LineItems[0];
                        var action = new SetLineItemCustomTypeUpdateAction
                        {
                            LineItemId = lineItem.Id,
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };

                        var updatedCart = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.Single(updatedCart.LineItems);
                        Assert.Equal(type.Id, updatedCart.LineItems[0].Custom.Type.Id);

                        return updatedCart;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartSetLineItemCustomField()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithProduct(client,
                    productDraft =>
                        DefaultProductDraftWithTaxCategory(productDraft, taxCategory),
                    async product =>
                    {
                        await WithType(client, async type =>
                        {
                            var fields = CreateNewFields();
                            var customFieldsDraft = new CustomFieldsDraft
                            {
                                Type = type.ToKeyResourceIdentifier(),
                                Fields = fields
                            };
                            var lineItemDraft = new LineItemDraft
                            {
                                Sku = product.MasterData.Staged.MasterVariant.Sku,
                                Quantity = 5,
                                Custom = customFieldsDraft
                            };
                            await WithUpdateableCart(client,
                                cartDraft => DefaultCartDraftWithLineItem(cartDraft, lineItemDraft),
                                async cart =>
                                {
                                    Assert.Single(cart.LineItems);

                                    var lineItem = cart.LineItems[0];
                                    Assert.NotNull(lineItem.Custom);
                                    Assert.Equal(type.Id, lineItem.Custom.Type.Id);

                                    var stringFieldValue = TestingUtility.RandomString();

                                    var action = new SetLineItemCustomFieldUpdateAction
                                    {
                                        Name = "string-field",
                                        Value = stringFieldValue,
                                        LineItemId = lineItem.Id
                                    };

                                    var updatedCart = await client
                                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                                    Assert.Single(updatedCart.LineItems);
                                    Assert.Equal(stringFieldValue,
                                        updatedCart.LineItems[0].Custom.Fields["string-field"]);

                                    return updatedCart;
                                });
                        });
                    });
            });
        }

        [Fact]
        public async Task UpdateCartSetLineItemShippingDetails()
        {
            var quantity = 5;
            var addresses = new List<Address> {TestingUtility.GetRandomAddress()};
            await WithUpdateableCartWithSingleLineItem(client, quantity,
                cartDraft => DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Single(cart.ItemShippingAddresses);

                    var lineItemId = cart.LineItems[0].Id;
                    var addressKey = cart.ItemShippingAddresses[0].Key;

                    var itemShippingDetailsDraft =
                        TestingUtility.GetItemShippingDetailsDraft(addressKey, cart.LineItems[0].Quantity);

                    var action = new SetLineItemShippingDetailsUpdateAction
                    {
                        LineItemId = lineItemId,
                        ShippingDetails = itemShippingDetailsDraft
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.LineItems);
                    Assert.NotNull(updatedCart.LineItems[0].ShippingDetails);
                    Assert.Single(updatedCart.LineItems[0].ShippingDetails.Targets);
                    Assert.Equal(itemShippingDetailsDraft.Targets[0].Quantity,
                        updatedCart.LineItems[0].ShippingDetails.Targets[0].Quantity);
                    Assert.Equal(itemShippingDetailsDraft.Targets[0].AddressKey,
                        updatedCart.LineItems[0].ShippingDetails.Targets[0].AddressKey);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartApplyDeltaToLineItemShippingDetailsTargets()
        {
            var quantity = 5;
            var addresses = new List<Address> {TestingUtility.GetRandomAddress()};
            await WithUpdateableCartWithSingleLineItem(client, quantity,
                cartDraft => DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    Assert.Single(cart.LineItems);
                    Assert.Single(cart.ItemShippingAddresses);

                    var lineItemId = cart.LineItems[0].Id;
                    var addressKey = cart.ItemShippingAddresses[0].Key;

                    var targetsDelta = TestingUtility.GetTargetsDelta(addressKey, cart.LineItems[0].Quantity);

                    var action = new ApplyDeltaToLineItemShippingDetailsTargetsUpdateAction
                    {
                        LineItemId = lineItemId,
                        TargetsDelta = targetsDelta
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.LineItems);
                    Assert.NotNull(updatedCart.LineItems[0].ShippingDetails);
                    Assert.Single(updatedCart.LineItems[0].ShippingDetails.Targets);
                    Assert.Equal(targetsDelta[0].Quantity,
                        updatedCart.LineItems[0].ShippingDetails.Targets[0].Quantity);
                    Assert.Equal(targetsDelta[0].AddressKey,
                        updatedCart.LineItems[0].ShippingDetails.Targets[0].AddressKey);

                    return updatedCart;
                });
        }

        #endregion

        #region UpdateActionsOnCustomLineItem

        [Fact]
        public async Task UpdateCartAddCustomLineItem()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithUpdateableCart(client, async cart =>
                {
                    Assert.Empty(cart.CustomLineItems);

                    var customLineItem = TestingUtility.GetCustomLineItemDraft(taxCategory);

                    var action =
                        new AddCustomLineItemUpdateAction()
                        {
                            Name = customLineItem.Name,
                            Slug = customLineItem.Slug,
                            Quantity = customLineItem.Quantity,
                            Money = customLineItem.Money,
                            TaxCategory = customLineItem.TaxCategory.ToReference()
                        };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.CustomLineItems);
                    Assert.Equal(customLineItem.Name["en"], updatedCart.CustomLineItems[0].Name["en"]);
                    Assert.Equal(customLineItem.Slug, updatedCart.CustomLineItems[0].Slug);
                    return updatedCart;
                });
            });
        }

        [Fact]
        public async Task UpdateCartRemoveCustomLineItem()
        {
            await WithUpdateableCartWithSingleCustomLineItem(client, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.CustomLineItems);
                    var customLineItem = cart.CustomLineItems[0];

                    var action = new RemoveCustomLineItemUpdateAction
                    {
                        CustomLineItemId = customLineItem.Id
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Empty(updatedCart.CustomLineItems);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartChangeCustomLineItemQuantity()
        {
            await WithUpdateableCartWithSingleCustomLineItem(client, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.CustomLineItems);
                    var customLineItem = cart.CustomLineItems[0];

                    var newQuantity = cart.CustomLineItems[0].Quantity + 2;
                    var action = new ChangeCustomLineItemQuantityUpdateAction
                    {
                        CustomLineItemId = customLineItem.Id,
                        Quantity = newQuantity
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.CustomLineItems);
                    Assert.Equal(newQuantity, updatedCart.CustomLineItems[0].Quantity);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartChangeCustomLineItemMoney()
        {
            await WithUpdateableCartWithSingleCustomLineItem(client, DefaultCartDraft,
                async cart =>
                {
                    Assert.Single(cart.CustomLineItems);
                    var customLineItem = cart.CustomLineItems[0];

                    var newMoney = TestingUtility.MultiplyMoney(cart.CustomLineItems[0].Money, 2);
                    var action = new ChangeCustomLineItemMoneyUpdateAction
                    {
                        CustomLineItemId = customLineItem.Id,
                        Money = newMoney
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.CustomLineItems);
                    Assert.Equal(newMoney, updatedCart.CustomLineItems[0].Money);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartSetCustomLineItemCustomType()
        {
            await WithType(client, async type =>
            {
                await WithUpdateableCartWithSingleCustomLineItem(client, DefaultCartDraft,
                    async cart =>
                    {
                        Assert.Single(cart.CustomLineItems);
                        var customLineItem = cart.CustomLineItems[0];
                        var fields = CreateNewFields();

                        var action = new SetCustomLineItemCustomTypeUpdateAction
                        {
                            CustomLineItemId = customLineItem.Id,
                            Fields = fields,
                            Type = type.ToKeyResourceIdentifier()
                        };

                        var updatedCart = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                        Assert.Single(updatedCart.CustomLineItems);
                        Assert.Equal(type.Id, updatedCart.CustomLineItems[0].Custom.Type.Id);

                        return updatedCart;
                    });
            });
        }

        [Fact]
        public async Task UpdateCartSetCustomLineItemCustomField()
        {
            await WithTaxCategory(client, async taxCategory =>
            {
                await WithType(client, async type =>
                {
                    var fields = CreateNewFields();
                    var customFieldsDraft = new CustomFieldsDraft
                    {
                        Type = type.ToKeyResourceIdentifier(),
                        Fields = fields
                    };
                    var customLineItemDraft = TestingUtility.GetCustomLineItemDraft(taxCategory);
                    customLineItemDraft.Custom = customFieldsDraft;

                    await WithUpdateableCart(client,
                        draft => DefaultCartDraftWithCustomLineItem(draft, customLineItemDraft),
                        async cart =>
                        {
                            Assert.Single(cart.CustomLineItems);
                            var customLineItem = cart.CustomLineItems[0];

                            Assert.NotNull(customLineItem.Custom);
                            Assert.Equal(type.Id, customLineItem.Custom.Type.Id);

                            var stringFieldValue = TestingUtility.RandomString();
                            var action = new SetCustomLineItemCustomFieldUpdateAction
                            {
                                Name = "string-field",
                                Value = stringFieldValue,
                                CustomLineItemId = customLineItem.Id
                            };

                            var updatedCart = await client
                                .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.Single(updatedCart.CustomLineItems);
                            Assert.Equal(type.Id, updatedCart.CustomLineItems[0].Custom.Type.Id);
                            Assert.Equal(stringFieldValue,
                                updatedCart.CustomLineItems[0].Custom.Fields["string-field"]);

                            return updatedCart;
                        });
                });
            });
        }

        [Fact]
        public async Task UpdateCartSetCustomLineItemTaxRate()
        {
            await WithUpdateableCartWithSingleCustomLineItem(client,
                cartDraft => DefaultCartDraftWithTaxMode(cartDraft, TaxMode.External),
                async cart =>
                {
                    Assert.Equal(TaxMode.External, cart.TaxMode);
                    Assert.Single(cart.CustomLineItems);
                    var customLineItem = cart.CustomLineItems[0];

                    var externalTaxRateDraft = TestingUtility.GetExternalTaxRateDraft();
                    var action = new SetCustomLineItemTaxRateUpdateAction
                    {
                        CustomLineItemId = customLineItem.Id,
                        ExternalTaxRate = externalTaxRateDraft
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.CustomLineItems);
                    Assert.Equal(externalTaxRateDraft.Name, updatedCart.CustomLineItems[0].TaxRate.Name);
                    Assert.Equal(externalTaxRateDraft.Amount,
                        updatedCart.CustomLineItems[0].TaxRate.Amount);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartSetCustomLineItemTaxAmount()
        {
            await WithUpdateableCartWithSingleCustomLineItem(client,
                cartDraft => DefaultCartDraftWithTaxMode(cartDraft, TaxMode.ExternalAmount),
                async cart =>
                {
                    Assert.Equal(TaxMode.ExternalAmount, cart.TaxMode);
                    Assert.Single(cart.CustomLineItems);
                    var customLineItem = cart.CustomLineItems[0];

                    var externalTaxAmountDraft = TestingUtility.GetExternalTaxAmountDraft();
                    var action = new SetCustomLineItemTaxAmountUpdateAction
                    {
                        CustomLineItemId = customLineItem.Id,
                        ExternalTaxAmount = externalTaxAmountDraft
                    };
                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.CustomLineItems);
                    Assert.Equal(externalTaxAmountDraft.TotalGross,
                        updatedCart.CustomLineItems[0].TaxedPrice.TotalGross);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartSetCustomLineItemShippingDetails()
        {
            var addresses = new List<Address> {TestingUtility.GetRandomAddress()};
            await WithUpdateableCartWithSingleCustomLineItem(client,
                cartDraft => DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    Assert.Single(cart.CustomLineItems);
                    Assert.Single(cart.ItemShippingAddresses);

                    var customLineItemId = cart.CustomLineItems[0].Id;
                    var addressKey = cart.ItemShippingAddresses[0].Key;

                    var itemShippingDetailsDraft =
                        TestingUtility.GetItemShippingDetailsDraft(addressKey, cart.CustomLineItems[0].Quantity);

                    var action = new SetCustomLineItemShippingDetailsUpdateAction
                    {
                        CustomLineItemId = customLineItemId,
                        ShippingDetails = itemShippingDetailsDraft
                    };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.CustomLineItems);
                    Assert.NotNull(updatedCart.CustomLineItems[0].ShippingDetails);
                    Assert.Single(updatedCart.CustomLineItems[0].ShippingDetails.Targets);
                    Assert.Equal(itemShippingDetailsDraft.Targets[0].Quantity,
                        updatedCart.CustomLineItems[0].ShippingDetails.Targets[0].Quantity);
                    Assert.Equal(itemShippingDetailsDraft.Targets[0].AddressKey,
                        updatedCart.CustomLineItems[0].ShippingDetails.Targets[0].AddressKey);

                    return updatedCart;
                });
        }

        [Fact]
        public async Task UpdateCartApplyDeltaToCustomLineItemShippingDetailsTargets()
        {
            var addresses = new List<Address> {TestingUtility.GetRandomAddress()};
            await WithUpdateableCartWithSingleCustomLineItem(client,
                cartDraft => DefaultCartDraftWithItemShippingAddresses(cartDraft, addresses),
                async cart =>
                {
                    Assert.Single(cart.CustomLineItems);
                    Assert.Single(cart.ItemShippingAddresses);

                    var customLineItemId = cart.CustomLineItems[0].Id;
                    var addressKey = cart.ItemShippingAddresses[0].Key;

                    var targetsDelta = TestingUtility.GetTargetsDelta(addressKey, cart.CustomLineItems[0].Quantity);
                    var action =
                        new ApplyDeltaToCustomLineItemShippingDetailsTargetsUpdateAction
                        {
                            CustomLineItemId = customLineItemId,
                            TargetsDelta = targetsDelta
                        };

                    var updatedCart = await client
                        .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedCart.CustomLineItems);
                    Assert.NotNull(updatedCart.CustomLineItems[0].ShippingDetails);
                    Assert.Single(updatedCart.CustomLineItems[0].ShippingDetails.Targets);
                    Assert.Equal(targetsDelta[0].Quantity,
                        updatedCart.CustomLineItems[0].ShippingDetails.Targets[0].Quantity);
                    Assert.Equal(targetsDelta[0].AddressKey,
                        updatedCart.CustomLineItems[0].ShippingDetails.Targets[0].AddressKey);

                    return updatedCart;
                });
        }

        #endregion
    }
}