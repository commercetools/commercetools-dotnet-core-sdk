using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetBillingAddressStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setBillingAddress";
        public Address Address { get; set; }
    }
}