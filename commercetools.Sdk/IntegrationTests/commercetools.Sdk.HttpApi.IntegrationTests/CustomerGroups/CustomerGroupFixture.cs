﻿using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomerGroups;
using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Type = commercetools.Sdk.Domain.Types.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests.CustomerGroups
{
    public class CustomerGroupFixture : ClientFixture, IDisposable
    {
        private TypeFixture typeFixture;

        public CustomerGroupFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            this.CustomerGroupsToDelete = new List<CustomerGroup>();
            this.typeFixture = new TypeFixture(serviceProviderFixture);
        }

        public List<CustomerGroup> CustomerGroupsToDelete { get; private set; }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.CustomerGroupsToDelete.Reverse();
            foreach (CustomerGroup customerGroup in this.CustomerGroupsToDelete)
            {
                var deletedType = this.TryDeleteResource(customerGroup).Result;
            }
            this.typeFixture.Dispose();
        }

        public CustomerGroup CreateCustomerGroup()
        {
            CustomerGroupDraft draft = this.CreateCustomerGroupDraft();
            IClient commerceToolsClient = this.GetService<IClient>();
            CustomerGroup customerGroup = commerceToolsClient.ExecuteAsync(new CreateCommand<CustomerGroup>(draft)).Result;
            return customerGroup;
        }

        public CustomerGroupDraft CreateCustomerGroupDraft()
        {
            CustomerGroupDraft draft = new CustomerGroupDraft();
            draft.GroupName = TestingUtility.RandomString(10);
            draft.Key = TestingUtility.RandomString(10);
            return draft;
        }

        public CustomerGroup CreateCustomerGroupWithCustomFields()
        {
            CustomerGroupDraft draft = this.CreateCustomerGroupDraftWithCustomFields();
            IClient commerceToolsClient = this.GetService<IClient>();
            CustomerGroup customerGroup = commerceToolsClient.ExecuteAsync(new CreateCommand<CustomerGroup>(draft)).Result;
            return customerGroup;
        }

        public CustomerGroupDraft CreateCustomerGroupDraftWithCustomFields()
        {
            CustomerGroupDraft draft = new CustomerGroupDraft();
            draft.GroupName = TestingUtility.RandomString(10);
            draft.Key = TestingUtility.RandomString(10);
            CustomFieldsDraft customFieldsDraft = new CustomFieldsDraft();
            Type type = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(type);
            customFieldsDraft.Type = new ResourceIdentifier<Type> { Key = type.Key };
            customFieldsDraft.Fields = this.CreateNewFields();
            draft.Custom = customFieldsDraft;
            return draft;
        }

        public Fields CreateNewFields()
        {
            Fields fields = this.typeFixture.CreateNewFields();
            return fields;
        }

        public Type CreateNewType()
        {
            Type type = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(type);
            return type;
        }
    }
}
