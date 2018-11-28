namespace commercetools.Sdk.Domain
{
    [TypeMarker("Set")]
    public class SetType : FieldType
    {
        public FieldType ElementType { get; set; }
    }
}