using commercetools.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace commercetools.GeoLocation
{
    /// <summary>
    /// Defines the Base Geometry class.
    /// </summary>
    public class Geometry : IGeoLocationObject
    {
        #region Properties

        public GeoLocationTypeEnum Type { get; }

        [JsonProperty(PropertyName = "type")]
        public string GeoJsonType { get; }

        [JsonProperty(PropertyName = "coordinates")]
        public dynamic Coordinates { get; }

        #endregion

        #region Constructors
        private Geometry()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Geometry(dynamic data)
        {
            if (data == null || data.type == null)
            {
                return;
            }
            Coordinates = data.coordinates ?? new List<float>();
            GeoJsonType = data.type;
            Type = Helper.TryGetEnumByEnumMemberAttribute((string)data.type, out GeoLocationTypeEnum type) ? type : GeoLocationTypeEnum.Unknown;
        }

        #endregion

    }
}
