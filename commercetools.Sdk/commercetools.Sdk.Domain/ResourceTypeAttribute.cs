using System;

namespace commercetools.Sdk.Domain
{
    public class ResourceTypeAttribute : Attribute
    {
        public ResourceTypeAttribute(ReferenceTypeId value)
        {
            this.Value = value;
        }

        public ReferenceTypeId Value { get; private set; }
    }
}
