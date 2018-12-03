using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class ChangeAssetOrderUpdateAction : UpdateAction<Product>
    {
        public string Action => "changeAssetOrder";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public List<string> AssetOrder { get; set; }
    }
}