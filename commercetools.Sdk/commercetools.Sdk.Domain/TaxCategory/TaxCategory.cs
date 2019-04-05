using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [Endpoint("tax-categories")]
    [ResourceType(ReferenceTypeId.TaxCategory)]
    public class TaxCategory
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public int Version { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TaxRate> Rates { get; set; }
    }
}
