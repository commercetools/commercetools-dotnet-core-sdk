using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetLineItemCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setLineItemCustomType";
        [Required]
        public string LineItemId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}