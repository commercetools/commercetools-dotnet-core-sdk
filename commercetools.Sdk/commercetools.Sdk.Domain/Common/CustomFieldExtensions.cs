using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain
{
    // TODO Add other custom field types
    public static class CustomFieldExtensions
    {
        public static EnumValue ToEnum(this object o)
        {
            return o as EnumValue;
        }

        public static LocalizedEnumValue ToLocalizedEnum(this object o)
        {
            return o as LocalizedEnumValue;
        }

        public static LocalizedString ToLocalizedString(this object o)
        {
            return o as LocalizedString;
        }

        public static BaseMoney ToMoney(this object o)
        {
            return o as BaseMoney;
        }
    }
}