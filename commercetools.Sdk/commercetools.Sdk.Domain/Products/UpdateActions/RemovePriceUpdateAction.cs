using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class RemovePriceUpdateAction : UpdateAction<Product>
    {
        public string Action => "removePrice";
        [Required]
        public string PriceId { get; set; }
        public bool Staged { get; set; }
    }
}