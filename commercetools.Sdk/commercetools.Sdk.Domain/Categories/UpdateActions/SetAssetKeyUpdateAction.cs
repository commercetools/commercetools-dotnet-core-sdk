using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class SetAssetKeyUpdateAction : UpdateAction<Category>
    {
        public string Action => "setAssetKey";
        [Required]
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
    }
}