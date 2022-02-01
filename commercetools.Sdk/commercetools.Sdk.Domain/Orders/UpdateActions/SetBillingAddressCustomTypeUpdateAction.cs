namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetBillingAddressCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setBillingAddressCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}