using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.ShoppingLists;
using commercetools.Sdk.Domain.Zones;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Payments
{
    [Collection("Integration Tests")]
    public class PaymentsIntegrationTests : IClassFixture<PaymentsFixture>
    {
        private readonly PaymentsFixture paymentsFixture;

        public PaymentsIntegrationTests(PaymentsFixture paymentsFixture)
        {
            this.paymentsFixture = paymentsFixture;
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
    }
}
