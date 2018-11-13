namespace commercetools.Sdk.Domain
{
    public class AssetSource
    {
        public string Uri { get; set; }
        public string Key { get; set; }
        public AssetDimensions Dimensions { get; set; }
        public string ContentType { get; set; }
    }
}