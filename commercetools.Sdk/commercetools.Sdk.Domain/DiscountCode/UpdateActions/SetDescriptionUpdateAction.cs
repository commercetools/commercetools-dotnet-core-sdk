namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class SetDescriptionUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setDescription";
        public LocalizedString Description { get; set; }
    }
}