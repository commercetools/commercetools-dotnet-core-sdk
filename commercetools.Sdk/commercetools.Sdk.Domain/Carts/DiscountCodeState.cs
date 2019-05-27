namespace commercetools.Sdk.Domain.Carts
{
    public enum DiscountCodeState
    {
        NotActive,
        NotValid,
        DoesNotMatchCart,
        MatchesCart,
        MaxApplicationReached,
        ApplicationStoppedByPreviousDiscount
    }
}
