using System;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain
{
    public class Reference<T> : Reference, IReference<T>
    {
        public virtual T Obj { get; set; }
        public new ReferenceTypeId TypeId => this.GetResourceType();

        public IReference<T> ToReference()
        {
            return this;
        }
    }
}
