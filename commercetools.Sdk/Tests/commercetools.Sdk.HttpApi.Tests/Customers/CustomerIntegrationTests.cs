using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Customers;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests.Customers
{
    [Collection("Integration Tests")]
    public class CustomerIntegrationTests : IClassFixture<CustomerFixture>
    {
        private readonly CustomerFixture customerFixture;

        public CustomerIntegrationTests(CustomerFixture customerFixture)
        {
            this.customerFixture = customerFixture;
        }

        [Fact]
        public void SignUpCustomer()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            CustomerDraft customerDraft = this.customerFixture.GetCustomerDraft();
            CustomerSignInResult customerSignInResult = commerceToolsClient.ExecuteAsync(new SignUpCustomerCommand(customerDraft)).Result as CustomerSignInResult;
            this.customerFixture.CustomersToDelete.Add(customerSignInResult.Customer);
            Assert.Equal(customerDraft.Email, customerSignInResult.Customer.Email);
        }
    }
}
