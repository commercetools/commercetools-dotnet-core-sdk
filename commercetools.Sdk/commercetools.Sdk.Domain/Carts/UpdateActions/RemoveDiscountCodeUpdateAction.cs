using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.DiscountCodes;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class RemoveDiscountCodeUpdateAction : CartUpdateAction
    {
        public override string Action => "removeDiscountCode";
        [Required]
        public IReference<DiscountCode> DiscountCode { get; set; }
    }
}