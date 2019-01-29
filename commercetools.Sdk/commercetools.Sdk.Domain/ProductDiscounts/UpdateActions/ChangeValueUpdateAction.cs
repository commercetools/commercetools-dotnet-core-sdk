using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class ChangeValueUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "changeValue";
        [Required]
        public ProductDiscountValue Value { get; set; }
    }
}