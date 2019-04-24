using System;

namespace commercetools.Sdk.Domain.CustomObject
{
    [Endpoint("custom-objects")]
    [ResourceType(ReferenceTypeId.KeyValueDocument)]
    public class CustomObject<T> : CustomObjectBase
    {

        public T Value { get; set; }
    }
}
