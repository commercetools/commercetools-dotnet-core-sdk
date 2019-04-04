using System;

namespace commercetools.Sdk.Domain
{
    public class ResourceTypeMarkerAttribute : Attribute
    {
        public ResourceTypeMarkerAttribute(System.Type value)
        {
            this.Value = value;
        }

        public System.Type Value { get; private set; }
    }
}