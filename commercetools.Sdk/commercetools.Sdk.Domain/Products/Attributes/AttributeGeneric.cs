namespace commercetools.Sdk.Domain.Products.Attributes
{
    public class Attribute<T> : Attribute, IAttribute
    {
        public T Value { get; set; }

        public object JsonValue { get; set; }

        public bool ShouldSerializeJsonValue()
        {
            return false;
        }
    }
}
