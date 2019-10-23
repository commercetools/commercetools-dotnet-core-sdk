using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomerGroups;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.CustomerGroups
{
    public static class CustomerGroupsFixture
    {
        #region DraftBuilds

        public static CustomerGroupDraft DefaultCustomerGroupDraft(CustomerGroupDraft customerGroupDraft)
        {
            var randomInt = TestingUtility.RandomInt();
            customerGroupDraft.Key = $"Key{randomInt}";
            customerGroupDraft.GroupName = $"Group{randomInt}";
            return customerGroupDraft;
        }
        #endregion

        #region WithCustomerGroup

        public static async Task WithCustomerGroup( IClient client, Action<CustomerGroup> func)
        {
            await With(client, new CustomerGroupDraft(), DefaultCustomerGroupDraft, func);
        }
        public static async Task WithCustomerGroup( IClient client, Func<CustomerGroupDraft, CustomerGroupDraft> draftAction, Action<CustomerGroup> func)
        {
            await With(client, new CustomerGroupDraft(), draftAction, func);
        }

        public static async Task WithCustomerGroup( IClient client, Func<CustomerGroup, Task> func)
        {
            await WithAsync(client, new CustomerGroupDraft(), DefaultCustomerGroupDraft, func);
        }
        public static async Task WithCustomerGroup( IClient client, Func<CustomerGroupDraft, CustomerGroupDraft> draftAction, Func<CustomerGroup, Task> func)
        {
            await WithAsync(client, new CustomerGroupDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableCustomerGroup

        public static async Task WithUpdateableCustomerGroup(IClient client, Func<CustomerGroup, CustomerGroup> func)
        {
            await WithUpdateable(client, new CustomerGroupDraft(), DefaultCustomerGroupDraft, func);
        }

        public static async Task WithUpdateableCustomerGroup(IClient client, Func<CustomerGroupDraft, CustomerGroupDraft> draftAction, Func<CustomerGroup, CustomerGroup> func)
        {
            await WithUpdateable(client, new CustomerGroupDraft(), draftAction, func);
        }

        public static async Task WithUpdateableCustomerGroup(IClient client, Func<CustomerGroup, Task<CustomerGroup>> func)
        {
            await WithUpdateableAsync(client, new CustomerGroupDraft(), DefaultCustomerGroupDraft, func);
        }
        public static async Task WithUpdateableCustomerGroup(IClient client, Func<CustomerGroupDraft, CustomerGroupDraft> draftAction, Func<CustomerGroup, Task<CustomerGroup>> func)
        {
            await WithUpdateableAsync(client, new CustomerGroupDraft(), draftAction, func);
        }

        #endregion
    }
}
