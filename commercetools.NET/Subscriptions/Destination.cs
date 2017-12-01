using Newtonsoft.Json;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// A destination contains all info necessary for the commercetools platform to deliver a message onto your Message Queue. Message Queues can be differentiated by the type field.
    /// </summary>
    /// <remarks>
    /// Currently the Message Queues IronMQ, AWS SQS and AWS SNS are supported.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#destination"/>
    public class Destination
    {
        #region Properties

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        protected Destination(string type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        protected Destination(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Type = data.type;
        }

        #endregion
    }
}
