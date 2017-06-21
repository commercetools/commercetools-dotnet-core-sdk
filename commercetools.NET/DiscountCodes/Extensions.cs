using commercetools.CartDiscounts;
using commercetools.Common;

namespace commercetools.DiscountCodes
{
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the CartDiscountManager.
        /// </summary>
        /// <returns>CartDiscountManager</returns>
        public static DiscountCodeManager DiscountCodes(this Client client)
        {
            return new DiscountCodeManager(client);
        }
    }
}
