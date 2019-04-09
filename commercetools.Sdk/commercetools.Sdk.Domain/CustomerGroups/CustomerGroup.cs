using System;

namespace commercetools.Sdk.Domain.CustomerGroups
{
    [Endpoint("customer-groups")]
    [ResourceType(ReferenceTypeId.CustomerGroup)]
    public class CustomerGroup
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Name { get; set; }
        public CustomFields Custom { get; set; }
    }
}
