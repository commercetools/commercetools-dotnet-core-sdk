namespace commercetools.Sdk.Domain.Subscriptions
{
    /// <summary>
    /// Azure Event Grid can be used to push messages to Azure Functions, HTTP endpoints (webhooks), and several other Azure tools.
    /// </summary>
    [TypeMarker("EventGrid")]
    public class AzureEventGridDestination : Destination
    {
        /// <summary>
        /// The URI of the Topic
        /// </summary>
        public string Uri { get; set; }

        public string AccessKey { get; set; }

        public AzureEventGridDestination(string accessKey, string uri)
        {
            this.AccessKey = accessKey;
            this.Uri = uri;
        }
    }
}
