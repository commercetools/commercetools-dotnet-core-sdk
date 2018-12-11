using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class SetAssetSourcesUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetSources";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public List<AssetSource> Sources { get; set; }
    }
}