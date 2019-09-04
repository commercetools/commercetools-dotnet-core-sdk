using commercetools.Sdk.Domain.Reviews;

namespace commercetools.Sdk.Domain.Messages.Reviews
{
    [TypeMarker("ReviewRatingSet")]
    public class ReviewRatingSetMessage : Message<Review>
    {
        public int OldRating { get; set;}

        public int NewRating { get; set;}

        public bool IncludedInStatistics { get; set;}

        public Reference Target { get; set;}
    }
}
