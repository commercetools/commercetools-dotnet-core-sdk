namespace commercetools.Sdk.Domain.Categories
{
    public class AddAssetUpdateAction : UpdateAction<Category>
    {
        public string Action => "addAsset";
        public AssetDraft Asset { get; set; }
        public int Position { get; set; }
    }
}