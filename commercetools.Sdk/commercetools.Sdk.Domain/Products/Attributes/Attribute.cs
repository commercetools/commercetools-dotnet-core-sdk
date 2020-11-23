using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public abstract class Attribute : IAttribute
    {
        public string Name { get; set; }

        public object JsonValue { get; set; }

        public bool ShouldSerializeJsonValue()
        {
            return false;
        }

        public IAttribute ToIAttribute()
        {
            return this as IAttribute;
        }

        public EnumAttribute ToEnumAttribute()
        {
            if (this is EnumAttribute t) { return t; }
            if (this.IsEnumAttribute())
            {
                return new EnumAttribute(this as Attribute<PlainEnumValue>);
            }

            return null;
        }

        public LocalizedEnumAttribute ToLocalizedEnumAttribute()
        {
            if (this is LocalizedEnumAttribute t) { return t; }
            if (this.IsLocalizedEnumAttribute())
            {
                return new LocalizedEnumAttribute(this as Attribute<LocalizedEnumValue>);
            }

            return null;
        }

        public LocalizedTextAttribute ToLocalizedTextAttribute()
        {
            if (this is LocalizedTextAttribute t) { return t; }
            if (this.IsLocalizedTextAttribute())
            {
                return new LocalizedTextAttribute(this as Attribute<LocalizedString>);
            }

            return null;
        }

        public TextAttribute ToTextAttribute()
        {
            if (this is TextAttribute t) { return t; }
            if (this.IsTextAttribute())
            {
                return new TextAttribute(this as Attribute<string>);
            }

            return null;
        }

        public MoneyAttribute ToMoneyAttribute()
        {
            if (this is MoneyAttribute t) { return t; }
            if (this.IsMoneyAttribute())
            {
                return new MoneyAttribute(this as Attribute<BaseMoney>);
            }

            return null;
        }

        public BooleanAttribute ToBooleanAttribute()
        {
            if (this is BooleanAttribute t) { return t; }
            if (this.IsBooleanAttribute())
            {
                return new BooleanAttribute(this as Attribute<bool>);
            }

            return null;
        }

        public NumberAttribute ToNumberAttribute()
        {
            if (this is NumberAttribute t) { return t; }
            if (this.IsNumberAttribute())
            {
                return new NumberAttribute(this as Attribute<double>);
            }

            return null;
        }

        public DateAttribute ToDateAttribute()
        {
            if (this is DateAttribute t) { return t; }
            if (this.IsDateAttribute())
            {
                return new DateAttribute(this as Attribute<DateTime>);
            }

            return null;
        }

        public DateTimeAttribute ToDateTimeAttribute()
        {
            if (this is DateTimeAttribute t) { return t; }
            if (this.IsDateTimeAttribute())
            {
                return new DateTimeAttribute(this as Attribute<DateTime>);
            }

            return null;
        }

        public TimeAttribute ToTimeAttribute()
        {
            if (this is TimeAttribute t) { return t; }
            if (this.IsTimeAttribute())
            {
                return new TimeAttribute(this as Attribute<TimeSpan>);
            }

            return null;
        }

        public ReferenceAttribute ToReferenceAttribute()
        {
            if (this is ReferenceAttribute t) { return t; }
            if (this.IsReferenceAttribute())
            {
                return new ReferenceAttribute(this as Attribute<Reference>);
            }

            return null;
        }
        
        public NestedAttribute ToNestedAttribute()
        {
            if (this is NestedAttribute t) { return t; }
            if (this.IsNestedAttribute())
            {
                return new NestedAttribute(this as Attribute<List<Attribute>>);
            }

            return null;
        }

        public SetTextAttribute ToSetTextAttribute()
        {
            if (this is SetTextAttribute t) { return t; }
            if (this.IsSetTextAttribute())
            {
                return new SetTextAttribute(this as Attribute<AttributeSet<string>>);
            }

            return null;
        }

        public SetLocalizedTextAttribute ToSetLocalizedTextAttribute()
        {
            if (this is SetLocalizedTextAttribute t) { return t; }
            if (this.IsSetLocalizedTextAttribute())
            {
                return new SetLocalizedTextAttribute(this as Attribute<AttributeSet<LocalizedString>>);
            }

            return null;
        }

        public SetEnumAttribute ToSetEnumAttribute()
        {
            if (this is SetEnumAttribute t) { return t; }
            if (this.IsSetEnumAttribute())
            {
                return new SetEnumAttribute(this as Attribute<AttributeSet<PlainEnumValue>>);
            }

            return null;
        }

        public SetLocalizedEnumAttribute ToSetLocalizedEnumAttribute()
        {
            if (this is SetLocalizedEnumAttribute t) { return t; }
            if (this.IsSetLocalizedEnumAttribute())
            {
                return new SetLocalizedEnumAttribute(this as Attribute<AttributeSet<LocalizedEnumValue>>);
            }

            return null;
        }

        public SetMoneyAttribute ToSetMoneyAttribute()
        {
            if (this is SetMoneyAttribute t) { return t; }
            if (this.IsSetMoneyAttribute())
            {
                return new SetMoneyAttribute(this as Attribute<AttributeSet<BaseMoney>>);
            }

            return null;
        }

        public SetDateAttribute ToSetDateAttribute()
        {
            if (this is SetDateAttribute t) { return t; }
            if (this.IsSetDateAttribute())
            {
                return new SetDateAttribute(this as Attribute<AttributeSet<DateTime>>);
            }

            return null;
        }

        public SetDateTimeAttribute ToSetDateTimeAttribute()
        {
            if (this is SetDateTimeAttribute t) { return t; }
            if (this.IsSetDateTimeAttribute())
            {
                return new SetDateTimeAttribute(this as Attribute<AttributeSet<DateTime>>);
            }

            return null;
        }

        public SetTimeAttribute ToSetTimeAttribute()
        {
            if (this is SetTimeAttribute t) { return t; }
            if (this.IsSetTimeAttribute())
            {
                return new SetTimeAttribute(this as Attribute<AttributeSet<TimeSpan>>);
            }

            return null;
        }

        public SetBooleanAttribute ToSetBooleanAttribute()
        {
            if (this is SetBooleanAttribute t) { return t; }
            if (this.IsSetBooleanAttribute())
            {
                return new SetBooleanAttribute(this as Attribute<AttributeSet<bool>>);
            }

            return null;
        }

        public SetNumberAttribute ToSetNumberAttribute()
        {
            if (this is SetNumberAttribute t) { return t; }
            if (this.IsSetNumberAttribute())
            {
                return new SetNumberAttribute(this as Attribute<AttributeSet<double>>);
            }

            return null;
        }

        public SetReferenceAttribute ToSetReferenceAttribute()
        {
            if (this is SetReferenceAttribute t) { return t; }
            if (this.IsSetReferenceAttribute())
            {
                return new SetReferenceAttribute(this as Attribute<AttributeSet<Reference>>);
            }

            return null;
        }
        
        public SetNestedAttribute ToSetOfNestedAttribute()
        {
            if (this is SetNestedAttribute t) { return t; }
            if (this.IsSetOfNestedAttribute())
            {
                return new SetNestedAttribute(this as Attribute<AttributeSet<NestedAttribute>>);
            }

            return null;
        }
    }
}
