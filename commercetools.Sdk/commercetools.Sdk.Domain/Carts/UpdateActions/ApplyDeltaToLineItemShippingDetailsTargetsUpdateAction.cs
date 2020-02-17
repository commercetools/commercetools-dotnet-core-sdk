using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class ApplyDeltaToLineItemShippingDetailsTargetsUpdateAction : CartUpdateAction
    {
        public override string Action => "applyDeltaToLineItemShippingDetailsTargets";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public List<ItemShippingTarget> TargetsDelta { get; set; }
    }
}