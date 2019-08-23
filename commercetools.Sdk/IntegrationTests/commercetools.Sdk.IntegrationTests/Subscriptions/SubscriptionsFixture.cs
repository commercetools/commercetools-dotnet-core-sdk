using commercetools.Sdk.Domain.Subscriptions;

namespace commercetools.Sdk.IntegrationTests.Subscriptions
{
    public static class SubscriptionsFixture
    {
        #region DraftBuilds
        public static SubscriptionDraft DefaultSubscriptionDraft(SubscriptionDraft subscriptionDraft)
        {
            var random = TestingUtility.RandomInt();
            subscriptionDraft.Key = $"Key_{random}";

            return subscriptionDraft;
        }
        public static SubscriptionDraft DefaultSubscriptionDraftWithKey(SubscriptionDraft draft, string key)
        {
            var subscriptionDraft = DefaultSubscriptionDraft(draft);
            subscriptionDraft.Key = key;
            return subscriptionDraft;
        }
        #endregion
    }
}
