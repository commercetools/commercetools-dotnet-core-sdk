using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    
    public class SetLocaleStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setLocale";
        [Language]
        public string Locale { get; set; }
    }
}