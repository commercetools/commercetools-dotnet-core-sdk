using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.CustomerGroups.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.CustomerGroups.CustomerGroupsFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;

namespace commercetools.Sdk.IntegrationTests.CustomerGroups
{
    [Collection("Integration Tests")]
    public class CustomerGroupGroupsIntegrationTests
    {
        private readonly IClient client;

        public CustomerGroupGroupsIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateCustomerGroup()
        {
            var key = $"CreateCustomerGroup-{TestingUtility.RandomString()}";
            await WithCustomerGroup(
                client, customerGroupDraft => DefaultCustomerGroupDraftWithKey(customerGroupDraft, key),
                customerGroup => { Assert.Equal(key, customerGroup.Key); });
        }

        [Fact]
        public async Task GetCustomerGroupById()
        {
            var key = $"GetCustomerGroupById-{TestingUtility.RandomString()}";
            await WithCustomerGroup(
                client, customerGroupDraft => DefaultCustomerGroupDraftWithKey(customerGroupDraft, key),
                async customerGroup =>
                {
                    var retrievedCustomerGroup = await client
                        .ExecuteAsync(customerGroup.ToIdResourceIdentifier().GetById());
                    Assert.Equal(key, retrievedCustomerGroup.Key);
                });
        }

        [Fact]
        public async Task GetCustomerGroupByKey()
        {
            var key = $"GetCustomerGroupByKey-{TestingUtility.RandomString()}";
            await WithCustomerGroup(
                client, customerGroupDraft => DefaultCustomerGroupDraftWithKey(customerGroupDraft, key),
                async customerGroup =>
                {
                    var retrievedCustomerGroup = await client
                        .ExecuteAsync(customerGroup.ToKeyResourceIdentifier().GetByKey());
                    Assert.Equal(key, retrievedCustomerGroup.Key);
                });
        }

        [Fact]
        public async Task QueryCustomerGroups()
        {
            var key = $"QueryCustomerGroups-{TestingUtility.RandomString()}";
            await WithCustomerGroup(
                client, customerGroupDraft => DefaultCustomerGroupDraftWithKey(customerGroupDraft, key),
                async customerGroup =>
                {
                    var queryCommand = new QueryCommand<CustomerGroup>();
                    queryCommand.Where(p => p.Key == customerGroup.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task DeleteCustomerGroupById()
        {
            var key = $"DeleteCustomerGroupById-{TestingUtility.RandomString()}";
            await WithCustomerGroup(
                client, customerGroupDraft => DefaultCustomerGroupDraftWithKey(customerGroupDraft, key),
                async customerGroup =>
                {
                    await client.ExecuteAsync(customerGroup.DeleteById());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<CustomerGroup>(customerGroup))
                    );
                });
        }

        [Fact]
        public async Task DeleteCustomerGroupByKey()
        {
            var key = $"DeleteCustomerGroupByKey-{TestingUtility.RandomString()}";
            await WithCustomerGroup(
                client, customerGroupDraft => DefaultCustomerGroupDraftWithKey(customerGroupDraft, key),
                async customerGroup =>
                {
                    await client.ExecuteAsync(customerGroup.DeleteByKey());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<CustomerGroup>(customerGroup))
                    );
                });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateCustomerGroupByKeyChangeName()
        {
            await WithUpdateableCustomerGroup(client, async customerGroup =>
            {
                var name = TestingUtility.RandomString();
                var action = new ChangeNameUpdateAction {Name = name};

                var updatedCustomerGroup = await client
                    .ExecuteAsync(customerGroup.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(name, updatedCustomerGroup.Name);
                return updatedCustomerGroup;
            });
        }

        [Fact]
        public async Task UpdateCustomerGroupByIdSetKey()
        {
            await WithUpdateableCustomerGroup(client, async customerGroup =>
            {
                var key = TestingUtility.RandomString();
                var action = new SetKeyUpdateAction {Key = key};

                var updatedCustomerGroup = await client
                    .ExecuteAsync(customerGroup.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(key, updatedCustomerGroup.Key);
                return updatedCustomerGroup;
            });
        }

        [Fact]
        public async Task UpdateCustomerGroupSetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableCustomerGroup(client,
                    async customerGroup =>
                    {
                        var action = new SetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };

                        var updatedCustomerGroup = await client
                            .ExecuteAsync(customerGroup.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Equal(type.Id, updatedCustomerGroup.Custom.Type.Id);
                        return updatedCustomerGroup;
                    });
            });
        }

        [Fact]
        public async Task UpdateCustomerGroupSetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableCustomerGroup(client,
                    customerGroupDraft => DefaultCustomerGroupDraftWithCustomType(customerGroupDraft, type, fields),
                    async customerGroup =>
                    {
                        var action = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };

                        var updatedCustomerGroup = await client
                            .ExecuteAsync(customerGroup.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Equal(newValue, updatedCustomerGroup.Custom.Fields["string-field"]);
                        return updatedCustomerGroup;
                    });
            });
        }

        #endregion
    }
}