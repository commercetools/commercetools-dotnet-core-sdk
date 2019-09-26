using System;

namespace commercetools.Sdk.Domain.Subscriptions
{
    public class ChangeSubscription : IResourceTypeIdAsEnum<SubscriptionResourceTypeId>
    {
        public string ResourceTypeId { get; set; }
        
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
