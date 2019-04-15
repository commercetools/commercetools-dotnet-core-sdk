namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class RevertStagedVariantChangesUpdateAction : UpdateAction<Product>
    {
        public string Action => "revertStagedVariantChanges";
        public int VariantId { get; set; }
    }
}