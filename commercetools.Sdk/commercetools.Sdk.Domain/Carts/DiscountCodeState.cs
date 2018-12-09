namespace commercetools.Sdk.Domain.Carts
{
    public enum DiscountCodeState
    {
        NotActive,
        NotValid,
        DoesNotMatch,
        MatchesCart,
        MaxApplicationReached,
        ApplicationStoppedByPreviousDiscount
    }
}