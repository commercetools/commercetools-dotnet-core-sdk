namespace commercetools.Sdk.Domain
{
    [TypeMarker("nested")]
    public class NestedAttributeType : AttributeType
    {
        public Reference<ProductType> TypeReference { get; set; }
    }
}