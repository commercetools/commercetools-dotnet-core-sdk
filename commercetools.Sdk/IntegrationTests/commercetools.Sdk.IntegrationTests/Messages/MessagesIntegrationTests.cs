using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Customers.UpdateActions;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.Messages.Customers;
using commercetools.Sdk.Domain.Messages.Reviews;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Query;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Customers.CustomersFixture;
using static commercetools.Sdk.IntegrationTests.Reviews.ReviewsFixture;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.Messages
{
    /// <summary>
    /// Assume that messages enabled in danger zone of current project
    /// </summary>
    [Collection("Integration Tests")]
    public class MessagesIntegrationTests
    {
        private readonly IClient client;

        public MessagesIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task GetMessageById()
        {
            //lets create customer first to ensure message created
            await WithCustomer(client, async customer =>
            {
                var queryCommand = new QueryCommand<Message>();
                queryCommand.SetLimit(1);

                await AssertEventuallyAsync(async () =>
                    {
                        var returnedSet = await client.ExecuteAsync(queryCommand);
                        Assert.Single(returnedSet.Results);
                        var messageId = returnedSet.Results[0].Id;

                        var retrievedMessage = await client
                            .ExecuteAsync(new GetByIdCommand<Message>(messageId));
                        Assert.NotNull(retrievedMessage);
                    }
                );
            });
        }

        [Fact]
        public async Task QuerySpecificTypeOfMessages()
        {
            //lets create customer first to ensure message created
            await WithCustomer(client, async customer =>
            {
                var queryCommand = new QueryCommand<CustomerCreatedMessage>();
                queryCommand.Where(message =>
                    message.Type == "CustomerCreated" && message.Resource.Id == customer.Id.valueOf());

                await AssertEventuallyAsync(async () =>
                    {
                        //Act
                        var returnedSet = await client.ExecuteAsync(queryCommand);

                        //Assert
                        Assert.Single(returnedSet.Results);
                        var customerCreatedMessage = returnedSet.Results[0];
                        Assert.NotNull(customerCreatedMessage);
                        Assert.NotNull(customerCreatedMessage.Customer);
                        Assert.Equal(customer.Id, customerCreatedMessage.Customer.Id);
                    }
                );
            });
        }

        [Fact]
        public async Task QueryMultipleMessagesTypesOfSpecificResourceType()
        {
            //lets create customer first to ensure message created
            await WithCustomer(client, DefaultCustomerDraftWithAddress, async customer =>
            {
                Assert.Single(customer.Addresses);
                var oldAddress = customer.Addresses[0];
                //then change his address
                var newAddress = TestingUtility.GetRandomAddress();

                var updateActions = new List<UpdateAction<Customer>>();
                var action = new ChangeAddressUpdateAction() {Address = newAddress, AddressId = oldAddress.Id};
                updateActions.Add(action);

                var updatedCustomer = await client
                    .ExecuteAsync(new UpdateByIdCommand<Customer>(customer, updateActions));


                var queryCommand = new QueryCommand<Message<Customer>>();
                queryCommand.Where(message => message.Resource.Id == customer.Id.valueOf());

                await AssertEventuallyAsync(async () =>
                    {
                        //Act
                        var returnedSet = await client.ExecuteAsync(queryCommand);

                        //Assert
                        Assert.Equal(2, returnedSet.Results.Count);

                        var customerCreatedMessage =
                            returnedSet.Results.OfType<CustomerCreatedMessage>().FirstOrDefault();
                        var customerAddressChangedMessage =
                            returnedSet.Results.OfType<CustomerAddressChangedMessage>().FirstOrDefault();

                        Assert.NotNull(customerCreatedMessage);
                        Assert.NotNull(customerCreatedMessage.Customer);
                        Assert.Equal(customer.Id, customerCreatedMessage.Customer.Id);
                        Assert.NotNull(customerAddressChangedMessage);
                        Assert.NotNull(customerAddressChangedMessage.Address);
                        Assert.Equal(updatedCustomer.Addresses[0].Id, customerAddressChangedMessage.Address.Id);
                    }
                );
            });
        }

        [Fact]
        public async Task QueryDifferentMessageTypesOfDifferentResourceTypes()
        {
            //lets create customer first
            await WithCustomer(client, async customer =>
            {
                await WithReview(client, async review =>
                {
                    var queryCommand = new QueryCommand<Message>();
                    queryCommand.Where(message =>
                        message.Resource.Id == customer.Id.valueOf() || message.Resource.Id == review.Id.valueOf());

                    await AssertEventuallyAsync(async () =>
                        {
                            //Act
                            var returnedSet = await client.ExecuteAsync(queryCommand);

                            //Assert
                            Assert.Equal(2, returnedSet.Results.Count);

                            var reviewCreatedMessage =
                                returnedSet.Results.OfType<ReviewCreatedMessage>().FirstOrDefault();
                            var customerCreatedMessage =
                                returnedSet.Results.OfType<CustomerCreatedMessage>().FirstOrDefault();
                            Assert.NotNull(customerCreatedMessage);
                            Assert.NotNull(customerCreatedMessage.Customer);
                            Assert.Equal(customer.Id, customerCreatedMessage.Customer.Id);

                            Assert.NotNull(reviewCreatedMessage);
                            Assert.NotNull(reviewCreatedMessage.Review);
                            Assert.Equal(review.Id, reviewCreatedMessage.Review.Id);
                        }
                    );
                });
            });
        }
    }
}