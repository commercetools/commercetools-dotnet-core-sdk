using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetLocaleUpdateAction : OrderUpdateAction
    {
        public override string Action => "setLocale";
        [Language]
        public string Locale { get; set; }
    }
}