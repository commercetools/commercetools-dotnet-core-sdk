using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the transitionState update action. 
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#reviewstatetransition-message"/>
    public class ReviewStateTransitionMessage : Message
    {
        #region Properties

        [JsonProperty(PropertyName = "oldState")]
        public Reference OldState { get; private set; }

        [JsonProperty(PropertyName = "newState")]
        public Reference NewState { get; private set; }

        [JsonProperty(PropertyName = "oldIncludedInStatistics")]
        public bool? OldIncludedInStatistics { get; private set; }

        [JsonProperty(PropertyName = "newIncludedInStatistics")]
        public bool? NewIncludedInStatistics { get; private set; }

        [JsonProperty(PropertyName = "target")]
        public Reference Target { get; private set; }

        [JsonProperty(PropertyName = "force")]
        public bool? Force { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ReviewStateTransitionMessage(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.OldState = new Reference(data.oldState);
            this.NewState = new Reference(data.newState);
            this.OldIncludedInStatistics = data.oldIncludedInStatistics;
            this.NewIncludedInStatistics = data.newIncludedInStatistics;
            this.Target = new Reference(data.target);
            this.Force = data.force;
        }

        #endregion
    }
}