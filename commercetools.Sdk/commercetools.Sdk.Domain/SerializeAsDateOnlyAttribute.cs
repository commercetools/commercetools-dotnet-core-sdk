using System;

namespace commercetools.Sdk.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SerializeAsDateOnlyAttribute : Attribute
    {
        public SerializeAsDateOnlyAttribute(params string[] properties)
        {
            Properties = properties;
        }

        public string[] Properties { get; set; }
    }
}
