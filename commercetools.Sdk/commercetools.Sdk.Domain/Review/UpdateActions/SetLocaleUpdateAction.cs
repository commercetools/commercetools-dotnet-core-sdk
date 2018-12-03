namespace commercetools.Sdk.Domain.Reviews
{
    public class SetLocaleUpdateAction : UpdateAction<Review>
    {
        public string Action => "setLocale";
        public string Locale { get; set; }
    }
}