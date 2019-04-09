using System;
using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.Domain
{
    public class Reference<T> : Reference, IReferenceable<T>
    {
        public T Obj { get; set; }
        public new ReferenceTypeId TypeId => this.GetResourceType();
    }
}
