using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Messages.Reviews
{
    [TypeMarker("ReviewStateTransition")]
    public class ReviewStateTransitionMessage : Message<Review>
    {
        public Reference<State> OldState { get; set; }
        public Reference<State> NewState { get; set; }
        public bool OldIncludedInStatistics { get; set; }
        public bool NewIncludedInStatistics { get; set; }
        public Reference Target { get; set;}
        public bool Force { get; set;}
    }
}
