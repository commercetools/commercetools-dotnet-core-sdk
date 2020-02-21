using System.ComponentModel.DataAnnotations;
    
namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetLineItemCustomFieldUpdateAction : OrderUpdateAction
    {
        public override string Action => "setLineItemCustomField";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}