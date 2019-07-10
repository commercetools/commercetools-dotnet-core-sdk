namespace commercetools.Sdk.Domain.Reviews
{
    public class SetRatingUpdateAction : UpdateAction<Review>
    {
        public string Action => "setRating";
        public int? Rating { get; set; }
    }
}
