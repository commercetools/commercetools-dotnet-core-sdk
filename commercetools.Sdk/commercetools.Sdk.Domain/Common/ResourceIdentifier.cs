namespace commercetools.Sdk.Domain
{
    public class ResourceIdentifier: IReferenceable
    {
        public string Id { get; set; }
        public ReferenceTypeId? TypeId { get; set; }
        public string Key { get; set; }
    }
}
