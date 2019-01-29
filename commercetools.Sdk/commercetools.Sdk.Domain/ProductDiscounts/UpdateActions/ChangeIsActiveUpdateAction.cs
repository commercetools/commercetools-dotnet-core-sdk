using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class ChangeIsActiveUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "changeIsActive";
        [Required]
        public bool IsActive { get; set; }
    }
}