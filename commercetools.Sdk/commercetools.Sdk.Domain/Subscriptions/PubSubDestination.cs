namespace commercetools.Sdk.Domain.Subscriptions
{
    /// <summary>
    /// Google Cloud Pub/Sub Destination can be used both as a pull-queue, and to push messages to e.g. Google Cloud Functions or HTTP endpoints (webhooks).
    /// </summary>
    [TypeMarker("GoogleCloudPubSub")]
    public class PubSubDestination : Destination
    {
        public string ProjectId { get; set; }

        public string Topic { get; set; }

        public PubSubDestination(string projectId, string topic)
        {
            this.ProjectId = projectId;
            this.Topic = topic;
        }
    }
}
