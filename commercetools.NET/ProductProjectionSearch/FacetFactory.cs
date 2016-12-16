namespace commercetools.ProductProjectionSearch
{
    /// <summary>
    /// FacetFactory
    /// </summary>
    public class FacetFactory
    {
        /// <summary>
        /// Creates a Facet using JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        /// <returns>A object derived from Facet, or null</returns>
        public static Facet Create(dynamic data = null)
        {
            if (data == null || data.type == null)
            {
                return null;
            }

            switch ((string)data.type)
            {
                case "terms":
                    return new TermFacet(data);
                case "range":
                    return new RangeFacet(data);
                case "filter":
                    return new FilterFacet(data);
            }

            return null;
        }
    }
}
