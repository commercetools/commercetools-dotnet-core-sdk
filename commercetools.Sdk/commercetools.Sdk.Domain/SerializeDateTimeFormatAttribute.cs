using System;

namespace commercetools.Sdk.Domain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SerializeDateTimeFormatAttribute : Attribute
    {
        public SerializeDateTimeFormatAttribute(string dateTimeFormat)
        {
            this.DateTimeFormat = dateTimeFormat;
        }

        public string DateTimeFormat { get; private set; }
    }
}
