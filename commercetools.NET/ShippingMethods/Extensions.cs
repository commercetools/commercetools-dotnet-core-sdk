using commercetools.Common;

namespace commercetools.ShippingMethods
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the ShippingMethodManager.
        /// </summary>
        /// <returns>ShippingMethodManager</returns>
        public static ShippingMethodManager ShippingMethods(this Client client)
        {
            return new ShippingMethodManager(client);
        }
    }
}
