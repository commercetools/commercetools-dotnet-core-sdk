namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetBillingAddressCustomFieldUpdateAction : OrderUpdateAction
    {
        public override string Action => "setBillingAddressCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}