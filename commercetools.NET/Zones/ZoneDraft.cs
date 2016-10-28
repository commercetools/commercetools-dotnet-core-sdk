using System.Collections.Generic;

using Newtonsoft.Json;

namespace commercetools.Zones
{
    /// <summary>
    /// The representation to be sent to the server when creating a new zone.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-zones.html#zonedraft"/>
    public class ZoneDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "locations")]
        public List<Location> Locations { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name</param>
        public ZoneDraft(string name)
        {
            this.Name = name;
        }

        #endregion
    }
}
