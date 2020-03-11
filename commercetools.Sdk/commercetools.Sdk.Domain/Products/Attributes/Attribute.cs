using System;

namespace commercetools.Sdk.Domain.Products.Attributes
{
    public abstract class Attribute : IAttribute
    {
        public string Name { get; set; }
        
        public object JsonValue { get; set; }

        public IAttribute ToIAttribute()
        {
            return this as IAttribute;
        }

        public EnumAttribute ToEnumAttribute()
        {
            if (this.IsEnumAttribute())
            {
                return new EnumAttribute(this as Attribute<PlainEnumValue>);
            }

            return null;
        }

        public LocalizedEnumAttribute ToLocalizedEnumAttribute()
        {
            if (this.IsLocalizedEnumAttribute())
            {
                return new LocalizedEnumAttribute(this as Attribute<LocalizedEnumValue>);
            }

            return null;
        }

        public LocalizedTextAttribute ToLocalizedTextAttribute()
        {
            if (this.IsLocalizedTextAttribute())
            {
                return new LocalizedTextAttribute(this as Attribute<LocalizedString>);
            }

            return null;
        }

        public TextAttribute ToTextAttribute()
        {
            if (this.IsTextAttribute())
            {
                return new TextAttribute(this as Attribute<string>);
            }

            return null;
        }

        public MoneyAttribute ToMoneyAttribute()
        {
            if (this.IsMoneyAttribute())
            {
                return new MoneyAttribute(this as Attribute<Money>);
            }

            return null;
        }

        public BooleanAttribute ToBooleanAttribute()
        {
            if (this.IsBooleanAttribute())
            {
                return new BooleanAttribute(this as Attribute<bool>);
            }

            return null;
        }

        public NumberAttribute ToNumberAttribute()
        {
            if (this.IsNumberAttribute())
            {
                return new NumberAttribute(this as Attribute<double>);
            }

            return null;
        }

        public DateAttribute ToDateAttribute()
        {
            if (this.IsDateAttribute())
            {
                return new DateAttribute(this as Attribute<DateTime>);
            }

            return null;
        }

        public DateTimeAttribute ToDateTimeAttribute()
        {
            if (this.IsDateTimeAttribute())
            {
                return new DateTimeAttribute(this as Attribute<DateTime>);
            }

            return null;
        }

        public TimeAttribute ToTimeAttribute()
        {
            if (this.IsTimeAttribute())
            {
                return new TimeAttribute(this as Attribute<TimeSpan>);
            }

            return null;
        }

        public ReferenceAttribute ToReferenceAttribute()
        {
            if (this.IsReferenceAttribute())
            {
                return new ReferenceAttribute(this as Attribute<Reference>);
            }

            return null;
        }

        public SetTextAttribute ToSetTextAttribute()
        {
            if (this.IsSetTextAttribute())
            {
                return new SetTextAttribute(this as SetAttribute<string>);
            }

            return null;
        }

        public SetLocalizedTextAttribute ToSetLocalizedTextAttribute()
        {
            if (this.IsSetLocalizedTextAttribute())
            {
                return new SetLocalizedTextAttribute(this as SetAttribute<LocalizedString>);
            }

            return null;
        }

        public SetEnumAttribute ToSetEnumAttribute()
        {
            if (this.IsSetEnumAttribute())
            {
                return new SetEnumAttribute(this as SetAttribute<PlainEnumValue>);
            }

            return null;
        }

        public SetLocalizedEnumAttribute ToSetLocalizedEnumAttribute()
        {
            if (this.IsSetLocalizedEnumAttribute())
            {
                return new SetLocalizedEnumAttribute(this as SetAttribute<LocalizedEnumValue>);
            }

            return null;
        }

        public SetMoneyAttribute ToSetMoneyAttribute()
        {
            if (this.IsSetMoneyAttribute())
            {
                return new SetMoneyAttribute(this as SetAttribute<Money>);
            }

            return null;
        }

        public SetDateAttribute ToSetDateAttribute()
        {
            if (this.IsSetDateAttribute())
            {
                return new SetDateAttribute(this as SetAttribute<DateTime>);
            }

            return null;
        }

        public SetDateTimeAttribute ToSetDateTimeAttribute()
        {
            if (this.IsSetDateTimeAttribute())
            {
                return new SetDateTimeAttribute(this as SetAttribute<DateTime>);
            }

            return null;
        }

        public SetTimeAttribute ToSetTimeAttribute()
        {
            if (this.IsSetTimeAttribute())
            {
                return new SetTimeAttribute(this as SetAttribute<TimeSpan>);
            }

            return null;
        }

        public SetBooleanAttribute ToSetBooleanAttribute()
        {
            if (this.IsSetBooleanAttribute())
            {
                return new SetBooleanAttribute(this as SetAttribute<bool>);
            }

            return null;
        }

        public SetNumberAttribute ToSetNumberAttribute()
        {
            if (this.IsSetNumberAttribute())
            {
                return new SetNumberAttribute(this as SetAttribute<double>);
            }

            return null;
        }

        public SetReferenceAttribute ToSetReferenceAttribute()
        {
            if (this.IsSetReferenceAttribute())
            {
                return new SetReferenceAttribute(this as SetAttribute<Reference>);
            }

            return null;
        }
    }
}
