namespace commercetools.Sdk.Domain
{
    [FacetResultType("filter")]
    public class FilteredFacetResult : FacetResult
    {
        public int Count { get; set; }
        public int ProductCount { get; set; }
    }
}