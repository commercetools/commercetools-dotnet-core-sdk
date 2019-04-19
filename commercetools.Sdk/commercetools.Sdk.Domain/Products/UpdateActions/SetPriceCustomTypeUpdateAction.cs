using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetPriceCustomTypeUpdateAction : UpdateAction<Product>
    {
        public string Action => "setProductPriceCustomType";
        public ResourceIdentifier Type { get; set; }
        [Required]
        public string PriceId { get; set; }
        public bool Staged { get; set; }
        public Fields Fields { get; set; }
    }
}