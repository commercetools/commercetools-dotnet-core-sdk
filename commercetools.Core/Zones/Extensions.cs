using commercetools.Common;

namespace commercetools.Zones
{
    public static class Extensions
    {
        /// <summary>
        /// Creates an instance of the ZoneManager.
        /// </summary>
        /// <returns>ZoneManager</returns>
        public static ZoneManager Zones(this Client client)
        {
            return new ZoneManager(client);
        }
    }
}
