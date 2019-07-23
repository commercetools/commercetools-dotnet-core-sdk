namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class SetNameUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setName";
        public LocalizedString Name { get; set; }
    }
}