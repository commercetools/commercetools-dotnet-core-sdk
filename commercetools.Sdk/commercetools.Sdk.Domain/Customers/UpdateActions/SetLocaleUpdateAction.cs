using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetLocaleUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setLocale";
        [Language]
        public string Locale { get; set; }
    }
}
