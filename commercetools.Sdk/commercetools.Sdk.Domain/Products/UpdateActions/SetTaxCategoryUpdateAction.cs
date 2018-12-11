namespace commercetools.Sdk.Domain.Products
{
    public class SetTaxCategoryUpdateAction : UpdateAction<Product>
    {
        public string Action => "setTaxCategory";
        public Reference<TaxCategory> TaxCategory { get; set; }
    }
}