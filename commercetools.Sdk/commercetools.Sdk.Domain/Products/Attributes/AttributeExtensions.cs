using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public static class AttributeExtensions
    {
        public static bool IsTextAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<string>);
        }

        public static bool IsTimeAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<TimeSpan>);
        }

        public static bool IsBooleanAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<bool>);
        }

        public static bool IsDateAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<DateTime>);
        }
        public static bool IsDateTimeAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<DateTime>);
        }
        public static bool IsEnumAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<PlainEnumValue>);
        }
        public static bool IsLocalizedEnumAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<LocalizedEnumValue>);
        }
        public static bool IsLocalizedTextAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<LocalizedTextAttribute>);
        }
        public static bool IsMoneyAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<Money>);
        }
        public static bool IsNumberAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<double>);
        }
        public static bool IsReferenceAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(Attribute<Reference>);
        }
        
        public static bool IsNestedAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(NestedAttribute) || 
                attribute.GetType() == typeof(Attribute<List<Attribute>>);
        }
      
        public static bool IsSetBooleanAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<bool>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<bool>>);
        }
        public static bool IsSetDateAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<DateTime>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<DateTime>>);
        }
        public static bool IsSetDateTimeAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<DateTime>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<DateTime>>);
        }

        public static bool IsSetMoneyAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<Money>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<Money>>);
        }
        public static bool IsSetEnumAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<PlainEnumValue>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<PlainEnumValue>>);
        }
        public static bool IsSetLocalizedEnumAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<LocalizedEnumValue>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<LocalizedEnumValue>>);
        }
        public static bool IsSetLocalizedTextAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<LocalizedString>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<LocalizedString>>);
        }

        public static bool IsSetNumberAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<double>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<double>>);
        }

        public static bool IsSetReferenceAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<Reference>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<Reference>>);
        }
        public static bool IsSetTextAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<string>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<string>>);
        }
        public static bool IsSetTimeAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<TimeSpan>)
                || attribute.GetType() == typeof(Attribute<AttributeSet<TimeSpan>>);
        }
        public static bool IsSetOfNestedAttribute(this IAttribute attribute)
        {
            return
                attribute.GetType() == typeof(SetAttribute<NestedAttribute>)
                ||
                attribute.GetType() == typeof(Attribute<AttributeSet<NestedAttribute>>)
                ||
                attribute.GetType() == typeof(Attribute<AttributeSet<List<Attribute>>>);
        }
    }
}
