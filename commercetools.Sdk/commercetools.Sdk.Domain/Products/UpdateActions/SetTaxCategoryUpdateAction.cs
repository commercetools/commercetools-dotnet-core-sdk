namespace commercetools.Sdk.Domain.Products
{
    public class SetTaxCategoryUpdateAction : UpdateAction<Product>
    {
        public string Action => "setTaxCategory";
        public ResourceIdentifier<TaxCategory> TaxCategory { get; set; }
    }
}
