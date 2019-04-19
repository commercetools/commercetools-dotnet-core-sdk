using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetDiscountedPriceUpdateAction : UpdateAction<Product>
    {
        public string Action => "setDiscountedPrice";
        [Required]
        public string PriceId { get; set; }
        public bool Staged { get; set; }
        public DiscountedPrice Discounted { get; set; }
    }
}