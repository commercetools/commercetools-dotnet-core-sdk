using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [Endpoint("product-types")]
    [ResourceType(ReferenceTypeId.ProductType)]
    public class ProductType
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AttributeDefinition> Attributes { get; set; }
    }
}
