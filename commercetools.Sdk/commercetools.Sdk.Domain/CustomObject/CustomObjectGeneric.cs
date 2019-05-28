using System;

namespace commercetools.Sdk.Domain.CustomObject
{
    public class CustomObject<T> : CustomObjectBase
    {

        public T Value { get; set; }
    }
}
