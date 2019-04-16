using commercetools.Sdk.Domain.Carts;
using FluentValidation;

namespace commercetools.Sdk.Validation
{
    public class CartDraftValidator : BaseValidator<CartDraft>
    {
        public CartDraftValidator()
        {
            RuleFor(draft => draft.Currency).MustBeCurrency().NotEmpty();
            RuleFor(draft => draft.Country).MustBeCountry();
        }
    }
}
