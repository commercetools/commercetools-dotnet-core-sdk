using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetCategoryOrderHintUpdateAction : UpdateAction<Product>
    {
        public string Action => "setCategoryOrderHint";
        [Required]
        public string CategoryId { get; set; }
        public string OrderHint { get; set; }
        public bool Staged { get; set; }
    }
}