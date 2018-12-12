using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.ProductProjections
{
    public class TextSearch
    {
        [Language]
        public string Language { get; set; }

        public string Term { get; set; }
    }
}
