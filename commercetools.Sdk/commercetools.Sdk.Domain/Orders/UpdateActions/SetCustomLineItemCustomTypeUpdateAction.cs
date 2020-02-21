using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetCustomLineItemCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setCustomLineItemCustomType";
        [Required]
        public string CustomLineItemId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}