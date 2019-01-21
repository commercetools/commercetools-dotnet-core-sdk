using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.CustomerGroups.UpdateActions;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.Domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace commercetools.Sdk.HttpApi.IntegrationTests.CustomerGroups
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
        public void UpdateCustomerGroupByIdChangeName()
        {
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroup();
            string name = this.customerGroupFixture.RandomString(5);
            List<UpdateAction<CustomerGroup>> updateActions = new List<UpdateAction<CustomerGroup>>();
            ChangeNameUpdateAction changeNameUpdateAction = new ChangeNameUpdateAction() { Name = name };
            updateActions.Add(changeNameUpdateAction);
            CustomerGroup retrievedCustomerGroup = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<CustomerGroup>(new Guid(customerGroup.Id), customerGroup.Version, updateActions)).Result;
            this.customerGroupFixture.CustomerGroupsToDelete.Add(retrievedCustomerGroup);
            Assert.Equal(name, retrievedCustomerGroup.Name);
        }

        [Fact]
        public void UpdateCustomerGroupByIdSetKey()
        {
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroup();
            string key = this.customerGroupFixture.RandomString(5);
            List<UpdateAction<CustomerGroup>> updateActions = new List<UpdateAction<CustomerGroup>>();
            SetKeyUpdateAction setKeyUpdateAction = new SetKeyUpdateAction() { Key = key };
            updateActions.Add(setKeyUpdateAction);
            CustomerGroup retrievedCustomerGroup = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<CustomerGroup>(new Guid(customerGroup.Id), customerGroup.Version, updateActions)).Result;
            this.customerGroupFixture.CustomerGroupsToDelete.Add(retrievedCustomerGroup);
            Assert.Equal(key, retrievedCustomerGroup.Key);
        }

        [Fact]
        public void UpdateCustomerGroupByIdSetCustomType()
        {
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroup();
            var type = this.customerGroupFixture.CreateNewType();
            var fields = this.customerGroupFixture.CreateNewFields();
            List<UpdateAction<CustomerGroup>> updateActions = new List<UpdateAction<CustomerGroup>>();
            SetCustomTypeUpdateAction setCustomTypeUpdateAction = new SetCustomTypeUpdateAction() { Type = new ResourceIdentifier() { Id = type.Id }, Fields = fields };
            updateActions.Add(setCustomTypeUpdateAction);
            CustomerGroup retrievedCustomerGroup = commerceToolsClient.ExecuteAsync(new UpdateByIdCommand<CustomerGroup>(new Guid(customerGroup.Id), customerGroup.Version, updateActions)).Result;
            this.customerGroupFixture.CustomerGroupsToDelete.Add(retrievedCustomerGroup);
            Assert.Equal(type.Id, retrievedCustomerGroup.Custom.Type.Id);
        }

        [Fact]
        public void UpdateCustomerGroupByKeySetCustomField()
        {
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroupWithCustomFields();
            List<UpdateAction<CustomerGroup>> updateActions = new List<UpdateAction<CustomerGroup>>();
            string newValue = this.customerGroupFixture.RandomString(6);
            SetCustomFieldUpdateAction setCustomFieldUpdateAction = new SetCustomFieldUpdateAction() { Name = "string-field", Value = newValue };
            updateActions.Add(setCustomFieldUpdateAction);
            CustomerGroup retrievedCustomerGroup = commerceToolsClient.ExecuteAsync(new UpdateByKeyCommand<CustomerGroup>(customerGroup.Key, customerGroup.Version, updateActions)).Result;
            this.customerGroupFixture.CustomerGroupsToDelete.Add(retrievedCustomerGroup);
            Assert.Equal(newValue, retrievedCustomerGroup.Custom.Fields["string-field"]);
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

        [Fact]
        public void DeleteCustomerGroupById()
        {
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroup();
            CustomerGroup retrievedCustomerGroup = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<CustomerGroup>(new Guid(customerGroup.Id), customerGroup.Version)).Result;
            Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.ExecuteAsync(new GetByIdCommand<CustomerGroup>(new Guid(retrievedCustomerGroup.Id))));
        }

        [Fact]
        public void DeleteCustomerGroupByKey()
        {
            IClient commerceToolsClient = this.customerGroupFixture.GetService<IClient>();
            CustomerGroup customerGroup = this.customerGroupFixture.CreateCustomerGroup();
            CustomerGroup retrievedCustomerGroup = commerceToolsClient.ExecuteAsync(new DeleteByKeyCommand<CustomerGroup>(customerGroup.Key, customerGroup.Version)).Result;
            Assert.ThrowsAsync<HttpApiClientException>(() => commerceToolsClient.ExecuteAsync(new GetByIdCommand<CustomerGroup>(new Guid(retrievedCustomerGroup.Id))));
        }
    }
}
