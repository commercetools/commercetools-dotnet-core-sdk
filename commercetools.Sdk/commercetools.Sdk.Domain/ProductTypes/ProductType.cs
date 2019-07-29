using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain
{
    [Endpoint("product-types")]
    [ResourceType(ReferenceTypeId.ProductType)]
    public class ProductType : Resource<ProductType>, IKeyReferencable<ProductType>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AttributeDefinition> Attributes { get; set; }
    }
}
