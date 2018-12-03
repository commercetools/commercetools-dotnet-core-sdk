namespace commercetools.Sdk.Domain.Reviews
{
    public class SetRatingUpdateAction : UpdateAction<Review>
    {
        public string Action => "setRating";
        public double Rating { get; set; }
    }
}