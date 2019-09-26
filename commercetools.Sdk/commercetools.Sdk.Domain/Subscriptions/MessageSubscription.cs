using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Subscriptions
{
    public class MessageSubscription : IResourceTypeIdAsEnum<SubscriptionResourceTypeId>
    {
        public string ResourceTypeId { get; set; }
        public List<string> Types { get; set; }
        public SubscriptionResourceTypeId GetResourceTypeIdAsEnum()
        {
            try
            {
                return ResourceTypeId.GetEnum<SubscriptionResourceTypeId>();
            }
            catch (ArgumentException)
            {
                return SubscriptionResourceTypeId.Unknown;
            }
        }
    }
}
