namespace commercetools.Sdk.Domain.Subscriptions
{
    /// <summary>
    /// * Amazon Simple Notification Service (Amazon SNS) can be used to push messages to AWS Lambda, HTTP endpoints (webhooks)
    /// * or fan-out messages to Amazon Simple Queue Service (SQS).
    /// </summary>
    [TypeMarker("SNS")]
    public class SnsDestination : Destination
    {
        /// <summary>
        /// The Amazon Resource Name (ARN) of the Simple Notification Service (SNS) topic name.
        /// Amazon Resource Names (ARNs) uniquely identify AWS resources.
        /// </summary>
        public string TopicArn { get; set; }
        public string AccessKey { get; set; }
        public string AccessSecret { get; set; }

        public SnsDestination(string accessKey, string accessSecret, string topicArn)
        {
            this.AccessKey = accessKey;
            this.AccessSecret = accessSecret;
            this.TopicArn = topicArn;
        }
    }
}
