using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Messages;

namespace commercetools.Sdk.Domain.Subscriptions
{
    public class Payload
    {
        public string ProjectKey { get; set; }

        public string NotificationType { get; set; }

        public IReference Resource { get; set; }

        public UserProvidedIdentifiers ResourceUserProvidedIdentifiers { get; set; }
    }
}
