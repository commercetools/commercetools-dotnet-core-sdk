namespace commercetools.Sdk.HttpApi
{
    public interface IClientConfiguration
    {
        string ClientId { get; set; }

        string ProjectKey { get; set; }

        string ClientSecret { get; set; }

        string Scope { get; set; }

        string AuthorizationBaseAddress { get; set; }

        string ApiBaseAddress { get; set; }
    }
}
