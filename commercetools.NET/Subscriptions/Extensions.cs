using commercetools.Common;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the SubscriptionManager.
        /// </summary>
        /// <returns>SubscriptionManager</returns>
        public static SubscriptionManager Subscriptions(this Client client)
        {
            return new SubscriptionManager(client);
        }
    }
}
