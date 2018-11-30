namespace commercetools.Sdk.Domain
{
    [TypeMarker("set")]
    public class SetAttributeType : AttributeType
    {
        public AttributeType ElementType { get; set; }
    }
}