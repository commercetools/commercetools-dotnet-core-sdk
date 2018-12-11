using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class SetAssetKeyUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetKey";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        [Required]
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
    }
}