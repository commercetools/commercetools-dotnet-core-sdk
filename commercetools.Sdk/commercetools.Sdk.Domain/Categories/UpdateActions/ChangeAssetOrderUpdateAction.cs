using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class ChangeAssetOrderUpdateAction : UpdateAction<Category>
    {
        public string Action => "changeAssetOrder";
        public List<string> AssetOrder { get; set; }
    }
}