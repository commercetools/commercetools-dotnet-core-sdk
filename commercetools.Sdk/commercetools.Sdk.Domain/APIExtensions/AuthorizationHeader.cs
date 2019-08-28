namespace commercetools.Sdk.Domain.APIExtensions
{
    [TypeMarker("AuthorizationHeader")]
    public class AuthorizationHeader : HttpDestinationAuthentication
    {
        public string HeaderValue { get; set; }
    }
}
