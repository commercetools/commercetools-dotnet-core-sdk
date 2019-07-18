namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}