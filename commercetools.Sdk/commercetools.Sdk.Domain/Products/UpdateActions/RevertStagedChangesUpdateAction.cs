namespace commercetools.Sdk.Domain.Products
{
    public class RevertStagedChangesUpdateAction : UpdateAction<Product>
    {
        public string Action => "revertStagedChanges";
    }
}