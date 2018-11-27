namespace commercetools.Sdk.Domain
{
    [FieldType("Set")]
    public class SetType : FieldType
    {
        public FieldType ElementType { get; set; }
    }
}