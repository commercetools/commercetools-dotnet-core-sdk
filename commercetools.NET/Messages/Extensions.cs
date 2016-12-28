using commercetools.Common;

namespace commercetools.Messages
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the MessageManager.
        /// </summary>
        /// <returns>MessageManager</returns>
        public static MessageManager Messages(this Client client)
        {
            return new MessageManager(client);
        }
    }
}