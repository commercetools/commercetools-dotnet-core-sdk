namespace commercetools.Sdk.Client
{
    using Domain;

    public class TextSearch
    {
        [Language]
        public string Language { get; set; }

        public string Term { get; set; }
    }
}
