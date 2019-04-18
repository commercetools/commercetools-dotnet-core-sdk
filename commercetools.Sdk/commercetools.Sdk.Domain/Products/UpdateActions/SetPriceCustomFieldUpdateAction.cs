using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetPriceCustomFieldUpdateAction : UpdateAction<Product>
    {
        public string Action => "setProductPriceCustomField";
        [Required]
        public string PriceId { get; set; }
        public bool Staged { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}