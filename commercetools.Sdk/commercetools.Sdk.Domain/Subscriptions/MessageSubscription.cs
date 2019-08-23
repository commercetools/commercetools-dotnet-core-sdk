using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Subscriptions
{
    public class MessageSubscription
    {
        public ResourceTypeId ResourceTypeId { get; set; }
        public List<string> Types { get; set; }
    }
}
