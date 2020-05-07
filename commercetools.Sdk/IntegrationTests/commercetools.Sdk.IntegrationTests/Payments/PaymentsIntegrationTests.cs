using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Payments.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Payments.PaymentsFixture;
using static commercetools.Sdk.IntegrationTests.Customers.CustomersFixture;
using static commercetools.Sdk.IntegrationTests.States.StatesFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;

namespace commercetools.Sdk.IntegrationTests.Payments
{
    [Collection("Integration Tests")]
    public class PaymentsIntegrationTests
    {
        private readonly IClient client;

        public PaymentsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreatePayment()
        {
            var key = $"CreatePayment-{TestingUtility.RandomString()}";
            await WithPayment(
                client, paymentDraft => DefaultPaymentDraftWithKey(paymentDraft, key),
                payment => { Assert.Equal(key, payment.Key); });
        }

        [Fact]
        public async Task GetPaymentById()
        {
            var key = $"GetPaymentById-{TestingUtility.RandomString()}";
            await WithPayment(
                client, paymentDraft => DefaultPaymentDraftWithKey(paymentDraft, key),
                async payment =>
                {
                    var retrievedPayment = await client
                        .ExecuteAsync(payment.ToIdResourceIdentifier().GetById());
                    Assert.Equal(key, retrievedPayment.Key);
                });
        }

        [Fact]
        public async Task GetPaymentByKey()
        {
            var key = $"GetPaymentByKey-{TestingUtility.RandomString()}";
            await WithPayment(
                client, paymentDraft => DefaultPaymentDraftWithKey(paymentDraft, key),
                async payment =>
                {
                    var retrievedPayment = await client
                        .ExecuteAsync(payment.ToKeyResourceIdentifier().GetByKey());
                    Assert.Equal(key, retrievedPayment.Key);
                });
        }

        [Fact]
        public async Task QueryPayments()
        {
            var key = $"QueryPayments-{TestingUtility.RandomString()}";
            await WithPayment(
                client, paymentDraft => DefaultPaymentDraftWithKey(paymentDraft, key),
                async payment =>
                {
                    var queryCommand = new QueryCommand<Payment>();
                    queryCommand.Where(p => p.Key == payment.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeletePaymentById()
        {
            var key = $"DeletePaymentById-{TestingUtility.RandomString()}";
            await WithPayment(
                client, paymentDraft => DefaultPaymentDraftWithKey(paymentDraft, key),
                async payment =>
                {
                    await client.ExecuteAsync(payment.DeleteById());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Payment>(payment))
                    );
                });
        }

        [Fact]
        public async Task DeletePaymentByKey()
        {
            var key = $"DeletePaymentByKey-{TestingUtility.RandomString()}";
            await WithPayment(
                client, paymentDraft => DefaultPaymentDraftWithKey(paymentDraft, key),
                async payment =>
                {
                    await client.ExecuteAsync(payment.DeleteByKey());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Payment>(payment))
                    );
                });
        }
        
        [Fact]
        public async Task CreatePaymentWithCustomer()
        {
            await WithCustomer(client, async customer =>
            {
                await WithPayment(
                    client, 
                    paymentDraft => DefaultPaymentDraftWithCustomer(paymentDraft, customer),
                    payment =>
                    {
                        Assert.NotNull(payment);
                        Assert.Equal(customer.Id, payment.Customer.Id);
                    });
            });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdatePaymentByIdSetKey()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                var key = TestingUtility.RandomString();
                var action = new SetKeyUpdateAction {Key = key};

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(key, updatedPayment.Key);
                return updatedPayment;
            });
        }

        [Fact]
        public async Task UpdatePaymentByKeyChangeAmountPlanned()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                var newAmountPlanned = Money.FromDecimal("EUR", TestingUtility.RandomInt(100, 10000));
                var action = new ChangeAmountPlannedUpdateAction {Amount = newAmountPlanned};

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(newAmountPlanned, updatedPayment.AmountPlanned);
                return updatedPayment;
            });
        }

        [Fact]
        public async Task UpdatePaymentByKeySetCustomer()
        {
            await WithCustomer(client, async customer =>
            {
                await WithUpdateablePayment(client, async payment =>
                {
                    Assert.Null(payment.Customer);
                    var action = new SetCustomerUpdateAction
                    {
                        Customer = customer.ToReference()
                    };

                    var updatedPayment = await client
                        .ExecuteAsync(payment.UpdateByKey(actions => actions.AddUpdate(action)));

                    Assert.Equal(customer.Id, updatedPayment.Customer.Id);
                    return updatedPayment;
                });
            });
        }

        [Fact]
        public async Task UpdatePaymentSetAnonymousId()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                var anonymousId = TestingUtility.RandomString();
                var action = new SetAnonymousIdUpdateAction {AnonymousId = anonymousId};

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(anonymousId, updatedPayment.AnonymousId);
                return updatedPayment;
            });
        }

        [Fact]
        public async Task UpdatePaymentSetInterfaceId()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                var interfaceId = TestingUtility.RandomString();
                var action = new SetInterfaceIdUpdateAction {InterfaceId = interfaceId};

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(interfaceId, updatedPayment.InterfaceId);
                return updatedPayment;
            });
        }

        [Fact]
        public async Task UpdatePaymentSetMethodInfoInterface()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                var paymentInterface = TestingUtility.RandomString();
                var action = new SetMethodInfoInterfaceUpdateAction {Interface = paymentInterface};

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                Assert.NotNull(updatedPayment.PaymentMethodInfo);
                Assert.Equal(paymentInterface, updatedPayment.PaymentMethodInfo.PaymentInterface);
                return updatedPayment;
            });
        }

        [Fact]
        public async Task UpdatePaymentSetMethodInfoMethod()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                var paymentMethod = TestingUtility.RandomString();
                var action = new SetMethodInfoMethodUpdateAction {Method = paymentMethod};

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                Assert.NotNull(updatedPayment.PaymentMethodInfo);
                Assert.Equal(paymentMethod, updatedPayment.PaymentMethodInfo.Method);
                return updatedPayment;
            });
        }

        [Fact]
        public async Task UpdatePaymentSetMethodInfoName()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                var name = TestingUtility.RandomString();
                var action = new SetMethodInfoNameUpdateAction
                {
                    Name = new LocalizedString {{"en", name}}
                };

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                Assert.NotNull(updatedPayment.PaymentMethodInfo);
                Assert.Equal(name, updatedPayment.PaymentMethodInfo.Name["en"]);
                return updatedPayment;
            });
        }

        [Fact]
        public async Task UpdatePaymentSetStatusInterfaceCode()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                var interfaceCode = TestingUtility.RandomString();
                var action = new SetStatusInterfaceCodeUpdateAction
                {
                    InterfaceCode = interfaceCode
                };

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(interfaceCode, updatedPayment.PaymentStatus.InterfaceCode);
                return updatedPayment;
            });
        }

        [Fact]
        public async Task UpdatePaymentSetStatusInterfaceText()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                var interfaceText = TestingUtility.RandomString();
                var action = new SetStatusInterfaceTextUpdateAction
                {
                    InterfaceText = interfaceText
                };

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(interfaceText, updatedPayment.PaymentStatus.InterfaceText);
                return updatedPayment;
            });
        }

        [Fact]
        public async Task UpdatePaymentTransitionToNewState()
        {
            await WithState(client, stateDraft => DefaultStateDraftWithType(stateDraft, StateType.PaymentState),
                async state =>
                {
                    await WithUpdateablePayment(client,
                        async payment =>
                        {
                            Assert.Null(payment.PaymentStatus.State);
                            var action = new TransitionStateUpdateAction
                            {
                                State = state.ToReference()
                            };

                            var updatedPayment = await client
                                .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                            Assert.NotNull(updatedPayment.PaymentStatus);
                            Assert.Equal(state.Id, updatedPayment.PaymentStatus.State.Id);
                            return updatedPayment;
                        });
                });
        }

        [Fact]
        public async Task UpdatePaymentAddTransaction()
        {
            await WithUpdateablePayment(client, async payment =>
            {
                Assert.Empty(payment.Transactions);
                var refundTransaction = new TransactionDraft
                {
                    Amount = Money.FromDecimal("EUR", TestingUtility.RandomInt(100, 10000)),
                    Type = TransactionType.Refund,
                    Timestamp = DateTime.Now,
                    State = TransactionState.Initial,
                    InteractionId = TestingUtility.RandomString()
                };
                var action = new AddTransactionUpdateAction
                {
                    Transaction = refundTransaction
                };

                var updatedPayment = await client
                    .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedPayment.Transactions);
                Assert.Equal(refundTransaction.InteractionId, updatedPayment.Transactions[0].InteractionId);
                return updatedPayment;
            });
        }
        
        [Fact]
        public async Task UpdatePaymentChangeTransactionState()
        {
            var oldState = TransactionState.Initial;
            var newState = TransactionState.Success;
            var refundTransaction = new TransactionDraft
            {
                Amount = Money.FromDecimal("EUR", TestingUtility.RandomInt(100, 10000)),
                Type = TransactionType.Refund,
                Timestamp = DateTime.Now,
                State = oldState,
                InteractionId = TestingUtility.RandomString()
            };
            
            await WithUpdateablePayment(client,
                paymentDraft => DefaultPaymentDraftWithTransaction(paymentDraft, refundTransaction),
                async payment =>
                {
                    Assert.Single(payment.Transactions);
                    Assert.Equal(oldState, payment.Transactions[0].State);
                    var action = new ChangeTransactionStateUpdateAction
                    {
                        TransactionId = payment.Transactions[0].Id,
                        State = newState
                    };

                    var updatedPayment = await client
                        .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedPayment.Transactions);
                    Assert.Equal(newState, updatedPayment.Transactions[0].State);
                    return updatedPayment;
                });
        }
        
        [Fact]
        public async Task UpdatePaymentChangeTransactionTimestamp()
        {
            var oldTimeStamp = DateTime.Now.ToUniversalTime();
            var newTimeStamp = DateTime.Now.AddDays(-1).ToUniversalTime();
            var refundTransaction = new TransactionDraft
            {
                Amount = Money.FromDecimal("EUR", TestingUtility.RandomInt(100, 10000)),
                Type = TransactionType.Refund,
                Timestamp = oldTimeStamp,
                State = TransactionState.Initial,
                InteractionId = TestingUtility.RandomString()
            };
            
            await WithUpdateablePayment(client,
                paymentDraft => DefaultPaymentDraftWithTransaction(paymentDraft, refundTransaction),
                async payment =>
                {
                    Assert.Single(payment.Transactions);
                    var action = new ChangeTransactionTimestampUpdateAction
                    {
                        TransactionId = payment.Transactions[0].Id,
                        Timestamp = newTimeStamp
                    };

                    var updatedPayment = await client
                        .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedPayment.Transactions);
                    Assert.NotEqual(oldTimeStamp, updatedPayment.Transactions[0].Timestamp);
                    return updatedPayment;
                });
        }
        
        [Fact]
        public async Task UpdatePaymentChangeTransactionInteractionId()
        {
            var oldInteractionId = TestingUtility.RandomString();
            var newInteractionId = TestingUtility.RandomString();
            var refundTransaction = new TransactionDraft
            {
                Amount = Money.FromDecimal("EUR", TestingUtility.RandomInt(100, 10000)),
                Type = TransactionType.Refund,
                Timestamp = DateTime.Now,
                State = TransactionState.Initial,
                InteractionId = oldInteractionId
            };
            
            await WithUpdateablePayment(client,
                paymentDraft => DefaultPaymentDraftWithTransaction(paymentDraft, refundTransaction),
                async payment =>
                {
                    Assert.Single(payment.Transactions);
                    Assert.Equal(oldInteractionId, payment.Transactions[0].InteractionId);
                    var action = new ChangeTransactionInteractionIdUpdateAction
                    {
                        TransactionId = payment.Transactions[0].Id,
                        InteractionId = newInteractionId
                    };

                    var updatedPayment = await client
                        .ExecuteAsync(payment.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Single(updatedPayment.Transactions);
                    Assert.Equal(newInteractionId, updatedPayment.Transactions[0].InteractionId);
                    return updatedPayment;
                });
        }
        
        [Fact]
        public async Task UpdatePaymentAddInterfaceInteraction()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateablePayment(client,
                    async payment =>
                    {
                        Assert.Empty(payment.InterfaceInteractions);
                        
                        var action = new AddInterfaceInteractionUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };

                        var updatedPayment = await client
                            .ExecuteAsync(payment.UpdateByKey(actions => actions.AddUpdate(action)));
                        
                        Assert.Single(updatedPayment.InterfaceInteractions);
                        Assert.Equal(type.Id, updatedPayment.InterfaceInteractions[0].Type.Id);
                        return updatedPayment;
                    });
            });
        }
        
        [Fact]
        public async Task UpdatePaymentSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateablePayment(client,
                    async payment =>
                    {
                        var action = new SetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };

                        var updatedPayment = await client
                            .ExecuteAsync(payment.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Equal(type.Id, updatedPayment.Custom.Type.Id);
                        return updatedPayment;
                    });
            });
        }

        [Fact]
        public async Task UpdatePaymentSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateablePayment(client,
                    paymentDraft => DefaultPaymentDraftWithCustomType(paymentDraft, type, fields),
                    async payment =>
                    {
                        var action = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };

                        var updatedPayment = await client
                            .ExecuteAsync(payment.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Equal(newValue, updatedPayment.Custom.Fields["string-field"]);
                        return updatedPayment;
                    });
            });
        }
        
        #endregion
    }
}