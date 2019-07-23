using System;
using System.Linq;

namespace commercetools.Sdk.Domain
{
    public class GeoJsonGeometry
    {
        public string Type { get; set; }
        public double[] Coordinates { get; set; } = new double[2];


        public GeoJsonGeometry(double longitude, double latitude, string type = "Point")
        {
            Coordinates[0] = longitude;
            Coordinates[1] = latitude;
            this.Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj is GeoJsonGeometry geoLocation)
            {
                return this.Type.Equals(geoLocation.Type) &&
                       this.Coordinates.SequenceEqual(geoLocation.Coordinates);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode() + this.Coordinates.GetHashCode();
        }
    }
}
