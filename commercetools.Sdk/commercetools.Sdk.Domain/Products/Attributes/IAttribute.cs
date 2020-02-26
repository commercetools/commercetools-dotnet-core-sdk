namespace commercetools.Sdk.Domain.Products.Attributes
{
    public interface IAttribute
    {
        string Name { get; set; }

        object JsonValue { get; set; }
    }
}
