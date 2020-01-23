using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetCustomerIdStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setCustomerId";
        public string CustomerId { get; set; }
    }
}