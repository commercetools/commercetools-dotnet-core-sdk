using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public static class AttributeExtensions
    {
        public static bool IsTextAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<string>;
        }

        public static bool IsTimeAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<TimeSpan>);
        }

        public static bool IsBooleanAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<bool>;
        }

        public static bool IsDateAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<DateTime>;
        }
        public static bool IsDateTimeAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<DateTime>;
        }
        public static bool IsEnumAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<PlainEnumValue>;
        }
        public static bool IsLocalizedEnumAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<LocalizedEnumValue>;
        }
        public static bool IsLocalizedTextAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<LocalizedTextAttribute>;
        }
        public static bool IsMoneyAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<BaseMoney>;
        }
        public static bool IsNumberAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<double>;
        }
        public static bool IsReferenceAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<Reference>;
        }
        
        public static bool IsNestedAttribute(this IAttribute attribute)
        {
            return
                attribute is NestedAttribute ||
                attribute is Attribute<List<Attribute>>;
        }
      
        public static bool IsSetBooleanAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<bool>>;
        }
        public static bool IsSetDateAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<DateTime>>;
        }
        public static bool IsSetDateTimeAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<DateTime>>;
        }

        public static bool IsSetMoneyAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<BaseMoney>>;
        }
        public static bool IsSetEnumAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<PlainEnumValue>>;
        }
        public static bool IsSetLocalizedEnumAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<LocalizedEnumValue>>;
        }
        public static bool IsSetLocalizedTextAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<LocalizedString>>;
        }

        public static bool IsSetNumberAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<double>>;
        }

        public static bool IsSetReferenceAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<Reference>>;
        }
        public static bool IsSetTextAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<string>>;
        }
        public static bool IsSetTimeAttribute(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<TimeSpan>>;
        }
        public static bool IsSetOfNestedAttribute(this IAttribute attribute)
        {
            return
                attribute is SetAttribute<NestedAttribute> ||
                attribute is Attribute<AttributeSet<NestedAttribute>> ||
                attribute is Attribute<AttributeSet<List<Attribute>>>;
        }
        public static bool IsSetAttribute<T>(this IAttribute attribute)
        {
            return
                attribute is Attribute<AttributeSet<T>>;
        }
    }
}
