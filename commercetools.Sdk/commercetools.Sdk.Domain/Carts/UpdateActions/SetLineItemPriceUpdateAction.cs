using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetLineItemPriceUpdateAction : CartUpdateAction
    {
        public override string Action => "setLineItemPrice";
        [Required]
        public string LineItemId { get; set; }
        public BaseMoney ExternalPrice { get; set; }
    }
}