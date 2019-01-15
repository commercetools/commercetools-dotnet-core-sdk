namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class ChangeAssetNameUpdateAction : UpdateAction<Category>
    {
        public string Action => "changeAssetName";
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public LocalizedString Name { get; set; }
    }
}