namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}