namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetItemShippingAddressCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setItemShippingAddressCustomType";
        [Required]
        public string AddressKey { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}