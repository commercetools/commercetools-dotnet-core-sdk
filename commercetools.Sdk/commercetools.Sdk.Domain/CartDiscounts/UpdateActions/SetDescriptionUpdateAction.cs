namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class SetDescriptionUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}