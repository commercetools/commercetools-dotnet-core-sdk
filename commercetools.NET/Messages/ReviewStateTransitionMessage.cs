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

        /// <summary>
        /// Old State
        /// </summary>
        [JsonProperty(PropertyName = "oldState")]
        public Reference OldState { get; private set; }

        /// <summary>
        /// New State
        /// </summary>
        [JsonProperty(PropertyName = "newState")]
        public Reference NewState { get; private set; }

        /// <summary>
        /// Indicates if this review was taken into account in the ratings statistics of the target before the state transition.
        /// </summary>
        [JsonProperty(PropertyName = "oldIncludedInStatistics")]
        public bool? OldIncludedInStatistics { get; private set; }

        /// <summary>
        /// Indicates if this review is taken into account in the ratings statistics of the target after the state transition.
        /// </summary>
        [JsonProperty(PropertyName = "newIncludedInStatistics")]
        public bool? NewIncludedInStatistics { get; private set; }

        /// <summary>
        /// Target
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        public Reference Target { get; private set; }

        /// <summary>
        /// Force
        /// </summary>
        [JsonProperty(PropertyName = "force")]
        public bool? Force { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ReviewStateTransitionMessage(dynamic data)
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
