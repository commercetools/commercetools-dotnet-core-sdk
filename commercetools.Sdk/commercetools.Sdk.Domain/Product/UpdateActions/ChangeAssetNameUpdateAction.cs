using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class ChangeAssetNameUpdateAction : UpdateAction<Product>
    {
        public string Action => "changeAssetName";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public LocalizedString Name { get; set; }
    }
}