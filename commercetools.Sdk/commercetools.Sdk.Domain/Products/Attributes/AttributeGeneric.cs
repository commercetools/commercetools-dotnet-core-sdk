namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class Attribute<T> : Attribute
    {
        public T Value { get; set; }
    }
}