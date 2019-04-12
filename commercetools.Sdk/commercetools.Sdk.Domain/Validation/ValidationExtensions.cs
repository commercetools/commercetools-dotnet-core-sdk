namespace commercetools.Sdk.Domain.Validation
{
    public static class ValidationExtensions
    {
        public static ICountryValidator CountryValidator { get; set; }
        public static ICurrencyValidator CurrencyValidator { get; set; }
        public static ICultureValidator CultureValidator { get; set; }
    }
}
