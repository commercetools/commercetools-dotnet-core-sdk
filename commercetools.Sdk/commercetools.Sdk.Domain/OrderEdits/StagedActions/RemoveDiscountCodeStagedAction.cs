using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.DiscountCodes;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class RemoveDiscountCodeStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "removeDiscountCode";
        [Required]
        public IReference<DiscountCode> DiscountCode { get; set; }
    }
}