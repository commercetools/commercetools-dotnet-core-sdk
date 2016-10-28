using commercetools.Common;

namespace commercetools.Payments
{
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the PaymentManager.
        /// </summary>
        /// <returns>PaymentManager</returns>
        public static PaymentManager Payments(this Client client)
        {
            return new PaymentManager(client);
        }
    }
}