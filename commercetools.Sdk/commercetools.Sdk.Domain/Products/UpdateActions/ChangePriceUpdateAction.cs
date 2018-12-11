using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class ChangePriceUpdateAction : UpdateAction<Product>
    {
        public string Action => "changePrice";
        [Required]
        public string PriceId { get; set; }
        [Required]
        public PriceDraft Price { get; set; }
        public bool Staged { get; set; }
    }
}