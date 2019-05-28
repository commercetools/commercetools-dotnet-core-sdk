using System;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.CustomObject
{
    [Endpoint("custom-objects")]
    [ResourceType(ReferenceTypeId.KeyValueDocument)]
    public abstract class CustomObjectBase : Resource<CustomObjectBase>
    {
        public string Container { get; set; }

        public string Key { get; set; }
    }
}
