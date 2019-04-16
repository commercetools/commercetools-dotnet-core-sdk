using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Validation
{
    public class ReviewDraftValidator : BaseValidator<ReviewDraft>
    {
        public ReviewDraftValidator()
        {
            RuleFor(draft => draft.Locale).MustBeLocale();
        }
    }
}
