using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetCustomLineItemCustomFieldUpdateAction : OrderUpdateAction
    {
        public override string Action => "setCustomLineItemCustomField";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}