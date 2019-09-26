using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Subscriptions
{
    [Endpoint("subscriptions")]
    [ResourceType(ReferenceTypeId.Subscription)]
    public class Subscription : Resource<Subscription>
    {
        public string Key { get; set; }

        public Destination Destination { get; set; }

        public List<MessageSubscription> Messages { get; set; }

        public List<ChangeSubscription> Changes { get; set; }

        public Format Format { get; set; }
        
        public SubscriptionHealthStatus Status { get; set; }
    }
}
