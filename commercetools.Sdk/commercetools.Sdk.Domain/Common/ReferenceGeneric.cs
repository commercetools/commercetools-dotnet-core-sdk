using System;
using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.Domain
{
    public class Reference<T> : Reference
    {
        public T Obj { get; set; }
    }
}
