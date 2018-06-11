using commercetools.Common;

namespace commercetools.Zones
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the ZoneManager.
        /// </summary>
        /// <returns>ZoneManager</returns>
        public static ZoneManager Zones(this IClient client)
        {
            return new ZoneManager(client);
        }
    }
}
