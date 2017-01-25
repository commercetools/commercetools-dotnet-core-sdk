using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Zones.UpdateActions
{
    /// <summary>
    /// RemoveLocationAction
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-zones.html#remove-location"/>
    public class RemoveLocationAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Location
        /// </summary>
        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location">Location</param>
        public RemoveLocationAction(Location location)
        {
            this.Action = "removeLocation";
            this.Location = location;
        }

        #endregion
    }
}
