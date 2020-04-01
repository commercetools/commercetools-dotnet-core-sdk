﻿using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.ProductProjections
{
    public class ProductProjectionAdditionalParameters : IAdditionalParameters<ProductProjection>
    {
        public bool? Staged { get; set; }
        [Currency]
        public string PriceCurrency { get; set; }
        [Country]
        public string PriceCountry { get; set; }
        public string PriceCustomerGroup { get; set; }
        public string PriceChannel { get; set; }

        public string StoreProjection { get; set; }
        
        public string LocaleProjection { get; set; }
    }
}
