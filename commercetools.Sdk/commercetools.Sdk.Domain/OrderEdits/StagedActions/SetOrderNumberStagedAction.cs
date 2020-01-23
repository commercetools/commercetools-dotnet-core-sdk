using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetOrderNumberStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setOrderNumber";
        public string OrderNumber { get; set; }
    }
}