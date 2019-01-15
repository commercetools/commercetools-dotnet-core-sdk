namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class SetAssetCustomFieldUpdateAction : UpdateAction<Category>
    {
        public string Action => "setAssetCustomField";
        public string AssetId { get; set; }
        public string AssetKey { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}