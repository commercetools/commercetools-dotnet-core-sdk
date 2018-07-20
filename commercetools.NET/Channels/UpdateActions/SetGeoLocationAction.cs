using commercetools.Common;
using commercetools.GeoLocation;
using Newtonsoft.Json;

namespace commercetools.Channels.UpdateActions
{
    /// <summary>
    /// Set GeoLocation
    /// </summary>
    /// <see href="http://docs.commercetools.com/http-api-projects-channels.html#set-geolocation"/>
    public class SetGeoLocationAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// GeoLocation
        /// </summary>
        [JsonProperty(PropertyName = "geoLocation")]
        public IGeoLocationObject GeoLocation { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="geoLocation">GeoLocation</param>
        public SetGeoLocationAction(IGeoLocationObject geoLocation)
        {
            this.Action = "setGeoLocation";
            this.GeoLocation = geoLocation;
        }

        #endregion
    }
}
