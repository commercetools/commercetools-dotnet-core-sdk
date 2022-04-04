using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetReturnItemCustomFieldUpdateAction : OrderUpdateAction
    {
        public override string Action => "setReturnItemCustomField";
        [Required]
        public string ReturnItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}