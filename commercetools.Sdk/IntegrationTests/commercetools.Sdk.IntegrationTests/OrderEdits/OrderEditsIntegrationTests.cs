using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts.UpdateActions;
using commercetools.Sdk.Domain.OrderEdits;
using commercetools.Sdk.Domain.OrderEdits.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Orders.OrdersFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using static commercetools.Sdk.IntegrationTests.OrderEdits.OrderEditsFixture;
using static commercetools.Sdk.IntegrationTests.Projects.ProjectFixture;
using static commercetools.Sdk.IntegrationTests.Products.ProductsFixture;
using static commercetools.Sdk.IntegrationTests.Payments.PaymentsFixture;
using AddPaymentUpdateAction = commercetools.Sdk.Domain.Orders.UpdateActions.AddPaymentUpdateAction;
using SetCustomerEmailUpdateAction = commercetools.Sdk.Domain.Orders.UpdateActions.SetCustomerEmailUpdateAction;
using SetCustomFieldUpdateAction = commercetools.Sdk.Domain.OrderEdits.UpdateActions.SetCustomFieldUpdateAction;
using SetCustomTypeUpdateAction = commercetools.Sdk.Domain.OrderEdits.UpdateActions.SetCustomTypeUpdateAction;
using SetKeyUpdateAction = commercetools.Sdk.Domain.OrderEdits.UpdateActions.SetKeyUpdateAction;
using SetLocaleUpdateAction = commercetools.Sdk.Domain.Orders.UpdateActions.SetLocaleUpdateAction;

namespace commercetools.Sdk.IntegrationTests.OrderEdits
{
    [Collection("Integration Tests")]
    public class OrderEditsIntegrationTests
    {
        private readonly IClient client;

        public OrderEditsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async void CreateOrderEdit()
        {
            var email = "john.doe@commercetools.de";
            await WithSimpleOrder(client, async order =>
            {
                await WithOrderEdit(client, draft =>
                    {
                        var orderEditDraft = DefaultOrderEditDraft(draft, order);
                        var action = new SetCustomerEmailUpdateAction
                        {
                            Email = email
                        };
                        orderEditDraft.StagedActions.Add(action);
                        return orderEditDraft;
                    },
                    orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.NotNull(orderEdit.Result);
                        var previewSuccessResult = orderEdit.Result as OrderEditPreviewSuccess;
                        Assert.NotNull(previewSuccessResult);
                        Assert.Equal(email, previewSuccessResult.Preview.CustomerEmail);
                    });
            });
        }

        [Fact]
        public async void GetOrderEditById()
        {
            var key = TestingUtility.RandomString();

            await WithSimpleOrder(client, async order =>
            {
                await WithOrderEdit(client,
                    draft =>
                    {
                        var orderEditDraft = DefaultOrderEditDraftWithStagedAction(draft, order);
                        orderEditDraft.Key = key;
                        return orderEditDraft;
                    },
                    async orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.Single(orderEdit.StagedActions);
                        var retrievedOrderEdit = await client
                            .ExecuteAsync(orderEdit.ToIdResourceIdentifier().GetById());
                        Assert.Equal(key, retrievedOrderEdit.Key);
                    });
            });
        }

        [Fact]
        public async void GetOrderEditByKey()
        {
            var key = TestingUtility.RandomString();
            await WithSimpleOrder(client, async order =>
            {
                await WithOrderEdit(client,
                    draft =>
                    {
                        var orderEditDraft = DefaultOrderEditDraftWithStagedAction(draft, order);
                        orderEditDraft.Key = key;
                        return orderEditDraft;
                    },
                    async orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.Single(orderEdit.StagedActions);

                        var retrievedOrderEdit = await client
                            .ExecuteAsync(orderEdit.ToKeyResourceIdentifier().GetByKey());
                        Assert.Equal(key, retrievedOrderEdit.Key);
                    });
            });
        }

        [Fact]
        public async void QueryOrderEdits()
        {
            var key = TestingUtility.RandomString();
            await WithSimpleOrder(client, async order =>
            {
                await WithOrderEdit(client,
                    draft =>
                    {
                        var orderEditDraft = DefaultOrderEditDraftWithStagedAction(draft, order);
                        orderEditDraft.Key = key;
                        return orderEditDraft;
                    },
                    async orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.Single(orderEdit.StagedActions);

                        var queryCommand = new QueryCommand<OrderEdit>();
                        queryCommand.Where(o => o.Key == orderEdit.Key.valueOf());
                        var returnedSet = await client.ExecuteAsync(queryCommand);
                        Assert.Single(returnedSet.Results);
                        Assert.Equal(key, returnedSet.Results[0].Key);
                    });
            });
        }

        [Fact]
        public async void DeleteOrderEditById()
        {
            await WithSimpleOrder(client, async order =>
            {
                await WithOrderEdit(client,
                    draft => DefaultOrderEditDraftWithStagedAction(draft, order),
                    async orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.Single(orderEdit.StagedActions);

                        await client.ExecuteAsync(orderEdit.DeleteById());
                        await Assert.ThrowsAsync<NotFoundException>(
                            () => client.ExecuteAsync(new GetByIdCommand<OrderEdit>(orderEdit))
                        );
                    });
            });
        }

        [Fact]
        public async void DeleteOrderEditByKey()
        {
            await WithSimpleOrder(client, async order =>
            {
                await WithOrderEdit(client, draft => DefaultOrderEditDraftWithStagedAction(draft, order),
                    async orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.Single(orderEdit.StagedActions);

                        await client.ExecuteAsync(orderEdit.DeleteByKey());
                        await Assert.ThrowsAsync<NotFoundException>(
                            () => client.ExecuteAsync(new GetByIdCommand<OrderEdit>(orderEdit))
                        );
                    });
            });
        }

        [Fact]
        public async void ApplyOrderEditSetCustomerEmail()
        {
            var email = "john.doe@commercetools.de";
            await WithUpdateableSimpleOrder(client, async order =>
            {
                Assert.Null(order.CustomerEmail);

                //create orderEdit with stagedAction SetCustomerEmailStagedAction
                await WithUpdateableOrderEdit(client, draft =>
                    {
                        var orderEditDraft = DefaultOrderEditDraft(draft, order);
                        var action = new SetCustomerEmailUpdateAction
                        {
                            Email = email
                        };
                        orderEditDraft.StagedActions.Add(action);
                        return orderEditDraft;
                    },
                    async orderEdit =>
                    {
                        var retrievedOrderEdit = await client
                            .ExecuteAsync(orderEdit.ToIdResourceIdentifier().GetById().Expand(oe => oe.Resource));

                        Assert.NotNull(retrievedOrderEdit);
                        Assert.NotNull(retrievedOrderEdit.Resource.Obj);

                        //Apply OrderEdit
                        var applyOrderEditCommand = new ApplyOrderEditCommand(retrievedOrderEdit,
                            retrievedOrderEdit.Resource.Obj.Version);

                        var appliedOrderEdit = await client.ExecuteAsync(applyOrderEditCommand);

                        Assert.NotNull(appliedOrderEdit.Result);
                        Assert.IsType<OrderEditApplied>(appliedOrderEdit.Result);
                        return appliedOrderEdit;
                    });

                //then retrieved updated order
                var updatedOrder = await client
                    .ExecuteAsync(order.ToIdResourceIdentifier().GetById());

                Assert.NotNull(updatedOrder.CustomerEmail);
                Assert.Equal(email, updatedOrder.CustomerEmail);
                return updatedOrder;
            });
        }

        [Fact]
        public async void ApplyOrderEditAddLineItem()
        {
            string addedItemSku = "";
            var addedItemQuantity = 5;

            await WithUpdateableSimpleOrder(client, async order =>
            {
                Assert.Single(order.LineItems);
                var taxCategoryRef = await GetProductTaxCategory(client, order.LineItems[0].ProductId);

                await WithProduct(client,
                    draft =>
                    {
                        var productDraft = DefaultProductDraft(draft);
                        productDraft.TaxCategory = taxCategoryRef;
                        return productDraft;
                    },
                    async product =>
                    {
                        Assert.NotNull(product);
                        addedItemSku = product.MasterData.Staged.MasterVariant.Sku;
                        //create orderEdit with stagedAction AddLineItemStagedAction
                        await WithUpdateableOrderEdit(client, draft =>
                            {
                                var orderEditDraft = DefaultOrderEditDraft(draft, order);
                                //Add a new lineItem
                                var action = new AddLineItemUpdateAction
                                {
                                    Sku = addedItemSku,
                                    Quantity = addedItemQuantity
                                };
                                orderEditDraft.StagedActions.Add(action);
                                return orderEditDraft;
                            },
                            async orderEdit =>
                            {
                                var retrievedOrderEdit = await client
                                    .ExecuteAsync(
                                        orderEdit.ToIdResourceIdentifier().GetById().Expand(oe => oe.Resource));

                                Assert.NotNull(retrievedOrderEdit);
                                Assert.NotNull(retrievedOrderEdit.Resource.Obj);

                                //Apply OrderEdit
                                var applyOrderEditCommand = new ApplyOrderEditCommand(retrievedOrderEdit,
                                    retrievedOrderEdit.Resource.Obj.Version);

                                var appliedOrderEdit = await client.ExecuteAsync(applyOrderEditCommand);

                                Assert.NotNull(appliedOrderEdit.Result);
                                Assert.IsType<OrderEditApplied>(appliedOrderEdit.Result);
                                return appliedOrderEdit;
                            });
                    });
                //then retrieved updated order
                var updatedOrder = await client
                    .ExecuteAsync(order.ToIdResourceIdentifier().GetById());

                //assert that order now with 2 lineItems
                Assert.Equal(2, updatedOrder.LineItems.Count);
                Assert.Contains(updatedOrder.LineItems,
                    item => item.Variant.Sku == addedItemSku
                            && item.Quantity == addedItemQuantity);
                return updatedOrder;
            });
        }

        [Fact]
        public async void ApplyOrderEditAddPayment()
        {
            await WithPayment(client, async payment =>
            {
                Assert.NotNull(payment);
                await WithUpdateableSimpleOrder(client, async order =>
                {
                    Assert.Null(order.PaymentInfo);

                    //create orderEdit with stagedAction AddPaymentStagedAction
                    await WithUpdateableOrderEdit(client, draft =>
                        {
                            var orderEditDraft = DefaultOrderEditDraft(draft, order);
                            var action = new AddPaymentUpdateAction
                            {
                                Payment = payment.ToKeyResourceIdentifier()
                            };
                            orderEditDraft.StagedActions.Add(action);
                            return orderEditDraft;
                        },
                        async orderEdit =>
                        {
                            var retrievedOrderEdit = await client
                                .ExecuteAsync(orderEdit.ToIdResourceIdentifier().GetById().Expand(oe => oe.Resource));

                            Assert.NotNull(retrievedOrderEdit);
                            Assert.NotNull(retrievedOrderEdit.Resource.Obj);

                            //Apply OrderEdit
                            var applyOrderEditCommand = new ApplyOrderEditCommand(retrievedOrderEdit,
                                retrievedOrderEdit.Resource.Obj.Version);

                            var appliedOrderEdit = await client.ExecuteAsync(applyOrderEditCommand);

                            Assert.NotNull(appliedOrderEdit.Result);
                            Assert.IsType<OrderEditApplied>(appliedOrderEdit.Result);
                            return appliedOrderEdit;
                        });


                    //then retrieved updated order
                    var updatedOrder = await client
                        .ExecuteAsync(order.ToIdResourceIdentifier().GetById());

                    Assert.NotNull(updatedOrder.PaymentInfo);
                    Assert.Single(updatedOrder.PaymentInfo.Payments);
                    Assert.Equal(payment.Id, updatedOrder.PaymentInfo.Payments[0].Id);
                    return updatedOrder;
                });
            });
        }

        #region UpdateActions

        [Fact]
        public async void UpdateOrderEditSetCommentByKey()
        {
            await WithSimpleOrder(client, async order =>
            {
                await WithUpdateableOrderEdit(client,
                    draft => DefaultOrderEditDraftWithStagedAction(draft, order),
                    async orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.Single(orderEdit.StagedActions);

                        var comment = TestingUtility.RandomString();
                        var action = new SetCommentUpdateAction
                        {
                            Comment = comment
                        };

                        var updatedOrderEdit = await client
                            .ExecuteAsync(orderEdit.UpdateByKey(
                                actions => actions.AddUpdate(action)));

                        Assert.Equal(comment, updatedOrderEdit.Comment);
                        return updatedOrderEdit;
                    });
            });
        }

        [Fact]
        public async void UpdateOrderEditSetKey()
        {
            await WithSimpleOrder(client, async order =>
            {
                await WithUpdateableOrderEdit(client,
                    draft => DefaultOrderEditDraftWithStagedAction(draft, order),
                    async orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.Single(orderEdit.StagedActions);

                        var key = TestingUtility.RandomString();
                        var action = new SetKeyUpdateAction {Key = key};

                        var updatedOrderEdit = await client
                            .ExecuteAsync(orderEdit.UpdateById(
                                actions => actions.AddUpdate(action)));

                        Assert.Equal(key, updatedOrderEdit.Key);
                        return updatedOrderEdit;
                    });
            });
        }

        [Fact]
        public async void UpdateOrderEditSetStagedActions()
        {
            await WithSimpleOrder(client, async order =>
            {
                await WithUpdateableOrderEdit(client,
                    draft => DefaultOrderEditDraftWithStagedAction(draft, order),
                    async orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.Single(orderEdit.StagedActions);
                        Assert.IsType<SetCustomerEmailUpdateAction>(orderEdit.StagedActions[0]);

                        var projectLanguages = GetProjectLanguages(client);
                        Assert.True(projectLanguages.Count > 0); //make sure that project has at least one language

                        var locale = projectLanguages[0];
                        var stagedActions = new List<IStagedOrderUpdateAction>
                        {
                            new SetLocaleUpdateAction
                            {
                                Locale = locale
                            }
                        };
                        var action = new SetStagedActionsUpdateAction
                        {
                            StagedActions = stagedActions
                        };

                        var updatedOrderEdit = await client
                            .ExecuteAsync(orderEdit.UpdateById(
                                actions => actions.AddUpdate(action)));

                        Assert.Single(updatedOrderEdit.StagedActions);
                        Assert.IsType<SetLocaleUpdateAction>(updatedOrderEdit.StagedActions[0]);
                        return updatedOrderEdit;
                    });
            });
        }

        [Fact]
        public async void UpdateOrderEditAddStagedAction()
        {
            await WithSimpleOrder(client, async order =>
            {
                await WithUpdateableOrderEdit(client,
                    draft => DefaultOrderEditDraftWithStagedAction(draft, order),
                    async orderEdit =>
                    {
                        Assert.NotNull(orderEdit);
                        Assert.Single(orderEdit.StagedActions);
                        Assert.IsType<SetCustomerEmailUpdateAction>(orderEdit.StagedActions[0]);

                        var projectLanguages = GetProjectLanguages(client);
                        Assert.True(projectLanguages.Count > 0); //make sure that project has at least one language

                        var locale = projectLanguages[0];
                        var stagedAction = new SetLocaleUpdateAction
                        {
                            Locale = locale
                        };
                        var action = new AddStagedActionUpdateAction
                        {
                            StagedAction = stagedAction
                        };

                        var updatedOrderEdit = await client
                            .ExecuteAsync(orderEdit.UpdateById(
                                actions => actions.AddUpdate(action)));

                        Assert.Equal(2, updatedOrderEdit.StagedActions.Count);
                        Assert.Contains(updatedOrderEdit.StagedActions,
                            a => a.GetType() == typeof(SetLocaleUpdateAction));
                        return updatedOrderEdit;
                    });
            });
        }

        [Fact]
        public async void UpdateOrderEditSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithSimpleOrder(client, async order =>
                {
                    await WithUpdateableOrderEdit(client,
                        draft => DefaultOrderEditDraftWithStagedAction(draft, order),
                        async orderEdit =>
                        {
                            Assert.NotNull(orderEdit);
                            Assert.Single(orderEdit.StagedActions);

                            var action = new SetCustomTypeUpdateAction
                            {
                                Type = type.ToKeyResourceIdentifier(),
                                Fields = fields
                            };

                            var updatedOrderEdit = await client
                                .ExecuteAsync(orderEdit.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Equal(type.Id, updatedOrderEdit.Custom.Type.Id);
                            return updatedOrderEdit;
                        });
                });
            });
        }

        [Fact]
        public async void UpdateOrderEditSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString();

            await WithType(client, async type =>
            {
                await WithSimpleOrder(client, async order =>
                {
                    await WithUpdateableOrderEdit(client,
                        draft => DefaultOrderEditDraftWithStagedAction(draft, order),
                        async orderEdit =>
                        {
                            Assert.NotNull(orderEdit);
                            Assert.Single(orderEdit.StagedActions);

                            var action = new SetCustomTypeUpdateAction
                            {
                                Type = type.ToKeyResourceIdentifier(),
                                Fields = fields
                            };

                            var updatedOrderEdit = await client
                                .ExecuteAsync(orderEdit.UpdateById(
                                    actions => actions.AddUpdate(action)));

                            Assert.Equal(type.Id, updatedOrderEdit.Custom.Type.Id);

                            //then set the custom field
                            var setFieldAction = new SetCustomFieldUpdateAction()
                            {
                                Name = "string-field", Value = newValue
                            };
                            var updatedOrderEditWithUpdatedCustomField = await client
                                .ExecuteAsync(updatedOrderEdit.UpdateById(
                                    actions => actions.AddUpdate(setFieldAction)));

                            Assert.Equal(newValue,
                                updatedOrderEditWithUpdatedCustomField.Custom.Fields["string-field"]);

                            return updatedOrderEditWithUpdatedCustomField;
                        });
                });
            });
        }

        #endregion
    }
}