using System.Runtime.Serialization;

namespace commercetools.CartDiscounts
{
    public enum CartDiscountType
    {
        [EnumMember(Value = "relative")]
        Relative,
        [EnumMember(Value = "absolute")]
        Absolute,
        [EnumMember(Value = "giftLineItem")]
        GiftLineItem
    }

    public enum CartDiscountTargetType
    {
        [EnumMember(Value = "lineItems")]
        LineItems,
        [EnumMember(Value = "customLineItems")]
        CustomLineItems,
        [EnumMember(Value = "shipping")]
        Shipping
    }
}
