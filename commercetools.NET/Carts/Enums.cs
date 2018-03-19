using System;

namespace commercetools.Carts
{
    /// <summary>
    /// CartState enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#cartstate"/>
    public enum CartState
    {
        Active,
        Merged,
        Ordered
    }

    /// <summary>
    /// DiscountCodeState enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#discountcodestate"/>
    public enum DiscountCodeState
    {
        NotActive,
        DoesNotMatchCart,
        MatchesCart,
        MaxApplicationReached
    }

    /// <summary>
    /// InventoryMode enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#inventorymode"/>
    public enum InventoryMode
    {
        TrackOnly,
        ReserveOnOrder,
        None,
    }

    /// <summary>
    /// LineItemMode enumeration.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#lineitemmode"/>
    public enum LineItemMode
    {
        Standard,
        GiftLineItem
    }

    /// <summary>
    /// LineItemPriceMode enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#lineitempricemode"/>
    public enum LineItemPriceMode
    {
        Platform,
        ExternalPrice,
        ExternalTotal
    }

    /// <summary>
    /// A rounding mode specifies how the platform should round monetary values.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#roundingmode"/>
    public enum RoundingMode
    {
        HalfEven,
        HalfUp,
        HalfDown
    }

    /// <summary>
    /// TaxMode enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#taxmode"/>
    public enum TaxMode
    {
        Platform,
        External,
        ExternalAmount,
        Disabled
    }
}
