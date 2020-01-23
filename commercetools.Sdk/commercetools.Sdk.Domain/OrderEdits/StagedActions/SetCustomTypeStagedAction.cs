namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCustomTypeStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}