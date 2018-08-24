namespace commercetools.Sdk.HttpApi
{
    public class ClientConfiguration : IClientConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string AuthorizationBaseAddress { get; set; }
    }
}