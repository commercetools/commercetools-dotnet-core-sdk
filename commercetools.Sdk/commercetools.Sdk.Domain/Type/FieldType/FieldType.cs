namespace commercetools.Sdk.Domain
{
    public abstract class FieldType
    {
        public string Name
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }

        public BooleanFieldType ToBooleanFieldType()
        {
            return this as BooleanFieldType;
        }

        public DateTimeFieldType ToDateTimeFieldType()
        {
            return this as DateTimeFieldType;
        }

        public DateFieldType ToDateFieldType()
        {
            return this as DateFieldType;
        }

        public EnumFieldType ToEnumFieldType()
        {
            return this as EnumFieldType;
        }

        public LocalizedEnumFieldType ToLocalizedEnumFieldType()
        {
            return this as LocalizedEnumFieldType;
        }

        public LocalizedStringFieldType ToLocalizedStringFieldType()
        {
            return this as LocalizedStringFieldType;
        }

        public MoneyFieldType ToMoneyFieldType()
        {
            return this as MoneyFieldType;
        }

        public NumberFieldType ToNumberFieldType()
        {
            return this as NumberFieldType;
        }

        public ReferenceFieldType ToReferenceFieldType()
        {
            return this as ReferenceFieldType;
        }

        public SetFieldType ToSetFieldType()
        {
            return this as SetFieldType;
        }

        public StringFieldType ToStringFieldType()
        {
            return this as StringFieldType;
        }

        public TimeFieldType ToTimeFieldType()
        {
            return this as TimeFieldType;
        }
    }
}