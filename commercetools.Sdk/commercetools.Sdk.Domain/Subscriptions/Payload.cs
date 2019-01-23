using commercetools.Sdk.Domain.Messages;

namespace commercetools.Sdk.Domain.Subscriptions
{
    public class Payload
    {
        public string ProjectKey { get; set; }
        
        public string NotificationType { get; set; }
        
        public ResourceIdentifier Resource { get; set; }

        public UserProvidedIdentifiers ResourceUserProvidedIdentifiers { get; set; }
    }
}