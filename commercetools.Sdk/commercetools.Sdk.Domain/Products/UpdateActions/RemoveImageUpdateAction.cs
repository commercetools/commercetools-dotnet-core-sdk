using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class RemoveImageUpdateAction : UpdateAction<Product>
    {
        public string Action => "removeImage";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public bool Staged { get; set; }

        private RemoveImageUpdateAction(string imageUrl, bool staged = true)
        {
            this.ImageUrl = imageUrl;
            this.Staged = staged;
        }

        public RemoveImageUpdateAction(string sku, string imageUrl, bool staged = true) : this(imageUrl, staged)
        {
            this.Sku = sku;
        }
        public RemoveImageUpdateAction(int variantId, string imageUrl, bool staged = true) : this(imageUrl, staged)
        {
            this.VariantId = variantId;
        }
    }
}
