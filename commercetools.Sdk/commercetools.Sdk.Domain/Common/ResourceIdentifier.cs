using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain
{
    public class ResourceIdentifier: IReference
    {
        public string Id { get; set; }
        public ReferenceTypeId? TypeId { get; set; }
        public string Key { get; set; }
    }
}
