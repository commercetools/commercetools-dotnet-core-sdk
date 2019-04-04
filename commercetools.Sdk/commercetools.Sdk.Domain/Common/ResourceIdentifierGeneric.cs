using System;

namespace commercetools.Sdk.Domain
{
    public class ResourceIdentifier<T>: ResourceIdentifier
    {
        public new ReferenceTypeId TypeId => this.GetResourceType();

        private ReferenceTypeId GetResourceType()
        {
            ReferenceTypeId referenceTypeId;
            string refTypeName = typeof(T).Name;
            Enum.TryParse(refTypeName, out referenceTypeId);
            return referenceTypeId;
        }
    }
}
