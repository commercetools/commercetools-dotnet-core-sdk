using Newtonsoft.Json;

namespace commercetools.Subscriptions
{
    /// <summary>
    /// AWS SNS can be used to push messages to AWS Lambda, HTTP endpoints (webhooks) or fan-out messages to SQS queues.
    /// </summary>
    /// <remarks>
    /// The topic needs to exist beforehand. It is recommended to create an accessKey and accessSecret pair specifically for each subscription that only has the sns:Publish permission on this topic.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-subscriptions.html#aws-sns-destination"/>
    /// <seealso href="https://aws.amazon.com/sns/"/>
    public class AWSSNSDestination : Destination
    {
        #region Properties

        /// <summary>
        /// Topic Arn
        /// </summary>
        [JsonProperty(PropertyName = "topicArn")]
        public string TopicArn { get; private set; }

        /// <summary>
        /// Access key
        /// </summary>
        [JsonProperty(PropertyName = "accessKey")]
        public string AccessKey { get; private set; }

        /// <summary>
        /// Access secret
        /// </summary>
        [JsonProperty(PropertyName = "accessSecret")]
        public string AccessSecret { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="topicArn">Topic Arn</param>
        /// <param name="accessKey">Access key</param>
        /// <param name="accessSecret">Access secret</param>
        public AWSSNSDestination(string topicArn, string accessKey, string accessSecret)
            : base("SNS")
        {
            this.TopicArn = topicArn;
            this.AccessKey = accessKey;
            this.AccessSecret = accessSecret;
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public AWSSNSDestination(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.TopicArn = data.topicArn;
            this.AccessKey = data.accessKey;
            this.AccessSecret = data.accessSecret;
        }

        #endregion
    }
}
