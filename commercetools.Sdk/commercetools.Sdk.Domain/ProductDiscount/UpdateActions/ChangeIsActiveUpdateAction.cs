using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    public class ChangeIsActiveUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "changeIsActive";
        [Required]
        public bool IsActive { get; set; }
    }
}