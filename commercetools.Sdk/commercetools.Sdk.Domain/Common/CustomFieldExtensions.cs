using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    // TODO Add cast checks
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

        public static Money ToMoney(this object o)
        {
            return o as Money;
        }
    }
}
