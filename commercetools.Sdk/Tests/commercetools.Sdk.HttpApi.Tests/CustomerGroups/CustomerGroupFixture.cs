using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CustomerGroups;
using System;
using System.Collections.Generic;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.Tests.CustomerGroups
{
    public class CustomerGroupFixture : ClientFixture, IDisposable
    {
        private TypeFixture typeFixture;

        public CustomerGroupFixture() : base()
        {
            this.CustomerGroupsToDelete = new List<CustomerGroup>();
            this.typeFixture = new TypeFixture();
        }

        public List<CustomerGroup> CustomerGroupsToDelete { get; private set; }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.CustomerGroupsToDelete.Reverse();
            foreach (CustomerGroup customerGroup in this.CustomerGroupsToDelete)
            {
                CustomerGroup deletedCustomerGroup = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<CustomerGroup>(new Guid(customerGroup.Id), customerGroup.Version)).Result;
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
            draft.GroupName = this.RandomString(6);
            draft.Key = this.RandomString(4);
            return draft;
        }

        public CustomerGroupDraft CreateCustomerGroupDraftWithCustomFields()
        {
            CustomerGroupDraft draft = new CustomerGroupDraft();
            draft.GroupName = this.RandomString(6);
            draft.Key = this.RandomString(4);
            CustomFieldsDraft customFieldsDraft = new CustomFieldsDraft();
            Type type = this.typeFixture.CreateType();
            this.typeFixture.TypesToDelete.Add(type);
            customFieldsDraft.Type = new ResourceIdentifier() { Key = type.Key };
            customFieldsDraft.Fields = new Fields();
            customFieldsDraft.Fields.Add("string-field", "test");
            customFieldsDraft.Fields.Add("localized-string-field", new LocalizedString() { { "en", "localized-string-field-value" } });
            customFieldsDraft.Fields.Add("enum-field", "enum-key-1");
            customFieldsDraft.Fields.Add("localized-enum-field", "enum-key-1");
            customFieldsDraft.Fields.Add("number-field", 3);
            customFieldsDraft.Fields.Add("boolean-field", true);
            customFieldsDraft.Fields.Add("date-field", new DateTime(2018, 11, 28));
            customFieldsDraft.Fields.Add("date-time-field", new DateTime(2018, 11, 28, 11, 01, 00));
            customFieldsDraft.Fields.Add("time-field", new TimeSpan(11, 01, 00));
            customFieldsDraft.Fields.Add("money-field", new Money() { CentAmount = 1800, CurrencyCode = "EUR" });
            customFieldsDraft.Fields.Add("set-field", new FieldSet<string>() { "test1", "test2" });
            draft.Custom = customFieldsDraft;
            return draft;
        }
    }
}
