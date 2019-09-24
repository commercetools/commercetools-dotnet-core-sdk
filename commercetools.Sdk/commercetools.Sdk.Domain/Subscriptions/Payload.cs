using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Messages;

namespace commercetools.Sdk.Domain.Subscriptions
{
    public abstract class Payload
    {
        public virtual string NotificationType { get => this.GetType().GetTypeMarkerAttributeValue();
            set { }
        }
        
        public string ProjectKey { get; set; }
        
        public Reference Resource { get; set; }

        public UserProvidedIdentifiers ResourceUserProvidedIdentifiers { get; set; }
    }
}
