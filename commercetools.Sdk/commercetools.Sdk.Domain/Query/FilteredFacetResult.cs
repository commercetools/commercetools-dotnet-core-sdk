namespace commercetools.Sdk.Domain
{
    [TypeMarker("filter")]
    public class FilteredFacetResult : FacetResult
    {
        public int Count { get; set; }
        public int ProductCount { get; set; }
    }
}