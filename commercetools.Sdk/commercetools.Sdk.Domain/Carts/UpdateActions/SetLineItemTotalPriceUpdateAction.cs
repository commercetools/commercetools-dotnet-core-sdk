using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetLineItemTotalPriceUpdateAction : CartUpdateAction
    {
        public override string Action => "setLineItemTotalPrice";
        [Required]
        public string LineItemId { get; set; }
        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }
    }
}