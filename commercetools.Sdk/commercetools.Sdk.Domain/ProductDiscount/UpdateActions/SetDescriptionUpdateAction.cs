namespace commercetools.Sdk.Domain.ProductDiscounts
{
    public class SetDescriptionUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}