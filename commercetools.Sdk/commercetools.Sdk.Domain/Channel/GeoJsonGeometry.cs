namespace commercetools.Sdk.Domain
{
    public class GeoJsonGeometry
    {
        public string Value { get; set; } = "point";
        public double[] Coordinates { get; set; }
    }
}