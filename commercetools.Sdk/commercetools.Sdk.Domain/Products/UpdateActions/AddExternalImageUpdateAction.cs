using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class AddExternalImageUpdateAction : UpdateAction<Product>
    {
        public string Action => "addExternalImage";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public Image Image { get; set; }
        public bool Staged { get; set; }

        private AddExternalImageUpdateAction(Image image, bool staged = true)
        {
            this.Image = image;
            this.Staged = staged;
        }

        public AddExternalImageUpdateAction(string sku, Image image, bool staged = true) : this(image, staged)
        {
            this.Sku = sku;
        }
        public AddExternalImageUpdateAction(int variantId, Image image, bool staged = true) : this(image, staged)
        {
            this.VariantId = variantId;
        }
    }
}
