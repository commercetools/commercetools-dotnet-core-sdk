namespace commercetools.Sdk.Domain.Products
{
    public class RevertStagedVariantChangesUpdateAction : UpdateAction<Product>
    {
        public string Action => "revertStagedVariantChanges";
        public int VariantId { get; set; }
    }
}