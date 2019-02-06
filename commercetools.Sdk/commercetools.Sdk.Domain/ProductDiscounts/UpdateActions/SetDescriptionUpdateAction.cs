namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}