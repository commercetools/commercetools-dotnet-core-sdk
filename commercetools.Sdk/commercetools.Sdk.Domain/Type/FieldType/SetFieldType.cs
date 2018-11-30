namespace commercetools.Sdk.Domain
{
    [TypeMarker("Set")]
    public class SetFieldType : FieldType
    {
        public FieldType ElementType { get; set; }
    }
}