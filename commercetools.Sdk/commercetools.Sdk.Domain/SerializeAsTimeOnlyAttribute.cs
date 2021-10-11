using System;

namespace commercetools.Sdk.Domain
{
    [Obsolete("Experimental")]
    [AttributeUsage(AttributeTargets.Class)]
    public class SerializeAsTimeOnlyAttribute : Attribute
    {
        public SerializeAsTimeOnlyAttribute(params string[] properties)
        {
            Properties = properties;
        }

        public string[] Properties { get; set; }
    }
}
