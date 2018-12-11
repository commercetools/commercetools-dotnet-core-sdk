using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain
{
    public class ProductAdditionalParameters : IAdditionalParameters<Product>
    {
        [Currency]
        public string PriceCurrency { get; set; }
        [Country]
        public string PriceCountry { get; set; }
        public string PriceCustomerGroup { get; set; }
        public string PriceChannel { get; set; }
    }
}
