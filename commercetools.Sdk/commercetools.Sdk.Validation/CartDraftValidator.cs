using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Validation
{
    public class CartDraftValidator : BaseValidator<CartDraft>
    {
        public CartDraftValidator()
        {
            RuleFor(draft => draft.Currency).MustBeCurrency();
            RuleFor(draft => draft.Country).MustBeCountry();
        }
    }
}
