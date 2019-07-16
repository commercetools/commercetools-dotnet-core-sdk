using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetTaxCategoryUpdateAction : UpdateAction<Product>
    {
        public string Action => "setTaxCategory";
        public ResourceIdentifier<TaxCategory> TaxCategory { get; set; }
    }
}
