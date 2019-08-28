namespace commercetools.Sdk.Domain.APIExtensions
{
    [TypeMarker("AzureFunctions")]
    public class AzureFunctionsAuthentication : HttpDestinationAuthentication
    {
        public string Key { get; set; }
    }
}
