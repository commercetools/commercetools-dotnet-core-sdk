using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Subscriptions
{
    public class SubscriptionDraft : IDraft<Subscription>
    {
        public string Key { get; set; }
        
        public Destination Destination { get; set; }

        public List<MessageSubscription> Messages { get; set; }

        public List<ChangeSubscription> Changes { get; set; }
    }
}
