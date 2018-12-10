namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetCustomLineItemCustomFieldUpdateAction : UpdateAction<Order>
    {
        public string Action => "setCustomLineItemCustomField";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}