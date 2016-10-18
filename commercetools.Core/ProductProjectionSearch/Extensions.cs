using commercetools.Common;

namespace commercetools.ProductProjectionSearch
{
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the ProductProjectionSearchManager.
        /// </summary>
        /// <returns>ProductProjectionSearchManager</returns>
        public static ProductProjectionSearchManager ProductProjectionSearch(this Client client)
        {
            return new ProductProjectionSearchManager(client);
        }
    }
}