using commercetools.Common;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the ProductTypeManager.
        /// </summary>
        /// <returns>ProductTypeManager</returns>
        public static ProductTypeManager ProductTypes(this Client client)
        {
            return new ProductTypeManager(client);
        }
    }
}
