namespace commercetools.Sdk.Domain
{
    [TypeMarker("filter")]
    public class FilteredFacetResult : FacetResult
    {
        public long Count { get; set; }
        public long ProductCount { get; set; }
    }
}