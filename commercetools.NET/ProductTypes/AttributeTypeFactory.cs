using System;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// AttributeTypeFactory
    /// </summary>
    public static class AttributeTypeFactory
    {
        /// <summary>
        /// Creates an AttributeType using JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        /// <returns>A object derived from AttributeType, or null</returns>
        public static AttributeType Create(dynamic data = null)
        {
            if (data == null || data.name == null)
            {
                return null;
            }

            switch ((string)data.name)
            {
                case "boolean":
                    return new BooleanType(data);
                case "text":
                    return new TextType(data);
                case "ltext":
                    return new LocalizableTextType(data);
                case "enum":
                    return new EnumType(data);
                case "lenum":
                    return new LocalizableEnumType(data);
                case "number":
                    return new NumberType(data);
                case "money":
                    return new MoneyType(data);
                case "date":
                    return new DateType(data);
                case "time":
                    return new TimeType(data);
                case "datetime":
                    return new DateTimeType(data);
                case "reference":
                    return new ReferenceType(data);
                case "set":
                    return new SetType(data);
                case "nested":
                    return new NestedType(data);
            }

            return null;
        }
    }
}
