using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.CustomObjects
{
    [Endpoint("custom-objects")]
    [ResourceType(ReferenceTypeId.KeyValueDocument)]
    public abstract class CustomObjectBase : Resource<CustomObjectBase>
    {
        public string Container { get; set; }

        public string Key { get; set; }
    }
}
