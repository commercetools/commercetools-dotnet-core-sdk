namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetShippingAddressCustomFieldUpdateAction : OrderUpdateAction
    {
        public override string Action => "setShippingAddressCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}