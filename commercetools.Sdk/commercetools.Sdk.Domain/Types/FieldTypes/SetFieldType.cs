namespace commercetools.Sdk.Domain.Types.FieldTypes
{
    [TypeMarker("Set")]
    public class SetFieldType : FieldType
    {
        public FieldType ElementType { get; set; }
    }
}