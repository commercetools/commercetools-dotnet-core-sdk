namespace commercetools.Sdk.Domain
{
    [TypeMarker("reference")]
    public class ReferenceAttributeType : AttributeType
    {
        public ReferenceFieldTypeId ReferenceTypeId { get; set; }
    }
}