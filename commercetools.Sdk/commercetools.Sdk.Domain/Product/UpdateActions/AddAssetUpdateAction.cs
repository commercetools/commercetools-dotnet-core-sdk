using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class AddAssetUpdateAction : UpdateAction<Product>
    {
        public string Action => "addAsset";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public int Position { get; set; }
        public bool Staged { get; set; }
        public AssetDraft Asset { get; set; }
    }
}