using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the setRating update action.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#reviewratingset-message"/>
    public class ReviewRatingSetMessage : Message
    {
        #region Properties

        /// <summary>
        /// Old Rating
        /// </summary>
        [JsonProperty(PropertyName = "oldRating")]
        public int? OldRating { get; private set; }

        /// <summary>
        /// New Rating
        /// </summary>
        [JsonProperty(PropertyName = "newRating")]
        public int? NewRating { get; private set; }

        /// <summary>
        /// Included In Statistics
        /// </summary>
        [JsonProperty(PropertyName = "includedInStatistics")]
        public bool? IncludedInStatistics { get; private set; }

        /// <summary>
        /// Target
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        public Reference Target { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ReviewRatingSetMessage(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.OldRating = data.oldRating;
            this.NewRating = data.newRating;
            this.IncludedInStatistics = data.includedInStatistics;
            this.Target = new Reference(data.target);
        }

        #endregion
    }
}
