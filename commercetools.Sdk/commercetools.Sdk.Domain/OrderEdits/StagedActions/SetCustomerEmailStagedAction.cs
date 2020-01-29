namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCustomerEmailStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCustomerEmail";
        public string Email { get; set; }
    }
}