using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain.Categories
{
    public class SetAssetCustomTypeUpdateAction : UpdateAction<Category>
    {
        public string Action => "setAssetCustomType";
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}
