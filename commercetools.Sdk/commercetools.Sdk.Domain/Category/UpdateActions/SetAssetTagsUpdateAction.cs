using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetAssetTagsUpdateAction : UpdateAction<Category>
    {
        public string Action => "setAssetTags";
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public List<string> Tags { get; set; }
    }
}