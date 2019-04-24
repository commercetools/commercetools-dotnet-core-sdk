using System;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.CustomerGroups
{
    [Endpoint("customer-groups")]
    [ResourceType(ReferenceTypeId.CustomerGroup)]
    public class CustomerGroup : Resource<CustomerGroup>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public CustomFields Custom { get; set; }
    }
}
