using Newtonsoft.Json;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// IronMQ can be used as a pull-queue, but it can also be used to push messages to IronWorkers or HTTP endpoints (webhooks) or fan-out messages to other IronMQ queues.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#ironmq-destination"/>
    /// <seealso href="https://www.iron.io/platform/ironmq/"/>
    public class IronMQDestination : Destination
    {
        #region Properties

        /// <summary>
        /// The webhook URI of your IronMQ.
        /// </summary>
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="uri">The webhook URI of your IronMQ.</param>
        public IronMQDestination(string uri)
            : base("IronMQ")
        {
            this.Uri = uri;
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public IronMQDestination(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Uri = data.uri;
        }

        #endregion
    }
}
