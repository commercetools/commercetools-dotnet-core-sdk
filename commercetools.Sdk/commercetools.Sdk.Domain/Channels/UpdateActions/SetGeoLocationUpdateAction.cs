namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class SetGeoLocationUpdateAction : UpdateAction<Channel>
    {
        public string Action => "setGeoLocation";
        public GeoJsonGeometry GeoLocation { get; set; }
    }
}
