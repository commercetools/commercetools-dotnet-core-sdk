﻿using System;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain
{
    [Endpoint("products")]
    [ResourceType(ReferenceTypeId.Product)]
    public class Product : Resource<Product>, IKeyReferencable<Product>, ICheckable<Product>
    {
        public string Key { get; set; }
        public Reference<ProductType> ProductType { get; set; }
        public ProductCatalogData MasterData { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public Reference<State> State { get; set; }
        public ReviewRatingStatistics ReviewRatingStatistics { get; set; }
    }
}
