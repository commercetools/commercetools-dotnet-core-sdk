using Newtonsoft.Json;

namespace commercetools.Zones
{
    /// <summary>
    /// A geographical location representing a country with an optional state.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-zones.html#location"/>
    public class Location
    {
        #region Properties

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public Location()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Location(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Country = data.country;
            this.State = data.state;
        }

        #endregion
    }
}
