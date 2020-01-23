using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetShippingAddressStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setShippingAddress";
        public Address Address { get; set; }
    }
}