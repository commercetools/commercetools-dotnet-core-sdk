using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class ChangeLineItemQuantityUpdateAction : CartUpdateAction
    {
        public override string Action => "changeLineItemQuantity";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public long Quantity { get; set; }
        public BaseMoney ExternalPrice { get; set; }
        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }
    }
}
