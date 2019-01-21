using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Customers;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Customers
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
        public void LoginCustomer()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            this.customerFixture.CustomersToDelete.Add(customer);
            CustomerSignInResult customerSignInResult = commerceToolsClient.ExecuteAsync(new LoginCustomerCommand(customer.Email, CustomerFixture.Password)).Result as CustomerSignInResult;
            Assert.NotNull(customerSignInResult);
        }

        [Fact]
        public void ChangePasswordCustomer()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            var password = "12345";
            Customer updatedCustomer = commerceToolsClient.ExecuteAsync(new ChangeCustomerPasswordCommand(customer.Id, customer.Version, CustomerFixture.Password, password)).Result;
            this.customerFixture.CustomersToDelete.Add(updatedCustomer);
            CustomerSignInResult customerSignInResult = commerceToolsClient.ExecuteAsync(new LoginCustomerCommand(customer.Email, password)).Result as CustomerSignInResult;
            Assert.NotNull(customerSignInResult);
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
        public void CreateTokenForCustomerEmailVerification()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            CustomerToken customerToken = commerceToolsClient.ExecuteAsync(new CreateTokenForCustomerEmailVerificationCommand(customer.Id, 10, customer.Version)).Result as CustomerToken;
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
        public void GetCustomerByEmailToken()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            CustomerToken customerToken = commerceToolsClient.ExecuteAsync(new CreateTokenForCustomerEmailVerificationCommand(customer.Id, 10, customer.Version)).Result as CustomerToken;
            this.customerFixture.CustomersToDelete.Add(customer);
            Customer retrievedCustomer = commerceToolsClient.ExecuteAsync(new GetCustomerByEmailTokenCommand(customerToken.Value)).Result;
            Assert.Equal(customer.Email, retrievedCustomer.Email);
        }

        [Fact]
        public void VerifyCustomerEmail()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            CustomerToken customerToken = commerceToolsClient.ExecuteAsync(new CreateTokenForCustomerEmailVerificationCommand(customer.Id, 10, customer.Version)).Result as CustomerToken;
            Customer retrievedCustomer = commerceToolsClient.ExecuteAsync(new VerifyCustomerEmailCommand(customerToken.Value, customer.Version)).Result;
            this.customerFixture.CustomersToDelete.Add(retrievedCustomer);
            Assert.Equal(customer.Email, retrievedCustomer.Email);
        }

        [Fact]
        public void ResetCustomerPassword()
        {
            IClient commerceToolsClient = this.customerFixture.GetService<IClient>();
            Customer customer = this.customerFixture.CreateCustomer();
            CustomerToken customerToken = commerceToolsClient.ExecuteAsync(new CreateTokenForCustomerPasswordResetCommand(customer.Email)).Result as CustomerToken;
            var password = "12345";
            Customer retrievedCustomer = commerceToolsClient.ExecuteAsync(new ResetCustomerPasswordCommand(customerToken.Value, password, customer.Version)).Result;
            this.customerFixture.CustomersToDelete.Add(retrievedCustomer);
            CustomerSignInResult customerSignInResult = commerceToolsClient.ExecuteAsync(new LoginCustomerCommand(customer.Email, password)).Result as CustomerSignInResult;
            Assert.NotNull(customerSignInResult);
        }
    }
}
