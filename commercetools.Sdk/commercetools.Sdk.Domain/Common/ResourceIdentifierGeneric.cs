using System;
using System.Linq;
using System.Reflection;

namespace commercetools.Sdk.Domain
{
    public class ResourceIdentifier<T>: ResourceIdentifier, IReferenceable<T>
    {
        public new ReferenceTypeId TypeId => this.GetResourceType();
    }
}
