namespace commercetools.Sdk.Domain.Subscriptions
{
    /// <summary>
    /// AWS EventBridge: can be used to push events and messages to a serverless event bus
    /// that can forward them to AWS SQS, SNS, Lambda, and other AWS services based on forwarding rules.
    /// </summary>
    [TypeMarker("EventBridge")]
    public class EventBridgeDestination : Destination
    {
        public string Region { get; set; }
        public string AccountId { get; set; }
        public EventBridgeDestination(string region, string accountId)
        {
            this.Region = region;
            this.AccountId = accountId;
        }
    }
}
