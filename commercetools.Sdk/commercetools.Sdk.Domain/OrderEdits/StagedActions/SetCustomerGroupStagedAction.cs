namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCustomerGroupStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCustomerGroup";
        public ResourceIdentifier CustomerGroup { get; set; }
    }
}