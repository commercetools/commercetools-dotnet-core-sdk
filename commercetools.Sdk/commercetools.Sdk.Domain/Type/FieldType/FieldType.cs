namespace commercetools.Sdk.Domain
{
    public abstract class FieldType
    {
        public string Name
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }

        public BooleanType ToBooleanType()
        {
            return this as BooleanType;
        }

        public DateTimeType ToDateTimeType()
        {
            return this as DateTimeType;
        }

        public DateType ToDateType()
        {
            return this as DateType;
        }

        public EnumType ToEnumType()
        {
            return this as EnumType;
        }

        public LocalizedEnumType ToLocalizedEnumType()
        {
            return this as LocalizedEnumType;
        }

        public LocalizedStringType ToLocalizedStringType()
        {
            return this as LocalizedStringType;
        }

        public MoneyType ToMoneyType()
        {
            return this as MoneyType;
        }

        public NumberType ToNumberType()
        {
            return this as NumberType;
        }

        public ReferenceFieldType ToReferenceFieldType()
        {
            return this as ReferenceFieldType;
        }

        public SetType ToSetType()
        {
            return this as SetType;
        }

        public StringType ToStringType()
        {
            return this as StringType;
        }

        public TimeType ToTimeType()
        {
            return this as TimeType;
        }
    }
}