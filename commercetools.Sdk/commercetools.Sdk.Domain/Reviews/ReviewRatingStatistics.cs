namespace commercetools.Sdk.Domain.Reviews
{
    public class ReviewRatingStatistics
    {
        public double AverageRating { get; set; }
        public double HighestRating { get; set; }
        public double LowestRating { get; set; }
        public int Count { get; set; }
        // TODO See what type of object this should be
        public object RatingsDistribution { get; set; }
    }
}
