using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class ApplyDeltaToCustomLineItemShippingDetailsTargetsUpdateAction : CartUpdateAction
    {
        public override string Action => "applyDeltaToCustomLineItemShippingDetailsTargets";
        [Required]
        public string CustomLineItemId { get; set; }
        [Required]
        public List<ItemShippingTarget> TargetsDelta { get; set; }
    }
}