namespace commercetools.Sdk.Domain.APIExtensions
{
    [TypeMarker("HTTP")]
    public class HttpDestination : Destination
    {
        public string Url { get; set; }
        public HttpDestinationAuthentication Authentication { get; set; }
    }
}
