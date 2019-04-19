using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class ChangeAssetOrderUpdateAction : UpdateAction<Product>
    {
        public string Action => "changeAssetOrder";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        public bool Staged { get; set; }
        public List<string> AssetOrder { get; set; }

        private ChangeAssetOrderUpdateAction(List<string> assetOrder, bool staged = true)
        {
            this.AssetOrder = assetOrder;
            this.Staged = staged;
        }

        public ChangeAssetOrderUpdateAction(string sku, List<string> assetOrder, bool staged = true) : this(assetOrder, staged)
        {
            this.Sku = sku;
        }
        public ChangeAssetOrderUpdateAction(int variantId, List<string> assetOrder, bool staged = true) : this(assetOrder, staged)
        {
            this.VariantId = variantId;
        }
    }
}
