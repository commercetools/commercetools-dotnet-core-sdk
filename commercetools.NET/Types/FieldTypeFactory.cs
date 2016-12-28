using System;

namespace commercetools.Types
{
    /// <summary>
    /// FieldTypeFactory
    /// </summary>
    public static class FieldTypeFactory
    {
        /// <summary>
        /// Creates an FieldType using JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        /// <returns>A object derived from FieldType, or null</returns>
        public static FieldType Create(dynamic data = null)
        {
            if (data == null || data.name == null)
            {
                return null;
            }

            switch ((string)data.name)
            {
                case "Boolean":
                    return new BooleanType(data);
                case "String":
                    return new StringType(data);
                case "LocalizedString":
                    return new LocalizedStringType(data);
                case "Enum":
                    return new EnumType(data);
                case "LocalizedEnum":
                    return new LocalizedEnumType(data);
                case "Number":
                    return new NumberType(data);
                case "Money":
                    return new MoneyType(data);
                case "Date":
                    return new DateType(data);
                case "Time":
                    return new TimeType(data);
                case "DateTime":
                    return new DateTimeType(data);
                case "Reference":
                    return new ReferenceType(data);
                case "Set":
                    return new SetType(data);
            }

            return null;
        }
    }
}
