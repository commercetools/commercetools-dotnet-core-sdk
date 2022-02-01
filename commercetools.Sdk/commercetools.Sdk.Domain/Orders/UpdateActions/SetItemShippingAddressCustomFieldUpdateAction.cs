namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetItemShippingAddressCustomFieldUpdateAction : OrderUpdateAction
    {
        public override string Action => "setItemShippingAddressCustomField";
        [Required]
        public string AddressKey { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}