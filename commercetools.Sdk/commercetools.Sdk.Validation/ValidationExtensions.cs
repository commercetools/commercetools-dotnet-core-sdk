using FluentValidation;

namespace commercetools.Sdk.Validation
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, TElement> MustBeCountry<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder) {
            return ruleBuilder.SetValidator(new CountryValidator());
        }

        public static IRuleBuilderOptions<T, TElement> MustBeLocale<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder) {
            return ruleBuilder.SetValidator(new CultureValidator());
        }

        public static IRuleBuilderOptions<T, TElement> MustBeCurrency<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder) {
            return ruleBuilder.SetValidator(new CurrencyValidator());
        }
    }
}
