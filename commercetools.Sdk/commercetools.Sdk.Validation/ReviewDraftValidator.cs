using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Reviews;

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
