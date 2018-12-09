using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class ApplyDeltaToCustomLineItemShippingDetailsTargetsUpdateAction : UpdateAction<Cart>
    {
        public string Action => "applyDeltaToCustomLineItemShippingDetailsTargets";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public List<ItemShippingTarget> TargetsDelta { get; set; }
    }
}