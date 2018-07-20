using commercetools.Common;

namespace commercetools.Channels
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the ChannelManager.
        /// </summary>
        /// <returns>ChannelManager</returns>
        public static ChannelManager Channels(this Client client)
        {
            return new ChannelManager(client);
        }
    }
}
