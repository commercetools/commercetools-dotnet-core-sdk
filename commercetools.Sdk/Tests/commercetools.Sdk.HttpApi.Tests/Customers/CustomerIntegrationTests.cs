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

        [Fact]
        public void ChangePasswordCustomer()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            Customer updatedCustomer = commerceToolsClient.ExecuteAsync(new ChangeCustomerPasswordCommand(customer.Id, customer.Version, CustomerFixture.Password, "12345")).Result;
            this.customerFixture.CustomersToDelete.Add(updatedCustomer);
            Assert.NotNull(updatedCustomer);
            // TODO Login with new password
        }

        [Fact]
        public void CreateTokenForCustomerPasswordReset()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            CustomerToken customerToken = commerceToolsClient.ExecuteAsync(new CreateTokenForCustomerPasswordResetCommand(customer.Email)).Result as CustomerToken;
            this.customerFixture.CustomersToDelete.Add(customer);
            Assert.NotNull(customerToken);
        }

        [Fact]
        public void GetCustomerByPasswordToken()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            CustomerToken customerToken = commerceToolsClient.ExecuteAsync(new CreateTokenForCustomerPasswordResetCommand(customer.Email)).Result as CustomerToken;
            this.customerFixture.CustomersToDelete.Add(customer);
            Customer retrievedCustomer = commerceToolsClient.ExecuteAsync(new GetCustomerByPasswordTokenCommand(customerToken.Value)).Result;
            Assert.Equal(customer.Email, retrievedCustomer.Email);
        }

        [Fact]
        public void ResetCustomerPassword()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            CustomerToken customerToken = commerceToolsClient.ExecuteAsync(new CreateTokenForCustomerPasswordResetCommand(customer.Email)).Result as CustomerToken;
            Customer retrievedCustomer = commerceToolsClient.ExecuteAsync(new ResetCustomerPasswordCommand(customerToken.Value, "12345", customer.Version)).Result;
            this.customerFixture.CustomersToDelete.Add(retrievedCustomer);
            Assert.NotNull(retrievedCustomer);
            // TODO Login with new password
        }
    }
}
