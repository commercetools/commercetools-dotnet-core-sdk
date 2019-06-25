namespace commercetools.Sdk.Domain.Reviews
{
    public class ReviewRatingStatistics
    {
        public double AverageRating { get; set; }
        public int HighestRating { get; set; }
        public int LowestRating { get; set; }
        public int Count { get; set; }
        // TODO See what type of object this should be
        public object RatingsDistribution { get; set; }
    }
}
