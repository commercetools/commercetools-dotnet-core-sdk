using commercetools.Common;

namespace commercetools.Carts
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the CartManager.
        /// </summary>
        /// <returns>CartManager</returns>
        public static CartManager Carts(this Client client)
        {
            return new CartManager(client);
        }
    }
}
