using commercetools.Sdk.Domain.Reviews;

namespace commercetools.Sdk.Domain.Messages.Reviews
{
    [TypeMarker("ReviewCreated")]
    public class ReviewCreatedMessage : Message<Review>
    {
        public Review Review { get; set; }
    }
}
