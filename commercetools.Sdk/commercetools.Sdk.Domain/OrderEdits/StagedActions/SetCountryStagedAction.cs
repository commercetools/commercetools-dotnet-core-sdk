using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCountryStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCountry";
        [Country]
        public string Country { get; set; }
    }
}