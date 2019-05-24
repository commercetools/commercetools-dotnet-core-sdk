using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Payments.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Payments
{
    [Collection("Integration Tests")]
    public class PaymentsIntegrationTests : IClassFixture<ServiceProviderFixture>, IDisposable
    {
        private readonly PaymentsFixture paymentsFixture;

        public PaymentsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.paymentsFixture = new PaymentsFixture(serviceProviderFixture);
        }

        public void Dispose()
        {
            this.paymentsFixture.Dispose();
        }

        [Fact]
        public void CreatePayment()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            PaymentDraft paymentDraft = this.paymentsFixture.GetPaymentDraft();
            Payment payment = commerceToolsClient
                .ExecuteAsync(new CreateCommand<Payment>(paymentDraft)).Result;
            this.paymentsFixture.PaymentsToDelete.Add(payment);
            Assert.Equal(paymentDraft.Key, payment.Key);
        }

        [Fact]
        public void GetPaymentById()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();
            this.paymentsFixture.PaymentsToDelete.Add(payment);
            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new GetByIdCommand<Payment>(payment.Id)).Result;
            Assert.Equal(payment.Id, retrievedPayment.Id);
        }

        [Fact]
        public void GetPaymentByKey()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();
            this.paymentsFixture.PaymentsToDelete.Add(payment);
            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new GetByKeyCommand<Payment>(payment.Key)).Result;
            Assert.Equal(payment.Key, retrievedPayment.Key);
        }

        [Fact]
        public void QueryPayments()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();
            this.paymentsFixture.PaymentsToDelete.Add(payment);
            QueryCommand<Payment> queryCommand = new QueryCommand<Payment>();
            queryCommand.Where(p => p.Key == payment.Key.valueOf());
            var returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, p => p.Key == payment.Key);
        }

        [Fact]
        public async void DeletePaymentById()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();
            var deleteCommand = new DeleteByIdCommand<Payment>(payment.Id, payment.Version)
            {
                DataErasure = true
            };
            Payment deletedPayment = commerceToolsClient
                .ExecuteAsync(
                    deleteCommand)
                .Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<Cart>(deletedPayment.Id)));
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public async void DeletePaymentByKey()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();
            Payment deletedPayment = commerceToolsClient
                .ExecuteAsync(
                    new DeleteByKeyCommand<Payment>(payment.Key, payment.Version))
                .Result;
            NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                commerceToolsClient.ExecuteAsync(
                    new GetByIdCommand<Cart>(deletedPayment.Id)));
            Assert.Equal(404, exception.StatusCode);
        }

        #region UpdateActions

        [Fact]
        public void UpdatePaymentByIdSetKey()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();
            string newKey = TestingUtility.RandomString(10);

            var updateActions = new List<UpdateAction<Payment>>();
            SetKeyUpdateAction setKeyAction = new SetKeyUpdateAction() {Key = newKey};
            updateActions.Add(setKeyAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Payment>(payment.Id, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);
            Assert.Equal(newKey, retrievedPayment.Key);
        }

        [Fact]
        public void UpdatePaymentByKeyChangeAmountPlanned()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();
            var newAmountPlanned = Money.Parse($"{TestingUtility.RandomInt(100, 10000)} EUR");

            var updateActions = new List<UpdateAction<Payment>>();
            ChangeAmountPlannedUpdateAction changeAmountPlannedUpdateAction = new ChangeAmountPlannedUpdateAction() {Amount = newAmountPlanned};
            updateActions.Add(changeAmountPlannedUpdateAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Payment>(payment.Key, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);
            Assert.Equal(newAmountPlanned, retrievedPayment.AmountPlanned);
        }

        [Fact]
        public void UpdatePaymentByKeySetCustomer()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();

            Assert.Null(payment.Customer);

            var customer = this.paymentsFixture.CreateCustomer();

            var updateActions = new List<UpdateAction<Payment>>();
            SetCustomerUpdateAction setCustomerUpdateAction = new SetCustomerUpdateAction
            {
                Customer = new Reference<Customer> {Id = customer.Id}
            };
            updateActions.Add(setCustomerUpdateAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Payment>(payment.Key, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.NotNull(retrievedPayment.Customer);
            Assert.Equal(customer.Id, retrievedPayment.Customer.Id);
        }

        [Fact]
        public void UpdatePaymentByKeySetAnonymousId()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();

            Assert.True(string.IsNullOrEmpty(payment.AnonymousId));

            var anonymousId = TestingUtility.RandomString(10);

            var updateActions = new List<UpdateAction<Payment>>();
            SetAnonymousIdUpdateAction setAnonymousIdUpdateAction = new SetAnonymousIdUpdateAction()
            {
                AnonymousId = anonymousId
            };
            updateActions.Add(setAnonymousIdUpdateAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Payment>(payment.Key, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Equal(anonymousId, retrievedPayment.AnonymousId);
        }

        [Fact]
        public void UpdatePaymentByKeySetInterfaceId()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();

            Assert.True(string.IsNullOrEmpty(payment.InterfaceId));

            var interfaceId = TestingUtility.RandomString(10);

            var updateActions = new List<UpdateAction<Payment>>();
            SetInterfaceIdUpdateAction setInterfaceIdUpdateAction = new SetInterfaceIdUpdateAction()
            {
                InterfaceId = interfaceId
            };
            updateActions.Add(setInterfaceIdUpdateAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Payment>(payment.Key, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Equal(interfaceId, retrievedPayment.InterfaceId);
        }

        [Fact]
        public void UpdatePaymentByKeySetMethodInfoInterface()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();

            Assert.True(string.IsNullOrEmpty(payment.PaymentMethodInfo.PaymentInterface));

            var paymentInterface = TestingUtility.RandomString(10);

            var updateActions = new List<UpdateAction<Payment>>();
            SetMethodInfoInterfaceUpdateAction setMethodInfoInterfaceUpdateAction = new SetMethodInfoInterfaceUpdateAction()
            {
                Interface = paymentInterface
            };
            updateActions.Add(setMethodInfoInterfaceUpdateAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Payment>(payment.Key, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.NotNull(retrievedPayment.PaymentMethodInfo);
            Assert.Equal(paymentInterface, retrievedPayment.PaymentMethodInfo.PaymentInterface);
        }

        [Fact]
        public void UpdatePaymentByKeySetMethodInfoMethod()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();

            Assert.True(string.IsNullOrEmpty(payment.PaymentMethodInfo.Method));

            var paymentMethod = TestingUtility.RandomString(10);

            var updateActions = new List<UpdateAction<Payment>>();
            SetMethodInfoMethodUpdateAction setMethodInfoMethodUpdateAction = new SetMethodInfoMethodUpdateAction()
            {
                Method = paymentMethod
            };
            updateActions.Add(setMethodInfoMethodUpdateAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Payment>(payment.Key, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.NotNull(retrievedPayment.PaymentMethodInfo);
            Assert.Equal(paymentMethod, retrievedPayment.PaymentMethodInfo.Method);
        }

        [Fact]
        public void UpdatePaymentByKeySetMethodInfoName()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();

            Assert.Null(payment.PaymentStatus.InterfaceCode);

            var name = TestingUtility.RandomString(10);

            var updateActions = new List<UpdateAction<Payment>>();
            SetMethodInfoNameUpdateAction setMethodInfoNameUpdateAction = new SetMethodInfoNameUpdateAction
            {
                Name = new LocalizedString() {{"en", name}}
            };
            updateActions.Add(setMethodInfoNameUpdateAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Payment>(payment.Key, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.NotNull(retrievedPayment.PaymentMethodInfo);
            Assert.Equal(name, retrievedPayment.PaymentMethodInfo.Name["en"]);
        }

        [Fact]
        public void UpdatePaymentByKeySetStatusInterfaceCode()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();

            Assert.True(string.IsNullOrEmpty(payment.PaymentStatus.InterfaceCode));

            var interfaceCode = "20000";

            var updateActions = new List<UpdateAction<Payment>>();
            var setStatusInterfaceCodeUpdateAction = new SetStatusInterfaceCodeUpdateAction
            {
                InterfaceCode = interfaceCode
            };
            updateActions.Add(setStatusInterfaceCodeUpdateAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Payment>(payment.Key, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Equal(interfaceCode, retrievedPayment.PaymentStatus.InterfaceCode);
        }

        [Fact]
        public void UpdatePaymentByKeySetStatusInterfaceText()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();

            Assert.True(string.IsNullOrEmpty(payment.PaymentStatus.InterfaceText));

            var interfaceText = "Operation successful";

            var updateActions = new List<UpdateAction<Payment>>();
            var setStatusInterfaceTextUpdateAction = new SetStatusInterfaceTextUpdateAction
            {
                InterfaceText = interfaceText
            };
            updateActions.Add(setStatusInterfaceTextUpdateAction);

            Payment retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Payment>(payment.Key, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Equal(interfaceText, retrievedPayment.PaymentStatus.InterfaceText);
        }

        [Fact]
        public void UpdatePaymentTransitionToNewState()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            var payment = this.paymentsFixture.CreatePayment();

            Assert.Null(payment.PaymentStatus.State);

            var initialPaymentState = this.paymentsFixture.CreateNewState(StateType.PaymentState, initial: true);

            Assert.NotNull(initialPaymentState.Id);

            TransitionStateUpdateAction transitionStateUpdateAction = new TransitionStateUpdateAction()
            {
                State = new Reference<State>{ Id = initialPaymentState.Id}
            };
            var updateActions = new List<UpdateAction<Payment>> {transitionStateUpdateAction};

            var retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Payment>(payment.Id, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.NotNull(retrievedPayment.PaymentStatus.State);
            Assert.Equal(initialPaymentState.Id, retrievedPayment.PaymentStatus.State.Id);
        }

        [Fact]
        public void UpdatePaymentAddTransaction()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            var payment = this.paymentsFixture.CreatePayment();

            Assert.Empty(payment.Transactions);

            var refundMoney = Money.Parse($"{TestingUtility.RandomInt(100, 10000)} EUR");
            var refundTransaction = this.paymentsFixture.GetTransactionDraft(DateTime.Now, TransactionType.Refund,
                refundMoney, TestingUtility.RandomString(10), TransactionState.Initial);


            AddTransactionUpdateAction addTransactionUpdateAction = new AddTransactionUpdateAction()
            {
                Transaction = refundTransaction
            };
            var updateActions = new List<UpdateAction<Payment>> {addTransactionUpdateAction};

            var retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Payment>(payment.Id, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Single(retrievedPayment.Transactions);
            Assert.Equal(refundTransaction.InteractionId, retrievedPayment.Transactions[0].InteractionId);
        }

        [Fact]
        public void UpdatePaymentChangeTransactionState()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            var payment = this.paymentsFixture.CreatePaymentWithTransaction();

            Assert.Single(payment.Transactions);

            var refundTransaction = payment.Transactions[0];

            ChangeTransactionStateUpdateAction changeTransactionStateUpdateAction = new ChangeTransactionStateUpdateAction()
            {
                TransactionId = new Guid(refundTransaction.Id),
                State = TransactionState.Success
            };
            var updateActions = new List<UpdateAction<Payment>> {changeTransactionStateUpdateAction};

            var retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Payment>(payment.Id, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Single(retrievedPayment.Transactions);
            Assert.NotEqual(refundTransaction.State, retrievedPayment.Transactions[0].State);
            Assert.Equal(TransactionState.Success, retrievedPayment.Transactions[0].State);
        }

        [Fact]
        public void UpdatePaymentChangeTransactionTimestamp()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            var payment = this.paymentsFixture.CreatePaymentWithTransaction();

            Assert.Single(payment.Transactions);

            var refundTransaction = payment.Transactions[0];
            var newTimeStamp = DateTime.Now.AddDays(-1).ToUniversalTime();

            ChangeTransactionTimestampUpdateAction changeTransactionTimestampUpdateAction = new ChangeTransactionTimestampUpdateAction
            {
                TransactionId = new Guid(refundTransaction.Id),
                Timestamp = newTimeStamp
            };
            var updateActions = new List<UpdateAction<Payment>> {changeTransactionTimestampUpdateAction};

            var retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Payment>(payment.Id, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Single(retrievedPayment.Transactions);
            Assert.True(retrievedPayment.Transactions[0].Timestamp.HasValue);
            Assert.NotEqual(refundTransaction.Timestamp, retrievedPayment.Transactions[0].Timestamp);
        }

        [Fact]
        public void UpdatePaymentChangeTransactionInteractionId()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            var payment = this.paymentsFixture.CreatePaymentWithTransaction();

            Assert.Single(payment.Transactions);

            var refundTransaction = payment.Transactions[0];
            var newInteractionId = TestingUtility.RandomString(10);
            ChangeTransactionInteractionIdUpdateAction changeInteractionIdUpdateAction = new ChangeTransactionInteractionIdUpdateAction()
            {
                TransactionId = new Guid(refundTransaction.Id),
                InteractionId = newInteractionId
            };
            var updateActions = new List<UpdateAction<Payment>> {changeInteractionIdUpdateAction};

            var retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Payment>(payment.Id, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Single(retrievedPayment.Transactions);
            Assert.Equal(newInteractionId, retrievedPayment.Transactions[0].InteractionId);
        }

        [Fact]
        public void UpdatePaymentAddInterfaceInteraction()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            var payment = this.paymentsFixture.CreatePayment();

            Assert.Empty(payment.InterfaceInteractions);

            var type = this.paymentsFixture.CreateNewType();
            var fields = this.paymentsFixture.CreateNewFields();

            AddInterfaceInteractionUpdateAction addInterfaceInteractionUpdateAction = new AddInterfaceInteractionUpdateAction()
            {
                Type = new ResourceIdentifier<Type> { Id = type.Id},
                Fields = fields
            };
            var updateActions = new List<UpdateAction<Payment>> {addInterfaceInteractionUpdateAction};

            var retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Payment>(payment.Id, payment.Version, updateActions))
                .Result;

            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Single(retrievedPayment.InterfaceInteractions);
            Assert.Equal(type.Id, retrievedPayment.InterfaceInteractions[0].Type.Id);
        }

        [Fact]
        public void UpdatePaymentByIdSetCustomType()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePayment();

            var type = this.paymentsFixture.CreateNewType();
            var fields = this.paymentsFixture.CreateNewFields();

            var updateActions = new List<UpdateAction<Payment>>();

            var setCustomTypeUpdateAction = new SetCustomTypeUpdateAction()
            {
                Type = new ResourceIdentifier<Type> {Key = type.Key}, Fields = fields
            };
            updateActions.Add(setCustomTypeUpdateAction);

            var retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Payment>(payment.Id, payment.Version,
                    updateActions)).Result;
            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Equal(retrievedPayment.Id, payment.Id);
            Assert.Equal(type.Id, retrievedPayment.Custom.Type.Id);
        }

        [Fact]
        public void UpdatePaymentByIdSetCustomField()
        {
            IClient commerceToolsClient = this.paymentsFixture.GetService<IClient>();
            Payment payment = this.paymentsFixture.CreatePaymentWithCustomFields();

            var updateActions = new List<UpdateAction<Payment>>();
            string newValue = TestingUtility.RandomString(10);
            SetCustomFieldUpdateAction setCustomFieldUpdateAction = new SetCustomFieldUpdateAction()
            {
                Name = "string-field", Value = newValue
            };
            updateActions.Add(setCustomFieldUpdateAction);

            var retrievedPayment = commerceToolsClient
                .ExecuteAsync(new UpdateByIdCommand<Payment>(payment.Id, payment.Version,
                    updateActions)).Result;
            this.paymentsFixture.PaymentsToDelete.Add(retrievedPayment);

            Assert.Equal(payment.Id, retrievedPayment.Id);
            Assert.Equal(newValue, retrievedPayment.Custom.Fields["string-field"]);
        }
        #endregion
    }
}
