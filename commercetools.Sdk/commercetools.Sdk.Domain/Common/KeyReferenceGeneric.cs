using System;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain
{
    public class KeyReference<T> : KeyReference, IReference<T>
    {
        public new ReferenceTypeId TypeId => this.GetResourceType();

        public IReference<T> ToReference()
        {
            return this;
        }
    }
}
