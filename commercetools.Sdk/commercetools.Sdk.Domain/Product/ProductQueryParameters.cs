namespace commercetools.Sdk.Domain
{
    public class ProductQueryParameters : IQueryParameters<Product>
    {
        [Currency]
        public string PriceCurrency { get; set; }
        [Country]
        public string PriceCountry { get; set; }
        public string PriceCustomerGroup { get; set; }
        public string PriceChannel { get; set; }
    }
}
