using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetReturnItemCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setReturnItemCustomType";
        [Required]
        public string ReturnItemId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}