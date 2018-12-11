using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class SetAssetTagsUpdateAction : UpdateAction<Product>
    {
        public string Action => "setAssetDescription";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public List<string> Tags { get; set; }
    }
}