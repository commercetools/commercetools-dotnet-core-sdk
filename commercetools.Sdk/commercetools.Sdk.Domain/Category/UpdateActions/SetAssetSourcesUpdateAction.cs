using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetAssetSourcesUpdateAction : UpdateAction<Category>
    {
        public string Action => "setAssetSources";
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public List<AssetSource> Sources { get; set; }
    }
}
