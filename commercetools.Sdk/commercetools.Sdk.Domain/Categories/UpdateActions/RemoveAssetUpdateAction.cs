namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class RemoveAssetUpdateAction : UpdateAction<Category>
    {
        public string Action => "removeAsset";
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
    }
}