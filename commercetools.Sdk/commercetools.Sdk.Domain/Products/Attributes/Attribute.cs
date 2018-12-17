namespace commercetools.Sdk.Domain.Products.Attributes
{
    public abstract class Attribute
    {
        public string Name { get; set; }

        public EnumAttribute ToEnumAttribute()
        {
            return this as EnumAttribute;
        }

        public LocalizedEnumAttribute ToLocalizedEnumAttribute()
        {
            return this as LocalizedEnumAttribute;
        }

        public LocalizedTextAttribute ToLocalizedTextAttribute()
        {
            return this as LocalizedTextAttribute;
        }

        public TextAttribute ToTextAttribute()
        {
            return this as TextAttribute;
        }

        public BooleanAttribute ToBooleanAttribute()
        {
            return this as BooleanAttribute;
        }

        public NumberAttribute ToNumberAttribute()
        {
            return this as NumberAttribute;
        }

        public DateAttribute ToDateAttribute()
        {
            return this as DateAttribute;
        }

        public DateTimeAttribute ToDateTimeAttribute()
        {
            return this as DateTimeAttribute;
        }

        public TimeAttribute ToTimeAttribute()
        {
            return this as TimeAttribute;
        }

        public ReferenceAttribute ToReferenceAttribute()
        {
            return this as ReferenceAttribute;
        }

        public SetTextAttribute ToSetTextAttribute()
        {
            return this as SetTextAttribute;
        }

        public SetLocalizedTextAttribute ToSetLocalizedTextAttribute()
        {
            return this as SetLocalizedTextAttribute;
        }

        public SetEnumAttribute ToSetEnumAttribute()
        {
            return this as SetEnumAttribute;
        }

        public SetLocalizedEnumAttribute ToSetLocalizedEnumAttribute()
        {
            return this as SetLocalizedEnumAttribute;
        }

        public SetMoneyAttribute ToSetMoneyAttribute()
        {
            return this as SetMoneyAttribute;
        }

        public SetDateAttribute ToSetDateAttribute()
        {
            return this as SetDateAttribute;
        }

        public SetDateTimeAttribute ToSetDateTimeAttribute()
        {
            return this as SetDateTimeAttribute;
        }

        public SetTimeAttribute ToSetTimeAttribute()
        {
            return this as SetTimeAttribute;
        }

        public SetBooleanAttribute ToSetBooleanAttribute()
        {
            return this as SetBooleanAttribute;
        }

        public SetNumberAttribute ToSetNumberAttribute()
        {
            return this as SetNumberAttribute;
        }

        public SetReferenceAttribute ToSetReferenceAttribute()
        {
            return this as SetReferenceAttribute;
        }
    }
}