using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class SetAssetSourcesUpdateAction : UpdateAction<Category>
    {
        public string Action => "setAssetSources";
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public List<AssetSource> Sources { get; set; }
    }
}