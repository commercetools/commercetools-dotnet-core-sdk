using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Carts.UpdateActions;
using commercetools.Sdk.Domain.CustomObjects;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Projects;
using commercetools.Sdk.Domain.Projects.UpdateActions;
using commercetools.Sdk.Domain.ShippingMethods;
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
using static commercetools.Sdk.IntegrationTests.Projects.ProjectFixture;

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

        //Need to check replicateCartFromOrder
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


        [Fact]
        public async void UpdateCartSetShippingRateInputAsScore()
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
                        var secondScorePrice = (shippingRateDraft.Tiers[1] as CartScoreShippingRatePriceTier)?.Price;

                        var setCustomShippingMethodAction = new SetCustomShippingMethodUpdateAction()
                        {
                            ShippingMethodName = customShippingMethod,
                            ShippingRate = shippingRateDraft, //with shipping rate price tiers
                            ExternalTaxRate = externalTaxRateDraft,
                            TaxCategory = null
                        };

                        var cartWithShippingMethod = await client
                            .ExecuteAsync(cart.UpdateById(actions => actions.AddUpdate(setCustomShippingMethodAction)));
                        Assert.NotNull(cartWithShippingMethod.ShippingInfo);
                        Assert.Equal(customShippingMethod, cartWithShippingMethod.ShippingInfo.ShippingMethodName);

                        var setShippingRateInputAction = new SetShippingRateInputUpdateAction
                        {
                            ShippingRateInput = new ScoreShippingRateInputDraft {Score = 1}
                        };
                        var cartWithShippingMethodWithScore1 = await client
                            .ExecuteAsync(cartWithShippingMethod.UpdateById(actions => actions.AddUpdate(setShippingRateInputAction)));

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
                            Assert.Equal(customShippingMethod, cartWithShippingMethod.ShippingInfo.ShippingMethodName);

                            var setShippingRateInputAction = new SetShippingRateInputUpdateAction
                            {
                                ShippingRateInput = new ClassificationShippingRateInputDraft {Key = "Small"}
                            };
                            var cartWithShippingMethodWithSmallClassification = await client
                                .ExecuteAsync(cartWithShippingMethod.UpdateById(actions =>
                                    actions.AddUpdate(setShippingRateInputAction)));

                            Assert.NotNull(cartWithShippingMethodWithSmallClassification.ShippingRateInput);
                            Assert.IsType<ClassificationShippingRateInput>(cartWithShippingMethodWithSmallClassification
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

        #endregion
    }
}