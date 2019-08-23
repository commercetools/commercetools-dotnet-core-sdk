namespace commercetools.Sdk.Domain.Subscriptions
{
    /// <summary>
    /// Amazon Simple Queue Service (SQS) is a pull-queue on AWS and can be used as a subscription destination.
    /// </summary>
    [TypeMarker("SQS")]
    public class SqsDestination : Destination
    {
        public string QueueUrl { get; set; }
        public string AccessKey { get; set; }
        public string AccessSecret { get; set; }
        public string Region { get; set; }

        public SqsDestination(string accessKey, string accessSecret, string queueUrl, string region)
        {
            this.AccessKey = accessKey;
            this.AccessSecret = accessSecret;
            this.QueueUrl = queueUrl;
            this.Region = region;
        }
    }
}
