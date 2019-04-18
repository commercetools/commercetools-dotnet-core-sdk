using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.HttpApi
{
    public class ClientConfiguration : IClientConfiguration
    {
        public string ClientId { get; set; }

        public string ProjectKey { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; }

        [RegularExpression(@"^.*/$", ErrorMessage = "ClientConfiguration AuthorizationBaseAddress URI should end with slash.")]
        public string AuthorizationBaseAddress { get; set; } = "https://auth.sphere.io/";

        [RegularExpression(@"^.*/$", ErrorMessage = "ClientConfiguration ApiBaseAddress URI should end with slash.")]
        public string ApiBaseAddress { get; set; } = "https://api.sphere.io/";
    }
}
