using commercetools.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.GeoLocation
{
    /// <summary>
    /// Defines the Point class.
    /// A point refers to a Position on a map, expressed in longitude and latitude.
    /// </summary>
    public class Point : IGeoLocationObject
    {
        #region Properties

        [JsonIgnore]
        public double Longitude { get; set; }

        [JsonIgnore]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public GeoLocationTypeEnum Type
        {
            get
            {
                return GeoLocationTypeEnum.Point;
            }
        }

        [JsonProperty(PropertyName = "coordinates")]
        public List<double> Coordinates
        {
            get
            {
                return new List<double> { this.Longitude, this.Latitude };
            }
            set
            {
                if (value.Count > 1)
                {
                    this.Longitude = value[0];
                    this.Latitude = value[1];
                }
                else
                {
                    throw new ArgumentException("List length < 2. List must contain Longitude and Latitude coordinates");
                }
            }
        }
        #endregion

        #region Constructors
        private Point()
        {

        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Point(dynamic data)
        {
            if (data == null || data.coordinates == null)
            {
                return;
            }
            this.Coordinates = Helper.GetListFromJsonArray<double>(data.coordinates);
        }

        public Point(double longitude, double latitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        #endregion

    }
}
