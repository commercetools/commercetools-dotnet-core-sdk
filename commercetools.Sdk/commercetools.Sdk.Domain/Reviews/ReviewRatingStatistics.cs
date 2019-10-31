using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Reviews
{
    public class ReviewRatingStatistics
    {
        public double AverageRating { get; set; }
        public int HighestRating { get; set; }
        public int LowestRating { get; set; }
        public int Count { get; set; }

        public Dictionary<String, long> RatingsDistribution { get; set; }
    }
}
