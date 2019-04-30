using System;
using System.Linq;
using System.Reflection;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain
{
    public class ResourceIdentifier<T>: ResourceIdentifier, IReference<T>
    {
        public new ReferenceTypeId TypeId => this.GetResourceType();

        public IReference<T> ToReference()
        {
            return this;
        }
    }
}
