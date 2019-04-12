using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class SetImageLabelUpdateAction : UpdateAction<Product>
    {
        public string Action => "setImageLabel";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        [Required] public string ImageUrl { get; set; }
        public string Label { get; set; }
        public bool Staged { get; set; }

        private SetImageLabelUpdateAction(string imageUrl, string label, bool staged = true)
        {
            this.ImageUrl = imageUrl;
            this.Label = label;
            this.Staged = staged;
        }

        public SetImageLabelUpdateAction(string sku, string imageUrl, string label, bool staged = true) : this(imageUrl,
            label, staged)
        {
            this.Sku = sku;
        }
        public SetImageLabelUpdateAction(int variantId, string imageUrl, string label, bool staged = true) : this(imageUrl,
            label, staged)
        {
            this.VariantId = variantId;
        }
    }
}
