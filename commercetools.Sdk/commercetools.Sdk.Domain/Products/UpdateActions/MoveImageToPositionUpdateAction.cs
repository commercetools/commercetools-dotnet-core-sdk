using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class MoveImageToPositionUpdateAction : UpdateAction<Product>
    {
        public string Action => "moveImageToPosition";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int Position { get; set; }
        public bool Staged { get; set; }

        private MoveImageToPositionUpdateAction(string imageUrl, int position, bool staged = true)
        {
            this.ImageUrl = imageUrl;
            this.Position = position;
            this.Staged = staged;
        }

        public MoveImageToPositionUpdateAction(string sku, string imageUrl, int position, bool staged = true) : this(imageUrl, position, staged)
        {
            this.Sku = sku;
        }
        public MoveImageToPositionUpdateAction(int variantId, string imageUrl, int position, bool staged = true) : this(imageUrl, position, staged)
        {
            this.VariantId = variantId;
        }
    }
}
