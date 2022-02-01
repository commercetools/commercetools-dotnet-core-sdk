namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetShippingAddressCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setShippingAddressCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}