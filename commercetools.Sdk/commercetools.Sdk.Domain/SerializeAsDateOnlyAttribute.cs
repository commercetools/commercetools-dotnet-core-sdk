using System;

namespace commercetools.Sdk.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SerializeAsDateOnlyAttribute : Attribute
    {
        public SerializeAsDateOnlyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
    }
}
