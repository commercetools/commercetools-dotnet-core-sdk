using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Reviews
{
    /// <summary>
    /// ReviewRatingStatistics
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-reviews.html#reviewratingstatistics"/>
    public class ReviewRatingStatistics
    {
        #region Properties

        [JsonProperty(PropertyName = "averageRating")]
        public decimal? AverageRating { get; private set; }

        [JsonProperty(PropertyName = "highestRating")]
        public int? HighestRating { get; private set; }

        [JsonProperty(PropertyName = "lowestRating")]
        public int? LowestRating { get; private set; }

        [JsonProperty(PropertyName = "count")]
        public int? Count { get; private set; }

        [JsonProperty(PropertyName = "ratingsDistribution")]
        public JObject RatingsDistribution { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReviewRatingStatistics()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ReviewRatingStatistics(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.AverageRating = data.averageRating;
            this.HighestRating = data.highestRating;
            this.LowestRating = data.lowestRating;
            this.Count = data.count;
            this.RatingsDistribution = data.ratingsDistribution != null ? new JObject(data.ratingsDistribution) : null;
        }

        #endregion
    }
}
