namespace commercetools.Sdk.Domain.Categories
{
    public class SetAssetDescriptionUpdateAction : UpdateAction<Category>
    {
        public string Action => "setAssetDescription";
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public LocalizedString Description { get; set; }
    }
}