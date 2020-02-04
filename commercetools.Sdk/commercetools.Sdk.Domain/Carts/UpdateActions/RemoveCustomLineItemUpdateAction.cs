using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class RemoveCustomLineItemUpdateAction : CartUpdateAction
    {
        public override string Action => "removeCustomLineItem";
        [Required]
        public string CustomLineItemId { get; set; }
    }
}