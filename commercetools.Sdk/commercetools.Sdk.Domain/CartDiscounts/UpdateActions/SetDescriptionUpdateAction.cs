namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}