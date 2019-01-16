using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Query;
using System;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests.CustomerGroups
{
    [Collection("Integration Tests")]
    public class CustomerGroupIntegrationTests : IClassFixture<CustomerGroupFixture>
    {
        private readonly CustomerGroupFixture customerGroupFixture;

        public CustomerGroupIntegrationTests(CustomerGroupFixture customerGroupFixture)
        {
            this.customerGroupFixture = customerGroupFixture;
        }

        [Fact]
        public void GetCustomerGroupById()
        {
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroup();
            this.customerGroupFixture.CustomerGroupsToDelete.Add(customerGroup);
            CustomerGroup retrievedCustomerGroup = commerceToolsClient.ExecuteAsync(new GetByIdCommand<CustomerGroup>(new Guid(customerGroup.Id))).Result;
            Assert.Equal(customerGroup.Id, retrievedCustomerGroup.Id);
        }

        [Fact]
        public void GetCustomerGroupByKey()
        {
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroup();
            this.customerGroupFixture.CustomerGroupsToDelete.Add(customerGroup);
            CustomerGroup retrievedCustomerGroup = commerceToolsClient.ExecuteAsync(new GetByKeyCommand<CustomerGroup>(customerGroup.Key)).Result;
            Assert.Equal(customerGroup.Key, retrievedCustomerGroup.Key);
        }

        [Fact]
        public void CreateCustomerGroup()
        {
            CustomerGroupDraft draft = this.customerGroupFixture.CreateCustomerGroupDraft();
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = commerceToolsClient.ExecuteAsync(new CreateCommand<CustomerGroup>(draft)).Result;
            this.customerGroupFixture.CustomerGroupsToDelete.Add(customerGroup);
            Assert.Equal(draft.Key, customerGroup.Key);
        }

        [Fact]
        public void CreateCustomerGroupWithCustomFields()
        {
            CustomerGroupDraft draft = this.customerGroupFixture.CreateCustomerGroupDraftWithCustomFields();
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = commerceToolsClient.ExecuteAsync(new CreateCommand<CustomerGroup>(draft)).Result;
            this.customerGroupFixture.CustomerGroupsToDelete.Add(customerGroup);
            Assert.Equal(draft.Custom.Fields.Count, customerGroup.Custom.Fields.Count);
        }

        [Fact]
        public void QueryCustomerGroup()
        {
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroup();
            this.customerGroupFixture.CustomerGroupsToDelete.Add(customerGroup);
            string name = customerGroup.Name;
            QueryPredicate<CustomerGroup> queryPredicate = new QueryPredicate<CustomerGroup>(c => c.Name == name);
            QueryCommand<CustomerGroup> queryCommand = new QueryCommand<CustomerGroup>();
            queryCommand.SetWhere(queryPredicate);
            PagedQueryResult<CustomerGroup> returnedSet = commerceToolsClient.ExecuteAsync(queryCommand).Result;
            Assert.Contains(returnedSet.Results, c => c.Name == customerGroup.Name);
        }
    }
}
