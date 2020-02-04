using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetCountryUpdateAction : CartUpdateAction
    {
        public override string Action => "setCountry";
        [Country]
        public string Country { get; set; }
    }
}